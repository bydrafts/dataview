using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Drafts.Menu
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SelectableMenu : MonoBehaviour
    {
        private static readonly List<SelectableMenu> Stack = new();

        public MenuItem firstItem;
        public CanvasGroup canvasGroup;
        public bool closeOnAwake;
        public UnityEvent onEnable;
        public UnityEvent onDisable;

        private MenuItem _lastItem;

        private void Awake()
        {
            canvasGroup.interactable = enabled;
            if (closeOnAwake) gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (Stack.Count > 0)
            {
                Stack[^1].canvasGroup.interactable = false;
                Stack[^1].onDisable.Invoke();
                Stack[^1]._lastItem = EventSystem.current?.currentSelectedGameObject?.GetComponent<MenuItem>();
            }

            EventSystem.current?.SetSelectedGameObject(null);
            canvasGroup.interactable = true;
            onEnable.Invoke();

            Stack.Add(this);
            Invoke(nameof(SelectFirst), 0f);
        }

        private void OnDisable()
        {
            EventSystem.current?.SetSelectedGameObject(null);
            _lastItem = firstItem;
            canvasGroup.interactable = false;
            Stack.Remove(this);
            onDisable.Invoke();

            if (Stack.Count <= 0) return;
            Stack[^1].canvasGroup.interactable = true;
            Stack[^1].Invoke(nameof(SelectFirst), 0f);
            Stack[^1].onEnable.Invoke();
        }

        private void SelectFirst()
        {
            var item = _lastItem ?? firstItem;

            if (item && item.isActiveAndEnabled)
            {
                item.Selectable.Select();
                return;
            }

            foreach (var s in GetComponentsInChildren<MenuItem>())
            {
                if (!s.isActiveAndEnabled) continue;
                s.Selectable.Select();
                return;
            }
        }

        private void Reset()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}