namespace WN
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Current Level Data", menuName = "WN/Current Level Data")]
    public class CurrentLevelData : ScriptableObject
    {
        [HideInInspector]
        public LevelController LevelController;

        public LevelData Data => LevelController.LevelData;
    }
}