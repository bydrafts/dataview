using UnityEngine;
using UnityEngine.Events;

namespace Drafts.DataView
{
    public class DataViewExtras : MonoBehaviour
    {
        [SerializeField] private DataView target;

        public UnityEvent onNull;
        public UnityEvent onNotNull;

        private void Awake()
        {
            Raise(target.GetData());
            target.onDataChanged.AddListener(Raise);
        }

        private void OnDestroy()
        {
            target.onDataChanged.RemoveListener(Raise);
        }

        private void Raise(object data)
        {
            if (data is null)
                onNull.Invoke();
            else onNotNull.Invoke();
        }
    }
}