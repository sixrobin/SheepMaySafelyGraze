namespace WN
{
    using System.Linq;
    using RSLib.Extensions;
    using UnityEngine;

    public class PolygonController : MonoBehaviour
    {
        [SerializeField]
        private RSLib.Framework.Events.GameEvent _pointAddedEvent = null;

        [SerializeField]
        private RSLib.Framework.Events.GameEvent _pointDeletedEvent = null;

        [SerializeField]
        private RSLib.Framework.Events.GameEvent _pointDraggedEvent = null;
        
        [SerializeField]
        private RSLib.Framework.Events.GameEvent _pointMovedEvent = null;

        [SerializeField]
        private PolygonPoint _polygonPointPrefab = null;

        [SerializeField]
        private BoxCollider2D _drawZone = null;
        
        [SerializeField, Min(0f)]
        private float _clickOnPointDistance = 0.2f;

        [Header("AUDIO")]
        [SerializeField]
        private RSLib.Audio.ClipProvider _addClipProvider = null;
        
        [SerializeField]
        private RSLib.Audio.ClipProvider _removeClipProvider = null;

        [SerializeField]
        private RSLib.Audio.ClipProvider _dragBeginClipProvider = null;

        [SerializeField]
        private RSLib.Audio.ClipProvider _dragOverClipProvider = null;
        
        private Camera _mainCamera;
        
        public event System.Action<PolygonPoint> PointAdded;
        public event System.Action<PolygonPoint> PointDeleted;
        public event System.Action<PolygonPoint> PointDragBegin;
        public event System.Action<PolygonPoint> PointDragOver;
        
        [HideInInspector]
        public int MaxPointsCount = 5;
        
        public System.Collections.Generic.List<PolygonPoint> Polygon { get; } = new();

        public PolygonPoint PointToDelete { get; private set; }
        public PolygonPoint PointToDrag { get; private set; }

        public Vector3 PointToDragOrigin { get; private set; }
        public Vector3 PointToDragLineEdgeOrigin { get; private set; }
        
        public bool IsDraggingPoint => PointToDrag != null;

        public System.Collections.Generic.List<Vector3> GetIntersections()
        {
            System.Collections.Generic.List<Vector3> intersections = new();

            for (int i = 0; i < Polygon.Count - 1; ++i)
            {
                for (int j = i + 2; j < Polygon.Count; ++j)
                {
                    Vector3 a1 = Polygon[i].Position;
                    Vector3 a2 = Polygon[i + 1].Position;
                    Vector3 b1 = Polygon[j].Position;
                    Vector3 b2 = Polygon[(j + 1) % Polygon.Count].Position;

                    if (RSLib.Maths.Geometry.ComputeSegmentsIntersection(a1, a2, b1, b2, out Vector2 intersection))
                        if (Vector3.Distance(intersection, b2) > 0.001f)
                            intersections.Add(intersection);
                }
            }

            return intersections;
        }

        public void ResetPolygon()
        {
            for (int i = Polygon.Count - 1; i >= 0; --i)
            {
                PolygonPoint point = Polygon[i];
                Polygon.Remove(point);
                Destroy(point.gameObject);
            }
        }
        
        public bool IsPositionValid(Vector3 position)
        {
            return _drawZone.bounds.Contains(position);
        }
        
        public bool IsInPolygon(Vector3 position)
        {
            if (Polygon.Count < 3)
                return false;
            
            int wn = RSLib.Maths.Geometry.ComputeWindingNumber(Polygon.Select(o => o.Position).ToArray(), position);
            return wn != 0;
        }

        private bool TryGetPointAtPosition(Vector3 position, float distance, out PolygonPoint point)
        {
            for (int i = 0; i < Polygon.Count; ++i)
            {
                if (Vector3.SqrMagnitude(position - Polygon[i].Position) <= distance * distance)
                {
                    point = Polygon[i];
                    return true;
                }
            }

            point = null;
            return false;
        }
        
        private void AddPoint(Vector3 position)
        {
            if (Polygon.Count == MaxPointsCount && MaxPointsCount > -1)
                return;

            if (!IsPositionValid(position))
                return;
            
            PolygonPoint polygonPoint = Instantiate(_polygonPointPrefab,
                                                    position,
                                                    _polygonPointPrefab.transform.rotation,
                                                    transform);
            
            Polygon.Add(polygonPoint);
            
            PointAdded?.Invoke(polygonPoint);
            _pointAddedEvent.Raise();
            
            RSLib.Audio.AudioManager.PlaySound(_addClipProvider);
        }

        private void DeletePoint(PolygonPoint point)
        {
            Polygon.Remove(point);
            Destroy(point.gameObject);
            
            PointDeleted?.Invoke(point);
            _pointDeletedEvent.Raise();
            
            RSLib.Audio.AudioManager.PlaySound(_removeClipProvider);
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Vector3 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition).WithZ(0f);

            TryGetPointAtPosition(position, _clickOnPointDistance, out PolygonPoint pointToDelete);
            PointToDelete = pointToDelete;
            if (pointToDelete != null && Input.GetMouseButtonDown(1))
                DeletePoint(pointToDelete);
            
            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (TryGetPointAtPosition(position, _clickOnPointDistance, out PolygonPoint pointToDrag))
                {
                    PointToDrag = pointToDrag;
                    PointToDragOrigin = PointToDrag.Position;
                    PointToDragLineEdgeOrigin = PointToDrag.LineEdgePosition;
                    PointToDrag.OnDragBegin();
                    
                    _pointDraggedEvent.Raise();
                    PointDragBegin?.Invoke(PointToDrag);
                    
                    RSLib.Audio.AudioManager.PlaySound(_dragBeginClipProvider);
                }
                else
                {
                    AddPoint(position);
                }
            }

            if (PointToDrag != null)
            {
                PointToDrag.transform.position = position;

                if (Input.GetMouseButtonUp(0))
                {
                    if (!IsPositionValid(position))
                        PointToDrag.transform.position = PointToDragOrigin;
                    
                    PointDragOver?.Invoke(PointToDrag);
                    _pointMovedEvent.Raise();
                    
                    PointToDrag.OnDragOver();
                    PointToDrag = null;
                    
                    RSLib.Audio.AudioManager.PlaySound(_dragOverClipProvider);
                }
            }
        }
    }
}