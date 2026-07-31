using System;
using UnityEngine.Events;

namespace Drafts.Menu
{
    [Serializable]
    public class ItemCallbacks
    {
        public UnityEvent<MenuItem> onSubmit = new();
        public UnityEvent<MenuItem> onCancel = new();
        public UnityEvent<MenuItem> onSelect = new();
        public UnityEvent<MenuItem> onDeselect = new();

        public UnityEvent<MenuItem> onDrag = new();
        public UnityEvent<MenuItem> onDrop = new();
        public UnityEvent<MenuItem> onDragCancel = new();
    }
}