using UnityEngine;
using UnityEngine.UI;

namespace Drafts.DataView
{
    public class SpriteView : DataView<Sprite>
    {
        [SerializeField] private Image value;

        protected override void Subscribe() => value.overrideSprite = Data;
        protected override void Unsubscribe() => value.overrideSprite = null;
    }
}