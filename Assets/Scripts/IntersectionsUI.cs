namespace WN
{
    using UnityEngine;

    public class IntersectionsUI : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas = null;

        [SerializeField]
        private UnityEngine.UI.LayoutElement _layoutElement = null;
        
        [SerializeField]
        private CurrentLevelData _currentLevelData = null;

        [SerializeField]
        private TMPro.TextMeshProUGUI _currentText = null;
        
        [SerializeField]
        private TMPro.TextMeshProUGUI _requiredText = null;

        [SerializeField]
        private RSLib.Data.Color _invalidColor = null;

        private Color _initColor;
        
        public void RefreshRequired()
        {
            int requiredIntersections = _currentLevelData.Data.RequiredIntersections;
            _requiredText.text = requiredIntersections.ToString();

            _canvas.enabled = requiredIntersections > -1;
            _layoutElement.ignoreLayout = requiredIntersections == -1;
        }
        
        public void RefreshCurrent()
        {
            int requiredIntersections = _currentLevelData.Data.RequiredIntersections;
            int intersections = _currentLevelData.LevelController.PolygonController.GetIntersections().Count;
            
            _currentText.text = intersections.ToString();
            _currentText.color = intersections <= requiredIntersections ? _initColor : _invalidColor;
        }

        private void Awake()
        {
            _initColor = _currentText.color;
        }
    }
}