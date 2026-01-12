using UnityEngine;
using UnityEngine.Events;

namespace Drafts.DataView
{
    public class UnityEventView<T> : DataView<T>
    {
        [SerializeField] private UnityEvent<T> value;
        protected override void Subscribe() => value.Invoke(Data);
        protected override void Unsubscribe() => value.Invoke(default);
    }
}