namespace WN
{
    using UnityEngine;

    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private LevelData _levelData = null;

        [SerializeField]
        private PolygonController _polygonController = null;
        
        public LevelData LevelData => _levelData;
        public PolygonController PolygonController => _polygonController;
        
        public FreePoint[] FreePoints { get; private set; }

        public void StartLevel()
        {
            gameObject.SetActive(true);
            
            PolygonController.ResetPolygon();
            
            FreePoints = GetComponentsInChildren<FreePoint>();
            foreach (FreePoint freePoint in FreePoints)
            {
                freePoint.OnLevelStarted();
                freePoint.UpdateIsInPolygon();
            }
        }

        private void OnDisable()
        {
            FreePoints = null;
        }
    }
}