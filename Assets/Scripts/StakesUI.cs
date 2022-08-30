namespace WN
{
    using UnityEngine;

    public class StakesUI : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.HorizontalLayoutGroup _stakesLayoutGroup = null;
        
        [SerializeField]
        private CurrentLevelData _currentLevelData = null;

        [SerializeField]
        private StakeUI _stakePrefab = null;

        private System.Collections.Generic.List<StakeUI> _stakes = new();
        
        public void RefreshMaxStakes()
        {
            for (int i = _stakesLayoutGroup.transform.childCount - 1; i >= 0; --i)
                if (_stakesLayoutGroup.transform.GetChild(i).TryGetComponent<StakeUI>(out _))
                    Destroy(_stakesLayoutGroup.transform.GetChild(i).gameObject);
            
            _stakes.Clear();
            
            int stakesMax = _currentLevelData.Data.MaxPointsCount;
            for (int i = 0; i < stakesMax; ++i)
            {
                StakeUI stake = Instantiate(_stakePrefab, _stakesLayoutGroup.transform);
                _stakes.Add(stake);
            }
        }
        
        public void RefreshStakes()
        {
            if (_currentLevelData.Data.MaxPointsCount == -1)
                return;
            
            int usedStakes = _currentLevelData.LevelController.PolygonController.Polygon.Count;
            int stakesLeft = _currentLevelData.Data.MaxPointsCount - usedStakes;
            
            for (int i = 0; i < _stakes.Count; ++i)
                _stakes[i].SetOn(i < stakesLeft);
        }
    }
}