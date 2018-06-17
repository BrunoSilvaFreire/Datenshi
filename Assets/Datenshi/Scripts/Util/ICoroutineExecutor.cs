using System.Collections;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public interface ICoroutineExecutor {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}