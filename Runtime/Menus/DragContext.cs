using UnityEngine;
using UnityEngine.Events;

namespace Drafts.Menu
{
    public class DragContext : MonoBehaviour
    {
        public DragTemplate dragTemplate;
        public Vector3 dragOffset;

        public UnityEvent<MenuItem> onDrag;
        public UnityEvent<MenuItem, MenuItem> onDrop;
        public UnityEvent<MenuItem> onCancel;
        public UnityEvent onBack;

        public bool IsDragging => _draggingItem;
        private MenuItem _draggingItem;

        private void Awake()
        {
            dragTemplate.gameObject.SetActive(false);
        }

        public void Cancel()
        {
            if (!_draggingItem)
            {
                onBack.Invoke();
                return;
            }

            onCancel.Invoke(_draggingItem);
            _draggingItem.callbacks.onDragCancel.Invoke(_draggingItem);
            SetDragActive(false, _draggingItem);
        }

        public void Select(MenuItem item)
        {
            if (!_draggingItem) return;
            dragTemplate.transform.position = item.transform.position + dragOffset;
        }

        public void DragOrDrop(MenuItem item)
        {
            if (_draggingItem)
                DropOn(item);
            else Drag(item);
        }

        public void Drag(MenuItem item)
        {
            if (_draggingItem) return;
            SetDragActive(true, item);
            onDrag.Invoke(item);
            item.callbacks.onDrag.Invoke(item);
            
            if (item.sounds) 
                item.sounds.drag.Play();
        }

        public void DropOn(MenuItem item)
        {
            if (!_draggingItem) return;
            onDrop.Invoke(_draggingItem, item);
            item.callbacks.onDrop.Invoke(_draggingItem);
            
            if (_draggingItem.sounds)
                _draggingItem.sounds.drag.Play();
            
            SetDragActive(false, _draggingItem);
        }

        private void SetDragActive(bool drag, MenuItem item)
        {
            if (dragTemplate.text)
            {
                item.text.enabled = !drag;
                dragTemplate.text.text = item.text.text;
            }

            if (dragTemplate.icon)
            {
                item.icon.enabled = !drag;
                var spr = item.icon.overrideSprite ?? item.icon.sprite;
                dragTemplate.icon.overrideSprite = spr;
                dragTemplate.icon.color = item.icon.color;
            }

            _draggingItem = drag ? item : null;
            dragTemplate.gameObject.SetActive(drag);
            dragTemplate.transform.position = item.transform.position + dragOffset;
        }
    }
}