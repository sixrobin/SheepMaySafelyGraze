namespace WN
{
    using UnityEngine;

    public class PressAnyKey : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onAnyKeyDown = null;

        public void StartInputListening()
        {
            StartCoroutine(ListenToInputCoroutine());
        }

        private System.Collections.IEnumerator ListenToInputCoroutine()
        {
            yield return RSLib.Yield.SharedYields.WaitForEndOfFrame;
            yield return new WaitUntil(() => Input.anyKeyDown);
            _onAnyKeyDown?.Invoke();
        }
    }
}