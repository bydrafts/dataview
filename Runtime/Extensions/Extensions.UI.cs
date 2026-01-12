using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Drafts.DataView
{
    public static partial class DataViewExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetEnabled(this Behaviour view, bool data)
        {
            if (view) view.enabled = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInverseEnabled(this Behaviour view, bool data)
        {
            if (view) view.enabled = !data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetText(this TMP_Text view, string data)
        {
            if (view) view.text = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetText(this TMP_Text view, object data)
        {
            if (view) view.text = data?.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetText(this TMP_InputField view, string data)
        {
            if (view) view.text = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetText(this TMP_InputField view, object data)
        {
            if (view) view.text = data?.ToString();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetColor(this TMP_Text view, Color data)
        {
            if (view) view.color = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetSprite(this Image view, Sprite data)
        {
            if (view) view.overrideSprite = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetColor(this Image view, Color data)
        {
            if (view) view.color = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetActive(this GameObject view, bool data)
        {
            if (view) view.SetActive(data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetActive(this Component view, bool data)
        {
            if (view) view.gameObject.SetActive(data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInverseActive(this GameObject view, bool data)
        {
            if (view) view.SetActive(!data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInteractable(this Selectable view, bool data)
        {
            if (view) view.interactable = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInverseInteractable(this Selectable view, bool data)
        {
            if (view) view.interactable = !data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInteractable(this CanvasGroup view, bool data)
        {
            if (view) view.interactable = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInverseInteractable(this CanvasGroup view, bool data)
        {
            if (view) view.interactable = !data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetIsOn(this Toggle view, bool data)
        {
            if (view) view.SetIsOnWithoutNotify(data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TrySetInverseIsOn(this Toggle view, bool data)
        {
            if (view) view.SetIsOnWithoutNotify(!data);
        }
    }
}