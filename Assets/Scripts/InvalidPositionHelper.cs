namespace WN
{
    using RSLib.Extensions;
    using UnityEngine;

    public class InvalidPositionHelper : InputHelper
    {
        protected override void Update()
        {
            base.Update();
            
            Vector3 position = _mainCamera.ScreenToWorldPoint(Input.mousePosition).WithZ(0f);
            _helperSpriteRenderer.enabled = !_currentLevelData.LevelController.PolygonController.IsPositionValid(position)
                                            && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }
    }
}