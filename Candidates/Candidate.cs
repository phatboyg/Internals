namespace Internals.Candidates
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A single value candidate for conditional checks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface Candidate<out T>
    {
        Candidate<T> Case(Func<T, bool> predicate, Action<T> continuation);
        void Else(Action<T> continuation);
    }

    /// <summary>
    /// An enumeration of candidates waiting for an initial match
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface Candidates<out T>
    {
        Candidate<T, TResult> Case<TResult>(Func<T, bool> predicate, Func<T, TResult> selector);
        TResult Else<TResult>(Func<T, TResult> selector);
    }

    /// <summary>
    /// An enumeration of candidates with a result time for selecting out the enumeration
    /// </summary>
    /// <typeparam name="T">The source enumeration type</typeparam>
    /// <typeparam name="TResult">The resulting enumeration type</typeparam>
    interface Candidate<out T, TResult>
    {
        Candidate<T, TResult> Case(Func<T, bool> predicate, Func<T, TResult> selector);
        TResult Else(Func<T, TResult> selector);
    }

    static class Candidate
    {
        internal static IEnumerable<Candidate<T, TResult>> Case<T, TResult>(this IEnumerable<T> source,
            Func<T, bool> predicate, Func<T, TResult> selector)
        {
            if (source == null)
                return Cached<T>.Result<TResult>.Empty;

            return new FirstCandidate<T, TResult>(new UnmatchedCandidates<T>(source), predicate, selector);
        }

        internal static IEnumerable<Candidate<T, TResult>> Case<T, TResult>(
            this IEnumerable<Candidate<T, TResult>> source, Func<T, bool> predicate,
            Func<T, TResult> selector)
        {
            if (source == null)
                return Cached<T>.Result<TResult>.Empty;

            return new NextCandidate<T, TResult>(source, predicate, selector);
        }

        internal static IEnumerable<TResult> Else<T, TResult>(this IEnumerable<Candidate<T, TResult>> source,
            Func<T, TResult> selector)
        {
            if (source == null)
                return Enumerable.Empty<TResult>();

            return new LastCandidate<T, TResult>(source, selector);
        }

        internal static Candidate<T> Case<T>(this T value, Func<T, bool> predicate)
        {
            if (predicate(value))
                return Cached<T>.Matched;

            return new UnmatchedCandidate<T>(value);
        }

        internal static Candidate<T> Case<T>(this T value, Func<T, bool> predicate, Action<T> continuation)
        {
            if (predicate(value))
            {
                continuation(value);
                return Cached<T>.Matched;
            }

            return new UnmatchedCandidate<T>(value);
        }

        static Candidate<T> Completed<T>()
        {
            return Cached<T>.Matched;
        }

        static Candidate<T, TResult> Completed<T, TResult>(TResult result)
        {
            return new MatchedCandidate<T, TResult>(result);
        }


        static class Cached<T>
        {
            public static readonly Candidate<T> Matched = GetMatchedCandidate();

            static Candidate<T> GetMatchedCandidate()
            {
                return new MatchedCandidate<T>();
            }


            internal static class Result<TResult>
            {
                public static readonly IEnumerable<Candidate<T, TResult>> Empty = GetEmptyCandidates();

                static IEnumerable<Candidate<T, TResult>> GetEmptyCandidates()
                {
                    return Enumerable.Empty<Candidate<T, TResult>>();
                }
            }
        }


        class FirstCandidate<T, TResult> :
            IEnumerable<Candidate<T, TResult>>
        {
            readonly Func<T, bool> _predicate;
            readonly Func<T, TResult> _selector;
            readonly IEnumerable<Candidates<T>> _source;

            public FirstCandidate(IEnumerable<Candidates<T>> source, Func<T, bool> predicate, Func<T, TResult> selector)
            {
                _source = source;
                _predicate = predicate;
                _selector = selector;
            }

            public IEnumerator<Candidate<T, TResult>> GetEnumerator()
            {
                return _source.Select(x => x.Case(_predicate, _selector)).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        class InitialCandidates<T> :
            Candidates<T>
        {
            readonly T _value;

            public InitialCandidates(T value)
            {
                _value = value;
            }

            public Candidate<T, TResult> Case<TResult>(Func<T, bool> predicate, Func<T, TResult> selector)
            {
                if (predicate(_value))
                    return Completed<T, TResult>(selector(_value));

                return new UnmatchedCandidate<T, TResult>(_value);
            }

            public TResult Else<TResult>(Func<T, TResult> selector)
            {
                return selector(_value);
            }
        }


        class LastCandidate<T, TResult> :
            IEnumerable<TResult>
        {
            readonly Func<T, TResult> _selector;
            readonly IEnumerable<Candidate<T, TResult>> _source;

            public LastCandidate(IEnumerable<Candidate<T, TResult>> source, Func<T, TResult> selector)
            {
                _source = source;
                _selector = selector;
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return _source.Select(candidate => candidate.Else(_selector)).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        class MatchedCandidate<T> :
            Candidate<T>
        {
            Candidate<T> Candidate<T>.Case(Func<T, bool> predicate, Action<T> continuation)
            {
                return this;
            }

            void Candidate<T>.Else(Action<T> continuation)
            {
            }
        }


        class MatchedCandidate<T, TResult> :
            Candidate<T, TResult>
        {
            readonly TResult _result;

            public MatchedCandidate(TResult result)
            {
                _result = result;
            }

            Candidate<T, TResult> Candidate<T, TResult>.Case(Func<T, bool> predicate, Func<T, TResult> selector)
            {
                return this;
            }

            TResult Candidate<T, TResult>.Else(Func<T, TResult> selector)
            {
                return _result;
            }
        }


        class NextCandidate<T, TResult> :
            IEnumerable<Candidate<T, TResult>>
        {
            readonly Func<T, bool> _predicate;
            readonly Func<T, TResult> _selector;
            readonly IEnumerable<Candidate<T, TResult>> _source;

            public NextCandidate(IEnumerable<Candidate<T, TResult>> source, Func<T, bool> predicate,
                Func<T, TResult> selector)
            {
                _source = source;
                _predicate = predicate;
                _selector = selector;
            }

            public IEnumerator<Candidate<T, TResult>> GetEnumerator()
            {
                return _source.Select(candidate => candidate.Case(_predicate, _selector)).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        class UnmatchedCandidate<T> :
            Candidate<T>
        {
            readonly T _value;

            public UnmatchedCandidate(T value)
            {
                _value = value;
            }

            Candidate<T> Candidate<T>.Case(Func<T, bool> predicate, Action<T> continuation)
            {
                if (predicate(_value))
                {
                    continuation(_value);
                    return Completed<T>();
                }

                return this;
            }

            void Candidate<T>.Else(Action<T> continuation)
            {
                continuation(_value);
            }
        }


        class UnmatchedCandidate<T, TResult> :
            Candidate<T, TResult>
        {
            readonly T _value;

            public UnmatchedCandidate(T value)
            {
                _value = value;
            }

            Candidate<T, TResult> Candidate<T, TResult>.Case(Func<T, bool> predicate, Func<T, TResult> selector)
            {
                if (predicate(_value))
                    return Completed<T, TResult>(selector(_value));

                return this;
            }

            TResult Candidate<T, TResult>.Else(Func<T, TResult> selector)
            {
                return selector(_value);
            }
        }


        class UnmatchedCandidates<T> :
            IEnumerable<Candidates<T>>
        {
            readonly IEnumerable<T> _source;

            public UnmatchedCandidates(IEnumerable<T> source)
            {
                _source = source;
            }

            public IEnumerator<Candidates<T>> GetEnumerator()
            {
                return _source.Select(x => new InitialCandidates<T>(x)).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}