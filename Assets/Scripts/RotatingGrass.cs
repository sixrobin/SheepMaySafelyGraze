namespace WN
{
    using RSLib.Extensions;
    using UnityEngine;

    public class RotatingGrass : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _angle = 3f;

        [SerializeField, Min(0f)]
        private float _speed = 1f;

        [SerializeField]
        private bool _negative = false;
        
        private float _initRotation;
        private float _timer;
        
        private void Rotate()
        {
            _timer += Time.deltaTime * _speed;
            transform.SetEulerAnglesZ(Mathf.Sin(_timer) * (_angle * 0.5f) * (_negative ? -1f : 1f));
        }
        
        private void Awake()
        {
            _timer = Random.value;
            _initRotation = transform.localEulerAngles.z;
        }

        private void Update()
        {
            Rotate();
        }
    }
}