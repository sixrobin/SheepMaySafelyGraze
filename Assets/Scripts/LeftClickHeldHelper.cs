namespace WN
{
    using RSLib.Extensions;
    using UnityEngine;

    public class LeftClickHeldHelper : InputHelper
    {
        private bool _pointDragged;

        public void OnPointDragged()
        {
            _pointDragged = true;
        }
        
        protected override void Update()
        {
            base.Update();
            
            bool enableHelp = !_pointDragged
                              && _currentLevelData.LevelController.PolygonController.PointToDelete != null
                              && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            
            _helperSpriteRenderer.enabled = enableHelp;
            _helperText.enabled = enableHelp;
        }
    }
}