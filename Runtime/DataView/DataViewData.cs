using UnityEngine;
using Object = UnityEngine.Object;

namespace Drafts.DataView
{
    public class DataViewData : MonoBehaviour
    {
        [SerializeField] DataView view;
        [SerializeField] Object data;
        private object Data => data is DataView v ? v.GetData() : data;
        private void Start() => view.SetData(Data);
    }
}