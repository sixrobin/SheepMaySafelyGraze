namespace RSLib.Extensions
{
    using UnityEngine;

    public static class RectTransformExtensions
    {
        #region CLAMP
        
        /// <summary>
        /// Clamps the RectTransform position to another RectTransform position.
        /// </summary>
        /// <param name="rectTransform">RectTransform to clamp.</param>
        /// <param name="otherRectTransform">Reference RectTransform to clamp to.</param>
        public static void ClampTo(this RectTransform rectTransform, RectTransform otherRectTransform)
        {
            Vector3 localPos = rectTransform.localPosition;

            Rect rect = rectTransform.rect;
            Rect otherRect = otherRectTransform.rect;

            Vector3 minPos = otherRect.min - rect.min;
            Vector3 maxPos = otherRect.max - rect.max;

            localPos.x = Mathf.Clamp(rectTransform.localPosition.x, minPos.x, maxPos.x);
            localPos.y = Mathf.Clamp(rectTransform.localPosition.y, minPos.y, maxPos.y);

            rectTransform.localPosition = localPos;
        }

        /// <summary>
        /// Clamps the RectTransform position to its parent RectTransform.
        /// </summary>
        public static void ClampToParent(this RectTransform rectTransform)
        {
            UnityEngine.Assertions.Assert.IsNotNull(
                rectTransform.parent,
                $"Cannot clamp {rectTransform.name} RectTransform to its parent since it has no parent.");

            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            UnityEngine.Assertions.Assert.IsNotNull(
                parentRectTransform,
                $"{rectTransform.name} RectTransform parent's {parentRectTransform.name} has no RectTransform component to do a clamp.");

            rectTransform.ClampTo(parentRectTransform);
        }
        
        #endregion // CLAMP
    }
}