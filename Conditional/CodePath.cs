namespace Internals.Conditional
{
    using System;

    interface Mode
    {
        bool Enabled { get; }
    }

    static class CodePath<T>
        where T : class, Mode, new()
    {
        // ReSharper disable StaticFieldInGenericType
        static Mode _flag;
        // ReSharper restore StaticFieldInGenericType

        static Mode Flag
        {
            get { return _flag ?? (_flag = new T()); }
        }

        public static void Set(Mode flag)
        {
            _flag = flag;
        }

        public static void Enable()
        {
            _flag = new Enabled();
        }

        public static void Disable()
        {
            _flag = new Disabled();
        }

        class Disabled :
            Mode
        {
            bool Mode.Enabled
            {
                get { return false; }
            }
        }

        class Enabled :
            Mode
        {
            bool Mode.Enabled
            {
                get { return true; }
            }
        }

        public static void If(Action enabledMethod)
        {
            if (Flag.Enabled)
                enabledMethod();
        }

        public static void Unless(Action disabledMethod)
        {
            if (!Flag.Enabled)
                disabledMethod();
        }

        public static void If(Action enabledMethod, Action disabledMethod)
        {
            if (Flag.Enabled)
                enabledMethod();
            else
                disabledMethod();
        }


        public static void If<T1>(Action<T1> enabledMethod, T1 arg1)
        {
            if (Flag.Enabled)
                enabledMethod(arg1);
        }

        public static void Unless<T1>(Action<T1> disabledMethod, T1 arg1)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1);
        }

        public static void If<T1>(Action<T1> enabledMethod, Action<T1> disabledMethod, T1 arg1)
        {
            if (Flag.Enabled)
                enabledMethod(arg1);
            else
                disabledMethod(arg1);
        }

#if !NET35
        public static void If<T1, T2>(Action<T1, T2> enabledMethod, T1 arg1, T2 arg2)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2);
        }

        public static void Unless<T1, T2>(Action<T1, T2> disabledMethod, T1 arg1, T2 arg2)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2);
        }

        public static void If<T1, T2>(Action<T1, T2> enabledMethod, Action<T1, T2> disabledMethod, T1 arg1, T2 arg2)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2);
            else
                disabledMethod(arg1, arg2);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3>(Action<T1, T2, T3> enabledMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3);
        }

        public static void Unless<T1, T2, T3>(Action<T1, T2, T3> disabledMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3);
        }

        public static void If<T1, T2, T3>(Action<T1, T2, T3> enabledMethod, Action<T1, T2, T3> disabledMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3);
            else
                disabledMethod(arg1, arg2, arg3);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4>(Action<T1, T2, T3, T4> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4);
        }

        public static void Unless<T1, T2, T3, T4>(Action<T1, T2, T3, T4> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4);
        }

        public static void If<T1, T2, T3, T4>(Action<T1, T2, T3, T4> enabledMethod, Action<T1, T2, T3, T4> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4);
            else
                disabledMethod(arg1, arg2, arg3, arg4);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5);
        }

        public static void Unless<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5);
        }

        public static void If<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> enabledMethod, Action<T1, T2, T3, T4, T5> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static void If<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> enabledMethod, Action<T1, T2, T3, T4, T5, T6> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
#endif
#if !NET35
        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> enabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }

        public static void Unless<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (!Flag.Enabled)
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }

        public static void If<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> enabledMethod, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> disabledMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (Flag.Enabled)
                enabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            else
                disabledMethod(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
#endif

        public static T Iff<T>(Func<T> enabledFunction, Func<T> disabledFunction)
        {
            if (Flag.Enabled)
                return enabledFunction();

            return disabledFunction();
        }


        public static T Iff<T1, T>(Func<T1, T> enabledFunction, Func<T1, T> disabledFunction, T1 arg1)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1);

            return disabledFunction(arg1);
        }

#if !NET35
        public static T Iff<T1, T2, T>(Func<T1, T2, T> enabledFunction, Func<T1, T2, T> disabledFunction, T1 arg1, T2 arg2)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2);

            return disabledFunction(arg1, arg2);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T>(Func<T1, T2, T3, T> enabledFunction, Func<T1, T2, T3, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3);

            return disabledFunction(arg1, arg2, arg3);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T>(Func<T1, T2, T3, T4, T> enabledFunction, Func<T1, T2, T3, T4, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4);

            return disabledFunction(arg1, arg2, arg3, arg4);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T>(Func<T1, T2, T3, T4, T5, T> enabledFunction, Func<T1, T2, T3, T4, T5, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T>(Func<T1, T2, T3, T4, T5, T6, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T>(Func<T1, T2, T3, T4, T5, T6, T7, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
#endif
#if !NET35
        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T> enabledFunction, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
#endif

#if !NET35
        public delegate T OFunc<T1, out T>(out T1 arg1);
#else
        public delegate T OFunc<T1, T>(out T1 arg1);
#endif

        public static T Iff<T1, T>(OFunc<T1, T> enabledFunction, OFunc<T1, T> disabledFunction, out T1 arg1)
        {
            if (Flag.Enabled)
                return enabledFunction(out arg1);

            return disabledFunction(out arg1);
        }

#if !NET35
        public delegate T OFunc<in T1, T2, out T>(T1 arg1, out T2 arg2);
#else
        public delegate T OFunc<T1, T2, T>(T1 arg1, out T2 arg2);
#endif

        public static T Iff<T1, T2, T>(OFunc<T1, T2, T> enabledFunction, OFunc<T1, T2, T> disabledFunction, T1 arg1, out T2 arg2)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, out arg2);

            return disabledFunction(arg1, out arg2);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, T3, out T>(T1 arg1, T2 arg2, out T3 arg3);
#else
        public delegate T OFunc<T1, T2, T3, T>(T1 arg1, T2 arg2, out T3 arg3);
#endif

        public static T Iff<T1, T2, T3, T>(OFunc<T1, T2, T3, T> enabledFunction, OFunc<T1, T2, T3, T> disabledFunction, T1 arg1, T2 arg2, out T3 arg3)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, out arg3);

            return disabledFunction(arg1, arg2, out arg3);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, T4, out T>(T1 arg1, T2 arg2, T3 arg3, out T4 arg4);
#else
        public delegate T OFunc<T1, T2, T3, T4, T>(T1 arg1, T2 arg2, T3 arg3, out T4 arg4);
#endif

        public static T Iff<T1, T2, T3, T4, T>(OFunc<T1, T2, T3, T4, T> enabledFunction, OFunc<T1, T2, T3, T4, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, out T4 arg4)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, out arg4);

            return disabledFunction(arg1, arg2, arg3, out arg4);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, T5, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out T5 arg5);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out T5 arg5);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T>(OFunc<T1, T2, T3, T4, T5, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out T5 arg5)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, out arg5);

            return disabledFunction(arg1, arg2, arg3, arg4, out arg5);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, T6, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out T6 arg6);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out T6 arg6);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T>(OFunc<T1, T2, T3, T4, T5, T6, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out T6 arg6)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, out arg6);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, out arg6);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, T7, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out T7 arg7);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out T7 arg7);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out T7 arg7)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, out arg7);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, out arg7);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, T8, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out T8 arg8);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out T8 arg8);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out T8 arg8)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, out arg8);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, out arg8);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, T9, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out T9 arg9);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out T9 arg9);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out T9 arg9)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, out arg9);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, out arg9);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, T10, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out T10 arg10);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out T10 arg10);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out T10 arg10)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, out arg10);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, out arg10);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, T11, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out T11 arg11);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out T11 arg11);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out T11 arg11)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, out arg11);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, out arg11);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, T12, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out T12 arg12);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out T12 arg12);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out T12 arg12)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, out arg12);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, out arg12);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, T13, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out T13 arg13);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out T13 arg13);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out T13 arg13)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, out arg13);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, out arg13);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, T14, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out T14 arg14);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out T14 arg14);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out T14 arg14)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, out arg14);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, out arg14);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, T15, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out T15 arg15);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out T15 arg15);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out T15 arg15)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, out arg15);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, out arg15);
        }

#if !NET35
        public delegate T OFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, T16, out T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out T16 arg16);
#else
        public delegate T OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out T16 arg16);
#endif

        public static T Iff<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T>(OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T> enabledFunction, OFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T> disabledFunction, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out T16 arg16)
        {
            if (Flag.Enabled)
                return enabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, out arg16);

            return disabledFunction(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, out arg16);
        }

    }
}
