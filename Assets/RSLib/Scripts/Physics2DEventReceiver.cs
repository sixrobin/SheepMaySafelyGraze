namespace RSLib
{
    using RSLib.Extensions;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class Physics2DEventReceiver : MonoBehaviour
    {
        [Header("LAYER MASK")]
        [SerializeField] private LayerMask _mask = 0;

        [Header("EVENTS")]
        [SerializeField] private Framework.Events.Collider2DEvent _onTriggerEnter = null;
        [SerializeField] private Framework.Events.Collider2DEvent _onTriggerExit = null;
        [SerializeField] private Framework.Events.Collision2DEvent _onCollisionEnter = null;
        [SerializeField] private Framework.Events.Collision2DEvent _onCollisionExit = null;

        public delegate void Collider2DEventHandler(Collider2D collider);
        public delegate void Collision2DEventHandler(Collision2D collider);

        public event Collider2DEventHandler TriggerEntered;
        public event Collider2DEventHandler TriggerExit;
        public event Collision2DEventHandler CollisionEntered;
        public event Collision2DEventHandler CollisionExit;

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (!_mask.HasLayer(collider.gameObject.layer))
                return;

            TriggerEntered?.Invoke(collider);
            _onTriggerEnter?.Invoke(collider);
        }

        protected virtual void OnTriggerExit2D(Collider2D collider)
        {
            if (!_mask.HasLayer(collider.gameObject.layer))
                return;

            TriggerExit?.Invoke(collider);
            _onTriggerExit?.Invoke(collider);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_mask.HasLayer(collision.gameObject.layer))
                return;

            CollisionEntered?.Invoke(collision);
            _onCollisionEnter?.Invoke(collision);
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            if (!_mask.HasLayer(collision.gameObject.layer))
                return;

            CollisionExit?.Invoke(collision);
            _onCollisionExit?.Invoke(collision);
        }

        private void Awake()
        {
            if (!GetComponent<Collider2D>())
                UnityEngine.Debug.LogError($"{GetType().Name} instance on {transform.name} does not have a Collider2D and can then not be triggered.", gameObject);
        }
    }
}