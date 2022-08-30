namespace WN
{
    using UnityEngine;

    public class LayoutRebuilder : MonoBehaviour
    {
        public void RebuildLayout()
        {
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform as RectTransform);
        }
    }
}