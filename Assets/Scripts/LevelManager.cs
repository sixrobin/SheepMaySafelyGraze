namespace WN
{
    using System.Linq;
    using RSLib.Extensions;
    using UnityEngine;

    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private RSLib.Framework.Events.GameEvent _levelCompletedEvent = null;
        
        [SerializeField]
        private RSLib.Framework.Events.GameEvent _levelUncompletedEvent = null;

        [SerializeField]
        private RSLib.Framework.Events.GameEvent _levelStartedEvent = null;

        [SerializeField]
        private RSLib.Framework.Events.GameEvent _levelRestartedEvent = null;

        [SerializeField]
        private RSLib.Framework.Events.GameEvent _lastLevelCompletedEvent = null;

        [SerializeField]
        private CurrentLevelData _currentLevelData = null;

        [SerializeField]
        private bool _autoLocateLevels = true;
        
        [SerializeField]
        private LevelController[] _levels = null;

        private int _currentLevelIndex = 0;

        public void OnLevelStructureChanged()
        {
            bool levelCompleted = CheckCurrentLevelCompletion();
            
            if (levelCompleted)
                _levelCompletedEvent.Raise();
            else
                _levelUncompletedEvent.Raise();
        }

        public void OnAskForHelp()
        {
            FreePoint includedFreePoint = _levels[_currentLevelIndex].FreePoints
                                                                     .Where(o => o.MustBeInPolygon)
                                                                     .ToArray()
                                                                     .RandomElement();
            
            if (includedFreePoint != null)
                includedFreePoint.OnAskForHelp();
        }
        
        public bool CheckCurrentLevelCompletion()
        {
            LevelController level = _levels[_currentLevelIndex];

            foreach (FreePoint freePoint in level.FreePoints)
                if (!freePoint.CheckState())
                    return false;

            if (level.LevelData.RequiredIntersections > -1)
            {
                int intersections = level.PolygonController.GetIntersections().Count;
                if (intersections != level.LevelData.RequiredIntersections)
                    return false;
            }

            return true;
        }

        public void RestartLevel()
        {
            LevelController level = _levels[_currentLevelIndex];
            level.StartLevel();
            
            _levelRestartedEvent.Raise();
        }
        
        public void StartFirstLevel()
        {
            _levels[_currentLevelIndex].gameObject.SetActive(false);
            _currentLevelIndex = 0;
            StartLevelAtIndex(_currentLevelIndex);
        }
        
        public void StartNextLevel()
        {
            _levels[_currentLevelIndex].gameObject.SetActive(false);
            _currentLevelIndex++;

            if (_currentLevelIndex == _levels.Length)
            {
                _lastLevelCompletedEvent.Raise();
                _currentLevelIndex = 0;
            }
            else
            {
                StartLevelAtIndex(_currentLevelIndex);
            }
        }
        
        private void StartLevelAtIndex(int index)
        {
            LevelController level = _levels[index];
            level.PolygonController.MaxPointsCount = level.LevelData.MaxPointsCount;
            level.StartLevel();

            _currentLevelData.LevelController = level;
            
            _levelStartedEvent.Raise();
        }
        
        private void Start()
        {
            if (_autoLocateLevels)
            {
                System.Collections.Generic.List<LevelController> levels = FindObjectsOfType<LevelController>().ToList();
                levels.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
                _levels = levels.ToArray();
            }
            
            for (int i = 0; i < _levels.Length; ++i)
                _levels[i].gameObject.SetActive(false);
        }
    }
}