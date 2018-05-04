# `UnityExtensions.EditorCoroutine`

The `EditorCoroutine` class implements a disposable coroutine runner that will
execute one iteration on each `EditorApplication.update` callback.

If the coroutine `yields` another `IEnumerator`, that subroutine will be 
iterated to completion before the parent coroutine continues.

**NOTE:** There is explicit support for `UnityEngine.WaitForSeconds`, but other
`UnityEngine.YieldInstruction` subclasses will not function as documented when
running in the editor.

```cs
static class Sample
{

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
```

# License

## [The MIT License (MIT)](https://mit-license.org)
Copyright © 2018

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.