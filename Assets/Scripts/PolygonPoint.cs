namespace WN
{
    using RSLib;
    using UnityEngine;

    public class PolygonPoint : MonoBehaviour
    {
        [SerializeField]
        private Transform _lineEdge = null;

        [SerializeField]
        private Transform _centerPivot = null;
        
        [SerializeField]
        private float _draggedMotionAngle = 90f;
        
        [SerializeField]
        private float _draggedMotionStep = 0.2f;

        [SerializeField]
        private ParticlesSpawnerPool _pointSetParticles = null;

        [SerializeField]
        private UnityEngine.Events.UnityEvent _onDragBegin = null;

        [SerializeField]
        private UnityEngine.Events.UnityEvent _onDragOver = null;
        
        public Vector3 Position => transform.position;
        public Vector3 LineEdgePosition => _lineEdge != null ? _lineEdge.position : Position;

        public void OnDragBegin()
        {
            _onDragBegin?.Invoke();

            StartCoroutine(DraggedMotionCoroutine());
        }
        
        public void OnDragOver()
        {
            _onDragOver?.Invoke();
            
            _pointSetParticles.SpawnParticles(Position);
            StopAllCoroutines();
            _centerPivot.transform.localEulerAngles = Vector3.zero;
        }

        private System.Collections.IEnumerator DraggedMotionCoroutine()
        {
            Vector3 eulerAngles = new Vector3(0f, 0f, _draggedMotionAngle * 0.5f);
            
            while (true)
            {
                eulerAngles.z *= -1;
                _centerPivot.transform.localEulerAngles = eulerAngles;
                yield return RSLib.Yield.SharedYields.WaitForSeconds(_draggedMotionStep);
            }
        }
        
        private void OnEnable()
        {
            _pointSetParticles.SpawnParticles(Position);
        }
    }
}
