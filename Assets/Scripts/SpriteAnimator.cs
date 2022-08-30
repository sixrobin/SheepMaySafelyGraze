using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;
    
    [SerializeField]
    private Sprite[] _sprites = null;

    [SerializeField, Min(1)]
    private int _frameRate = 12;

    [SerializeField]
    private bool _randomizeFirstSprite = true;
    
    public bool Paused;
    
    private int _currentSpriteIndex;
    private float _timer;

    private void Start()
    {
        if (_randomizeFirstSprite)
            _currentSpriteIndex = Random.Range(0, _sprites.Length);
        
        _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
    }

    private void Update()
    {
        if (Paused)
            return;
    
        _timer += Time.deltaTime;
        if (_timer < (1f / _frameRate))
            return;
        
        if (_sprites.Length > 0)
        {
            _currentSpriteIndex = ++_currentSpriteIndex % _sprites.Length;
            _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
            _timer = 0f;
        }
    }
}
