using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityExtensions;

namespace UnityExtensions.EditorCoroutineExamples
{

    static class Sample
    {

        //[MenuItem("UnityExtensions/EditorCoroutineExamples/Count To Ten")]
        public static void CountToTen()
        {
            EditorCoroutine.Start(CountToTenCoroutine());
        }

        private static IEnumerator CountToTenCoroutine()
        {
            for (var i = 1; i <= 10; ++i)
            {
                Debug.LogFormat("{0}", i);

                // yield until next EditorApplication.update
                yield return null;

                // yield until 1 second has passed
                yield return new WaitForSeconds(1);
            }

            // yield until a subroutine has completed
            yield return CountToTenCompletedSubroutine();
        }

        private static IEnumerator CountToTenCompletedSubroutine()
        {
            Debug.LogFormat("DONE!");

            yield break;
        }

    }

}