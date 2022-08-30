namespace WN
{
    using UnityEngine;

    public class PolygonDrawer : MonoBehaviour
    {
        [SerializeField]
        private PolygonController _polygonController = null;

        [SerializeField]
        private LineRenderer _lineRenderer = null;
        
        [SerializeField]
        private LineRenderer _dragLineRenderer = null;

        [SerializeField]
        private GameObject _intersectionPrefab = null;
        
        private System.Collections.Generic.List<GameObject> _intersections = new();

        public void Clear()
        {
            _lineRenderer.positionCount = 0;
            
            for (int i = _intersections.Count - 1; i >= 0; --i)
                Destroy(_intersections[i]);
            
            _intersections.Clear();
        }
        
        private void RefreshPolygon()
        {
            if (_polygonController.Polygon.Count == 0)
            {
                _lineRenderer.positionCount = 0;
            }
            else
            {
                _lineRenderer.positionCount = _polygonController.Polygon.Count == 2 ? 2 : _polygonController.Polygon.Count + 1;
                for (int i = 0; i < _lineRenderer.positionCount; ++i)
                    this._lineRenderer.SetPosition(i, _polygonController.Polygon[i % _polygonController.Polygon.Count].LineEdgePosition);
            }

            System.Collections.Generic.List<Vector3> intersections = _polygonController.GetIntersections();
            for (int i = _intersections.Count - 1; i >= 0; --i)
                Destroy(_intersections[i]);
            
            _intersections.Clear();
            for (int i = 0; i < intersections.Count; ++i)
            {
                GameObject intersection = Instantiate(_intersectionPrefab, transform);
                intersection.transform.position = intersections[i];
                _intersections.Add(intersection);
            }
        }

        private void RefreshDrag()
        {
            if (_polygonController.IsDraggingPoint)
            {
                _dragLineRenderer.positionCount = 2;
                _dragLineRenderer.SetPosition(0, _polygonController.PointToDragLineEdgeOrigin);
                _dragLineRenderer.SetPosition(1, _polygonController.PointToDrag.LineEdgePosition);
            }
            else
            {
                _dragLineRenderer.positionCount = 0;
            }
        }
        
        private void OnPointAdded(PolygonPoint point)
        {
            RefreshPolygon();
        }

        private void OnPointDeleted(PolygonPoint point)
        {
            RefreshPolygon();
        }
        
        private void OnPointDragOver(PolygonPoint point)
        {
            RefreshPolygon();
        }
        
        private void Start()
        {
            _polygonController.PointAdded += OnPointAdded;
            _polygonController.PointDeleted += OnPointDeleted;
            _polygonController.PointDragOver += OnPointDragOver;
            _lineRenderer.positionCount = 0;
        }

        private void Update()
        {
            if (_polygonController.IsDraggingPoint)
                RefreshPolygon();
            
            RefreshDrag();
        }

        private void OnDestroy()
        {
            _polygonController.PointAdded -= OnPointAdded;
            _polygonController.PointDeleted -= OnPointDeleted;
            _polygonController.PointDragOver -= OnPointDragOver;
        }

        private void OnDisable()
        {
            Clear();
        }
    }
}