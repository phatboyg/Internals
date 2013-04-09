// Copyright 2011-2013 Chris Patterson, Dru Sellers
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Internals.Tasks
{
#if !NET35

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    static class TaskUtil
    {
        static readonly Task _cachedCompleted = Completed(default(Unit));

        internal static Task Canceled()
        {
            return CancelCache<Unit>.CanceledTask;
        }

        internal static Task FastUnwrap(this Task<Task> task)
        {
            Task innerTask = task.Status == TaskStatus.RanToCompletion
                                 ? task.Result
                                 : null;
            return innerTask ?? task.Unwrap();
        }

        internal static Task<T> FastUnwrap<T>(this Task<Task<T>> task)
        {
            Task<T> innerTask = task.Status == TaskStatus.RanToCompletion
                                    ? task.Result
                                    : null;
            return innerTask ?? task.Unwrap();
        }


        internal static Task Completed()
        {
            return _cachedCompleted;
        }

        internal static Task<T> Completed<T>(T result)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(result);
            return tcs.Task;
        }

        internal static Task Faulted(Exception exception)
        {
            return Faulted<Unit>(exception);
        }

        internal static Task<T> Faulted<T>(Exception exception)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        internal static Task Faulted(IEnumerable<Exception> exceptions)
        {
            return Faulted<Unit>(exceptions);
        }

        internal static Task<T> Faulted<T>(IEnumerable<Exception> exceptions)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(exceptions);
            return tcs.Task;
        }

        internal static Task RunSynchronously(Action action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return Canceled();

            try
            {
                action();
                return Completed();
            }
            catch (Exception ex)
            {
                return Faulted(ex);
            }
        }

        internal static Task<T> RunSynchronously<T>(T value, Action<T> action,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return CancelCache<T>.CanceledTask;

            try
            {
                action(value);
                return Completed(value);
            }
            catch (Exception ex)
            {
                return Faulted<T>(ex);
            }
        }

        internal static Task<T> RunSynchronously<T>(Func<T> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                return CancelCache<T>.CanceledTask;

            try
            {
                T result = action();
                return Completed(result);
            }
            catch (Exception ex)
            {
                return Faulted<T>(ex);
            }
        }

        internal static bool TrySetFromTask<T>(this TaskCompletionSource<T> source, Task task)
        {
            if (task.Status == TaskStatus.Canceled)
                return source.TrySetCanceled();

            if (task.Status == TaskStatus.Faulted)
                return source.TrySetException(task.Exception.InnerExceptions);

            if (task.Status == TaskStatus.RanToCompletion)
            {
                var taskOfT = task as Task<T>;
                return source.TrySetResult(taskOfT != null
                                               ? taskOfT.Result
                                               : default(T));
            }

            return false;
        }

        internal static void MarkObserved(this Task task)
        {
            if (!task.IsCompleted)
                return;

            Exception unused = task.Exception;
        }

        /// <summary>
        ///     Creates a task that executes after the input task
        /// </summary>
        /// <param name="task"></param>
        /// <param name="continuationTask"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="runSynchronously"></param>
        /// <returns></returns>
        internal static Task Then(this Task task, Func<Task> continuationTask, CancellationToken cancellationToken,
            bool runSynchronously = true)
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                    return Faulted(task.Exception.InnerExceptions);

                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                    return Canceled();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        return continuationTask();
                    }
                    catch (Exception ex)
                    {
                        return Faulted(ex);
                    }
                }
            }

            return ThenAsync(task, continuationTask, cancellationToken, runSynchronously);
        }

        internal static Task<T> RunSynchronously<T>(this Task<T> task, Action<T> continuation,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return task.Then(t => RunSynchronously(t.Result, x => continuation(t.Result)), cancellationToken);
        }

        static Task ThenAsync(Task task, Func<Task> continuationTask, CancellationToken cancellationToken,
            bool runSynchronously)
        {
            var source = new TaskCompletionSource<Task>();
            task.ContinueWith(innerTask =>
                {
                    if (innerTask.IsFaulted)
                        source.TrySetException(innerTask.Exception.InnerExceptions);
                    else if (innerTask.IsCanceled || cancellationToken.IsCancellationRequested)
                        source.TrySetCanceled();
                    else
                        source.TrySetResult(continuationTask());
                }, runSynchronously
                       ? TaskContinuationOptions.ExecuteSynchronously
                       : TaskContinuationOptions.None);

            return source.Task.FastUnwrap();
        }

        internal static Task<TResult> Then<T, TResult>(this T task, Func<T, Task<TResult>> continuationTask,
            CancellationToken cancellationToken, bool runSynchronously = true)
            where T : Task
        {
            if (task.IsCompleted)
            {
                if (task.IsFaulted)
                    return Faulted<TResult>(task.Exception.InnerExceptions);

                if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                    return CancelCache<TResult>.CanceledTask;

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        return continuationTask(task);
                    }
                    catch (Exception ex)
                    {
                        return Faulted<TResult>(ex);
                    }
                }
            }

            return ThenAsync(task, continuationTask, cancellationToken, runSynchronously);
        }

        static Task<TResult> ThenAsync<T, TResult>(T task, Func<T, Task<TResult>> continuationTask, CancellationToken cancellationToken,
            bool runSynchronously)
            where T : Task
        {
            var source = new TaskCompletionSource<Task<TResult>>();
            task.ContinueWith(innerTask =>
                {
                    if (innerTask.IsFaulted)
                        source.TrySetException(innerTask.Exception.InnerExceptions);
                    else if (innerTask.IsCanceled || cancellationToken.IsCancellationRequested)
                        source.TrySetCanceled();
                    else
                        source.TrySetResult(continuationTask(task));
                }, runSynchronously
                       ? TaskContinuationOptions.ExecuteSynchronously
                       : TaskContinuationOptions.None);

            return source.Task.FastUnwrap();
        }


        internal static Task<T> ForEach<T>(Task<T> initialTask, IEnumerable<Func<Task<T>, Task<T>>> tasks,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return tasks.Aggregate(initialTask, (current, tasker) => current.Then(x => tasker(x), cancellationToken));
            }
            catch (Exception ex)
            {
                return Faulted<T>(ex);
            }
        }


        static class CancelCache<T>
        {
            public static readonly Task<T> CanceledTask = GetCanceledTask();

            static Task<T> GetCanceledTask()
            {
                var source = new TaskCompletionSource<T>();
                source.SetCanceled();
                return source.Task;
            }
        }


        struct Unit
        {
        }
    }
#endif
}