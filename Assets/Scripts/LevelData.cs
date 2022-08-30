namespace WN
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Level", menuName = "WN/Level")]
    public class LevelData : ScriptableObject
    {
        [SerializeField, Min(-1)]
        private int _maxPointsCount = 5;

        [SerializeField, Min(-1)]
        private int _requiredIntersections = -1;

        public int MaxPointsCount => _maxPointsCount;
        public int RequiredIntersections => _requiredIntersections;
    }
}