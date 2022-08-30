namespace WN
{
    using RSLib.Extensions;
    using UnityEngine;

    public class LeftClickHelper : InputHelper
    {
        private bool _pointSet;

        public void OnPointSet()
        {
            _pointSet = true;
        }
        
        protected override void Update()
        {
            base.Update();
            
            Vector3 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition).WithZ(0f);
            bool enableHelp = !_pointSet
                              && _currentLevelData.LevelController.PolygonController.IsPositionValid(position)
                              && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            
            _helperSpriteRenderer.enabled = enableHelp;
            _helperText.enabled = enableHelp;
        }
    }
}