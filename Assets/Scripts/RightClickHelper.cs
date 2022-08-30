namespace WN
{
    using RSLib.Extensions;
    using UnityEngine;

    public class RightClickHelper : InputHelper
    {
        private bool _pointDeleted;

        public void OnPointDeleted()
        {
            _pointDeleted = true;
        }
        
        protected override void Update()
        {
            base.Update();
            
            bool enableHelp = !_pointDeleted
                              && _currentLevelData.LevelController.PolygonController.PointToDelete != null
                              && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            
            _helperSpriteRenderer.enabled = enableHelp;
            _helperText.enabled = enableHelp;
        }
    }
}