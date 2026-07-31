using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if USE_LOCALIZATION
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
#endif

namespace Drafts.Menu
{
    [RequireComponent(typeof(Selectable))]
    public class MenuItem : MonoBehaviour, ISubmitHandler, ICancelHandler,
        ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private bool isEnabled = true;
        public DragContext drag;
        public TMP_Text text;
        public Image icon;
        public Color enabledColor = Color.white;
        public Color disabledColor = Color.red;
        public ItemSounds sounds;
        public AudioSource clickOverride;
        public ItemCallbacks callbacks = new();

        public Selectable Selectable { get; private set; }
#if USE_LOCALIZATION
        public LocalizedString BaseStr { get; private set; }
#endif
        public bool Enabled
        {
            get => isEnabled;
            set {
                isEnabled = value;
                if (text) text.color = value ? enabledColor : disabledColor;
            }
        }

        protected virtual void Awake()
        {
#if USE_LOCALIZATION
            BaseStr = text?.GetComponent<LocalizeStringEvent>()?.StringReference;
#endif
            Selectable = GetComponent<Selectable>();
            Enabled = isEnabled;
        }

        protected virtual void Reset()
        {
            if (!text) text = GetComponentInChildren<TMP_Text>();
            Selectable = GetComponent<Selectable>();

            var images = GetComponentsInChildren<Image>();
            if (!icon) icon = images.FirstOrDefault(i => i.gameObject != gameObject);

            Enabled = isEnabled;
        }

        private void Trigger(BaseEventData eventData)
        {
            if (eventData.used) return;
            eventData.Use();

            if (drag && drag.IsDragging)
            {
                if (sounds) (isEnabled ? sounds.drop : sounds.fail).Play();
                if (isEnabled) drag.DropOn(this);
                return;
            }

            if (sounds) (isEnabled ? clickOverride ? clickOverride : sounds.click : sounds.fail).Play();
            if (isEnabled) callbacks.onSubmit.Invoke(this);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) => Selectable.Select();
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) => Trigger(eventData);
        void ISubmitHandler.OnSubmit(BaseEventData eventData) => Trigger(eventData);
        void IDeselectHandler.OnDeselect(BaseEventData eventData) => callbacks.onDeselect.Invoke(this);

        void ICancelHandler.OnCancel(BaseEventData eventData)
        {
            if (eventData.used) return;
            eventData.Use();
            if (sounds) sounds.cancel.Play();

            if (drag && drag.IsDragging)
            {
                drag.Cancel();
                callbacks.onDragCancel.Invoke(this);
            }
            else callbacks.onCancel.Invoke(this);
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            if (eventData.used) return;
            eventData.Use();

            if (drag) drag.Select(this);

            if (sounds) sounds.navigate.Play();
            callbacks.onSelect.Invoke(this);
        }

        public void Drag() => drag.Drag(this);
        public void Drop() => drag.DropOn(this);
        public T GetData<T>() => (T)GetComponent<DataView.DataView>()?.GetData();
    }
}