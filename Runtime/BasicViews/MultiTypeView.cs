using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Drafts.DataView
{
    public class MultiTypeView : DataView<object>
    {
        [SerializeField] private DataView fallback;
        [SerializeField] private DataView[] typeViews;

        private readonly Dictionary<Type, DataView> _viewMap = new();

        private void Awake()
        {
            var dataViewTType = typeof(DataView<>);
            foreach (var view in typeViews)
            {
                var foundType = GetGenericTypeInstance(dataViewTType, view.GetType());
                if (foundType == null) throw new Exception("Only DataView<T is supported on typeViews");
                _viewMap.Add(foundType.GenericTypeArguments[0], view);
                view.gameObject.SetActive(false);
            }
        }

        protected override void Subscribe()
        {
            Unsubscribe();

            var type = Data.GetType();
            var view = _viewMap.FirstOrDefault(p => p.Key.IsAssignableFrom(type)).Value;
            view ??= fallback;
            view.SetData(Data);
            view.gameObject.SetActive(true);
        }

        protected override void Unsubscribe()
        {
            fallback.gameObject.SetActive(false);
            foreach (var v in typeViews)
                v.gameObject.SetActive(false);
        }

        private static Type GetGenericTypeInstance(Type generic, Type subject)
        {
            while (subject != null)
            {
                var interfaces = subject.GetInterfaces();

                foreach (var t in interfaces.Prepend(subject))
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == generic)
                        return t;

                subject = subject.BaseType;
            }

            return null;
        }
    }
}