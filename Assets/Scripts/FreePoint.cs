namespace WN
{
    using UnityEngine;

    public class FreePoint : MonoBehaviour
    {
        private const int CIRCLE_POINTS = 8;
        
        [SerializeField]
        private CurrentLevelData _currentLevelData = null;

        [SerializeField, Min(0f)]
        private float _windingNumberRadius = 0.5f;
        
        [SerializeField]
        private Vector2 _windingNumberOffset = Vector2.zero;
        
        [SerializeField]
        private bool _mustBeInPolygon = true;

        [SerializeField]
        private RSLib.ParticlesSpawnerPool _onEnableParticles = null;

        [SerializeField]
        private SpriteAnimator _helpSpriteAnimator = null;

        [SerializeField]
        private float _helpDuration = 1f;

        [SerializeField]
        private RSLib.Audio.ClipProvider _helpClipProvider = null;
        
        [SerializeField]
        private UnityEngine.Events.UnityEvent _onStateValid = null;

        [SerializeField]
        private UnityEngine.Events.UnityEvent _onStateInvalid = null;
        
        private bool _isInPolygon;

        public bool MustBeInPolygon => _mustBeInPolygon;
        
        public bool CheckState()
        {
            return _isInPolygon == MustBeInPolygon;
        }

        public void UpdateIsInPolygon()
        {
            bool previousIsInPolygon = _isInPolygon;
            _isInPolygon = _currentLevelData.LevelController != null && AreAllPointsInPolygon();

            if (previousIsInPolygon != _isInPolygon)
            {
                if (CheckState())
                    _onStateValid?.Invoke();
                else
                    _onStateInvalid?.Invoke();
            }
        }

        private bool AreAllPointsInPolygon()
        {
            PolygonController polygonController = _currentLevelData.LevelController.PolygonController;
            
            for (int i = 0; i < CIRCLE_POINTS; ++i)
            {
                float theta = (i * 2 * Mathf.PI) / CIRCLE_POINTS;
                float x = Mathf.Sin(theta) * _windingNumberRadius;
                float y = Mathf.Cos(theta) * _windingNumberRadius;
                Vector3 point = new(x, y);

                if (!polygonController.IsInPolygon(transform.position + point + (Vector3) _windingNumberOffset))
                    return false;
            }

            return true;
        }

        public void OnLevelStarted()
        {
            _helpSpriteAnimator.gameObject.SetActive(false);
            _onEnableParticles.SpawnParticles(transform.position);
        }

        public void OnAskForHelp()
        {
            StartCoroutine(HelpCoroutine());
        }

        private System.Collections.IEnumerator HelpCoroutine()
        {
            RSLib.Audio.AudioManager.PlaySound(_helpClipProvider);

            if (_helpSpriteAnimator.gameObject.activeSelf)
                yield break;
            
            _helpSpriteAnimator.gameObject.SetActive(true);
            yield return RSLib.Yield.SharedYields.WaitForSeconds(_helpDuration);
            _helpSpriteAnimator.gameObject.SetActive(false);
        }
        
        private void Start()
        {
            UpdateIsInPolygon();

            if (CheckState())
                _onStateValid?.Invoke();
            else
                _onStateInvalid?.Invoke();
        }

        private void Update()
        {
            if (_currentLevelData.LevelController?.PolygonController.IsDraggingPoint ?? false)
                UpdateIsInPolygon();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            System.Collections.Generic.List<Vector3> circlePositions = new();
            
            for (int i = 0; i < CIRCLE_POINTS; ++i)
            {
                float theta = (i * 2 * Mathf.PI) / CIRCLE_POINTS;
                float x = Mathf.Sin(theta) * _windingNumberRadius;
                float y = Mathf.Cos(theta) * _windingNumberRadius;
                circlePositions.Add(new Vector3(x, y));
            }

            Gizmos.color = _isInPolygon ? Color.green : Color.red;
            Vector3 sourcePosition = transform.position + (Vector3)_windingNumberOffset;
            
            for (int i = 0; i < circlePositions.Count; ++i)
            {
                Gizmos.DrawLine(sourcePosition + circlePositions[i],
                                sourcePosition + circlePositions[(i + 1) % circlePositions.Count]);
            }
        }
        #endif
    }
}