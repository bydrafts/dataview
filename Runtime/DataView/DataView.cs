using UnityEngine;
using UnityEngine.Events;

namespace Drafts.DataView
{
    public class DataView : MonoBehaviour
    {
        public UnityEvent<object> onDataChanged;
        private object _data;

        public virtual object GetData() => _data;

        public virtual void SetData(object data)
        {
            _data = data;
            onDataChanged?.Invoke(data);
        }

        public void CopyData(DataView other) => SetData(other.GetData());

        [ContextMenu("Reset Data")]
        public void ResetData() => SetData(null);

        [ContextMenu("Log Data")]
        public void LogData() => Debug.Log(GetData());
    }
}