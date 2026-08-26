using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace Drafts.DataView
{
    public class CollectionView : DataView<IEnumerable>
    {
        [SerializeField] protected DataView itemTemplate;
        [SerializeField] protected List<DataView> views = new();
        public IReadOnlyList<DataView> Views => views;

        private bool? _isFixed;
        protected bool IsFixed => _isFixed ??= views.Count > 0;

        private void Awake()
        {
            if (itemTemplate)
                itemTemplate.gameObject.SetActive(false);
        }

        protected override void Subscribe()
        {
            if (Data is INotifyCollectionChanged notifyCollection)
                notifyCollection.CollectionChanged += CollectionChanged;

            if (IsFixed)
                SetFixedItems();
            else
            {
                var index = 0;
                foreach (var item in Data)
                    AddItem(index++, item);
            }
        }

        private void SetFixedItems()
        {
            var items = Data.GetEnumerator();
            for (var i = 0; i < views.Count; i++)
            {
                var view = views[i];
                view.SetData(items.MoveNext() ? items.Current : null);
                view.GetComponent<IntView>()?.SetData(i);
            }

            if (items is IDisposable d) d.Dispose();
        }

        protected override void Unsubscribe()
        {
            if (Data is INotifyCollectionChanged notifyCollection)
                notifyCollection.CollectionChanged -= CollectionChanged;

            if (IsDestroying) return;
            
            if (IsFixed)
            {
                foreach (var view in views)
                    view.SetData(null);
            }
            else
            {
                foreach (var view in views)
                    Destroy(view.gameObject);
                views.Clear();
            }
        }

        private void SetItem(int index, object data)
        {
            //TODO shift items
            views[index].SetData(data);
        }

        private void AddItem(int index, object item)
        {
            var view = Instantiate(itemTemplate, itemTemplate.transform.parent);
            view.transform.SetSiblingIndex(index + 1); // 0 is the template
            view.gameObject.SetActive(true);

            view.SetData(item);
            views.Insert(index, view);
        }

        protected virtual void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this)
            {
                Debug.LogError("should not happen");
                if (Data is INotifyCollectionChanged notifyCollection)
                    notifyCollection.CollectionChanged -= CollectionChanged;
                return;
            }
            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var index = e.NewStartingIndex < 0
                        ? itemTemplate.transform.childCount
                        : e.NewStartingIndex;

                    for (var i = 0; i < e.NewItems.Count; i++)
                        if (IsFixed) SetItem(index + i, e.NewItems[i]);
                        else AddItem(index + i, e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Move:
                    Debug.LogError("Move not implemented");
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (IsFixed)
                        SetFixedItems();
                    else
                        for (var i = 0; i < e.OldItems.Count; i++)
                        {
                            var rIndex = e.OldStartingIndex + i;
                            Destroy(views[rIndex].gameObject);
                            views.RemoveAt(rIndex);
                        }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    for (var i = 0; i < e.OldItems.Count; i++)
                        views[e.OldStartingIndex + i].SetData(e.NewItems[i]);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    if (IsFixed)
                        foreach (var view in views)
                            view.SetData(null);
                    else
                    {
                        foreach (var view in views) Destroy(view.gameObject);
                        views.Clear();
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}