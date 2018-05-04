using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityExtensions
{

    public class EditorCoroutine : IDisposable
    {

        private readonly Stack<IEnumerator> _stack = new Stack<IEnumerator>();

        //======================================================================

        public EditorCoroutine(IEnumerator routine)
        {
            Push(routine);
        }

        void IDisposable.Dispose()
        {
            Stop();
        }

        //----------------------------------------------------------------------

        public static EditorCoroutine Start(IEnumerator routine)
        {
            return new EditorCoroutine(routine).Start();
        }

        public EditorCoroutine Start()
        {
            if (_stack.Count > 0)
            {
                EditorApplication.update -= Update;
                EditorApplication.update += Update;
            }
            return this;
        }

        public void Stop()
        {
            EditorApplication.update -= Update;
        }

        //======================================================================

        private void Update()
        {
            try { MoveNext(); }
            catch (Exception ex) { Stop(); Debug.LogError(ex); }
        }

        //----------------------------------------------------------------------

        private void MoveNext()
        {
            var routine = _stack.Peek();
            if (routine.MoveNext())
                Push(routine.Current);
            else
                Pop();
        }

        //----------------------------------------------------------------------

        private void Push(object subroutine)
        {
            Push(subroutine as IEnumerator ?? WaitForSeconds(subroutine));
        }

        private void Push(IEnumerator subroutine)
        {
            if (subroutine != null) _stack.Push(subroutine);
        }

        //----------------------------------------------------------------------

        private void Pop()
        {
            _stack.Pop();
            if (_stack.Count == 0) Stop();
        }

        //======================================================================

        private static readonly FieldInfo
        WaitForSeconds_m_Seconds =
            typeof(UnityEngine.WaitForSeconds)
            .GetField(
                "m_Seconds",
                BindingFlags.Instance |
                BindingFlags.NonPublic
            );

        private static IEnumerator WaitForSeconds(object subroutine)
        {
            var waitForSeconds = subroutine as UnityEngine.WaitForSeconds;
            if (waitForSeconds == null)
                return null;

            var seconds =
                (float)
                WaitForSeconds_m_Seconds
                .GetValue(waitForSeconds);

            return WaitForSeconds(seconds);
        }

        private static IEnumerator WaitForSeconds(float seconds)
        {
            var end = DateTime.Now + TimeSpan.FromSeconds(seconds);
            do yield return null; while (DateTime.Now < end);
        }

    }

}