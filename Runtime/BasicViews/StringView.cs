using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Drafts.DataView
{
    public class StringView : DataView<string>
    {
        [SerializeField] private TMP_Text value;
        [SerializeField] private string defaultText = "---";
        private string _format;

        protected override void Subscribe()
        {
            _format ??= value.text.Contains('{') ? value.text : "{0}";
            value.text = string.Format(_format, Data);
        }

        protected override void Unsubscribe()
        {
            _format ??= value.text.Contains('{') ? value.text : "{0}";
            value.text = defaultText;
        }


        public void SetData<T>(T data) => Data = data.ToString();
    }

    public static class StringViewExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetData<T>(this StringView v, T data)
        {
            if (v) v.SetData(data);
        }
    }
}