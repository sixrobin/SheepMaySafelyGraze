namespace WN
{
    using UnityEngine;

    public class InputHelper : MonoBehaviour
    {
        [SerializeField]
        protected Camera _mainCamera;

        [SerializeField]
        protected Vector2 _offset = Vector2.zero;
     
        [SerializeField]
        protected SpriteRenderer _helperSpriteRenderer = null;

        [SerializeField]
        protected TMPro.TextMeshProUGUI _helperText = null;

        [SerializeField]
        protected CurrentLevelData _currentLevelData = null;

        private void Awake()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
        }

        protected virtual void Update()
        {
            Vector3 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            position += (Vector3)_offset;
            position.z = 0f;
            transform.position = position;
        }
    }
}