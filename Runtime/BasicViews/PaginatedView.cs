using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

namespace Drafts.DataView
{
    public class PagedCollectionView : CollectionView
    {
        [SerializeField] private RectTransform viewport;

        private int _startIndex;
        private IList _list;
        
        protected override void Subscribe()
        {
            if (Data is not IList l) throw new Exception("PagedCollectionView only supports IList");
            _list = l;
            
            if (!IsFixed)
                CreateViews();
            Refresh();

            if (Data is INotifyCollectionChanged notify)
                notify.CollectionChanged += CollectionChanged;
        }

        protected override void Unsubscribe()
        {
            if (Data is INotifyCollectionChanged notify)
                notify.CollectionChanged -= CollectionChanged;
        }

        private void CreateViews()
        {
            if (views.Count != 0) return;
            itemTemplate.gameObject.SetActive(false);

            var layout = itemTemplate.GetComponent<LayoutElement>();
            var itemHeight = layout && layout.preferredHeight > 0
                ? layout.preferredHeight
                : ((RectTransform)itemTemplate.transform).rect.height;
            var visible = Mathf.CeilToInt(viewport.rect.height / itemHeight) + 1;

            for (var i = 0; i < visible; i++)
            {
                var view = Instantiate(itemTemplate, itemTemplate.transform.parent);
                view.gameObject.SetActive(true);
                views.Add(view);
            }
        }

        private void Refresh()
        {
            for (var i = 0; i < views.Count; i++)
            {
                var index = _startIndex + i;
                var view = views[i];
                view.Index = index;

                if (index >= 0 && index < _list.Count)
                {
                    view.gameObject.SetActive(true);
                    view.SetData(_list[index]);
                }
                else
                {
                    view.gameObject.SetActive(false);
                    view.SetData(null);
                }
            }
        }

        public void ScrollPage(int pageCount)
        {
            Scroll(pageCount * views.Count);
        }
        
        public void Scroll(int itemCount)
        {
            if (Data == null || _list.Count == 0 || itemCount == 0)
                return;

            if (IsFixed)
            {
                var max = Math.Max(0, _list.Count - views.Count);
                _startIndex = Math.Clamp(_startIndex + itemCount, 0, max);
                Refresh();
                return;
            }

            while (itemCount > 0)
            {
                ScrollDown();
                itemCount--;
            }

            while (itemCount < 0)
            {
                ScrollUp();
                itemCount++;
            }
        }

        private bool ScrollDown()
        {
            if (_startIndex + views.Count >= _list.Count) return false;
            _startIndex++;

            var first = views[0];
            var index = _startIndex + views.Count - 1;
            views.RemoveAt(0);
            views.Add(first);
            first.transform.SetAsLastSibling();
            first.Index = index;
            first.SetData(_list[index]);
            return true;
        }

        private bool ScrollUp()
        {
            if (_startIndex == 0) return false;
            _startIndex--;

            var last = views[^1];
            views.RemoveAt(views.Count - 1);
            views.Insert(0, last);
            last.transform.SetSiblingIndex(1);
            last.Index = _startIndex;
            last.SetData(_list[_startIndex]);
            return true;
        }

        protected override void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        public void Scroll(DataView item)
        {
            if (views.Count == 0) return;

            if (item == views[0])
            {
                if (ScrollUp())
                    views[0].GetComponent<Selectable>()?.Select();
            }
            else if (item == views[^1])
            {
                if (ScrollDown())
                    views[^1].GetComponent<Selectable>()?.Select();
            }
        }
    }
}