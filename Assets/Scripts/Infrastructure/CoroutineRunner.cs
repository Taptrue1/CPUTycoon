using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class CoroutineRunner : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public new Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return base.StartCoroutine(coroutine);
        }
        public new void StopCoroutine(Coroutine coroutine)
        {
            base.StopCoroutine(coroutine);
        }
    }
}