using System.Collections;
using UnityEngine;

namespace Drafts.DataView
{
    public class CollectionViewSelectFirst : MonoBehaviour
    {
        [SerializeField] CollectionView collection;
        [SerializeField] DataView target;

        private void Awake() => collection.onDataChanged.AddListener(SelectFirst);
        private void OnDestroy() => collection.onDataChanged.RemoveListener(SelectFirst);

        private void SelectFirst(object data)
        {
            if (data is not IEnumerable ie) return;
            foreach (var item in ie)
            {
                target.SetData(item);
                return;
            }

            target.SetData(null);
        }
    }
}