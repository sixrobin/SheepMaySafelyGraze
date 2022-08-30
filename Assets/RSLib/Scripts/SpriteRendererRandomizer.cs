namespace LDJAM50
{
    using UnityEngine;

    /// <summary>
    /// Used to randomize some SpriteRenderer values like sprite, flip and color.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererRandomizer : MonoBehaviour
    {
        [Header("REFS")]
        [SerializeField] private SpriteRenderer _spriteRenderer = null;

        [Header("SPRITE (Let empty not to change sprite)")]
        [SerializeField] private Sprite[] _sprites = null;
        
        [Header("FLIP")]
        [SerializeField, Range(0f, 1f)] private float _flipXChance = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _flipYChance = 0.5f;

        [Header("COLOR")]
        [SerializeField] private Color _colorA = Color.white;
        [SerializeField] private Color _colorB = Color.white;

        [ContextMenu("Randomize")]
        public void Randomize()
        {
            _spriteRenderer.flipX = Random.value < _flipXChance;
            _spriteRenderer.flipY = Random.value < _flipYChance;
            _spriteRenderer.color = Color.Lerp(_colorA, _colorB, Random.value);

            if (_sprites.Length > 0)
            {
                _spriteRenderer.sprite = _sprites[Random.Range(0, _sprites.Length)];
            }            
        }
        
        private void Awake()
        {
            if (_spriteRenderer != null || TryGetComponent(out _spriteRenderer))
                Randomize();
        }

        private void Reset()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}