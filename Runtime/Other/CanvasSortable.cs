using UnityEngine;

namespace Drafts.Menu
{
    public interface ICanvasSortable
    {
        Transform transform { get; }
        int Size { get; }
    }

    public class CanvasSortable : MonoBehaviour, ICanvasSortable
    {
        [SerializeField] private int size = 1;
        [SerializeField] private bool deactivateOnAwake;
        [SerializeField] private bool deactivateOnStart;
        public int Size => size;

        private void Awake()
        {
            var rect = GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            if (deactivateOnAwake) gameObject.SetActive(false);
        }

        private void Start()
        {
            if (deactivateOnStart) gameObject.SetActive(false);
        }
    }
}