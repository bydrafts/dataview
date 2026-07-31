using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Drafts.Menu
{
    [RequireComponent(typeof(RectTransform))]
    public class ScrollOnSelect : MonoBehaviour, ISelectHandler
    {
        public ScrollRect scrollRect;
        public float margin = 20f;
        private RectTransform _rectTransform;

        private void Awake() => _rectTransform = GetComponent<RectTransform>();

        public void OnSelect(BaseEventData eventData)
            => ScrollTo(scrollRect, _rectTransform, margin);

        public static void ScrollTo(ScrollRect scrollRect, RectTransform target, float margin)
        {
            Canvas.ForceUpdateCanvases();

            var content = scrollRect.content;
            var viewport = scrollRect.viewport;

            // Get the world corners of the viewport and the target item
            var viewportCorners = new Vector3[4];
            viewport.GetWorldCorners(viewportCorners);
            var targetCorners = new Vector3[4];
            target.GetWorldCorners(targetCorners);

            var viewportTop = viewportCorners[1].y;
            var viewportBottom = viewportCorners[0].y;
            var targetTop = targetCorners[1].y;
            var targetBottom = targetCorners[0].y;

            var scrollAmount = 0f;

            // Check if the item is out of view at the top
            if (targetTop > viewportTop - margin)
                scrollAmount = targetTop - (viewportTop - margin);
            // Check if the item is out of view at the bottom
            else if (targetBottom < viewportBottom + margin) scrollAmount = targetBottom - (viewportBottom + margin);

            // If no scroll is needed, we're done
            if (Mathf.Approximately(scrollAmount, 0f)) return;

            // Apply the calculated scroll amount to the content's position
            var newAnchoredPos = content.anchoredPosition;
            newAnchoredPos.y -= scrollAmount;
            
            // Clamp the position to ensure the content doesn't scroll beyond its boundaries
            var maxScroll = content.rect.height - viewport.rect.height;

            if (maxScroll > 0)
                newAnchoredPos.y = Mathf.Clamp(newAnchoredPos.y, 0, maxScroll);
            else
                newAnchoredPos.y = 0;

            content.anchoredPosition = newAnchoredPos;
        }
    }
}