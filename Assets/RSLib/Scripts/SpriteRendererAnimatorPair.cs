namespace RSLib
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [DisallowMultipleComponent]
    public sealed class SpriteRendererAnimatorPair : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        [SerializeField] private Animator _animator = null;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Animator Animator => _animator;

        private void GetMissingComponents()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_animator == null)
                _animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            GetMissingComponents();

            if (SpriteRenderer == null)
                UnityEngine.Debug.LogWarning($"SpriteRenderer is missing on {GetType().Name} attached to {transform.name}.", gameObject);

            if (Animator == null)
                UnityEngine.Debug.LogWarning($"Animator is missing on {GetType().Name} attached to {transform.name}.", gameObject);
        }

        private void Reset()
        {
            GetMissingComponents();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpriteRendererAnimatorPair))]
    public sealed class SpriteRendererAnimatorPairEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Used to reference both a SpriteRenderer and an Animator, as both are often together, " +
                "and it can avoid doing two loops on both components, but instead do only one on this component.", MessageType.Info);

            EditorGUILayout.Space(10f);

            base.OnInspectorGUI();
        }
    }
#endif
}