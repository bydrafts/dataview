using TMPro;
using UnityEngine;

namespace Drafts.DataView
{
    public class DropdownCarousel : MonoBehaviour
    {
        public TMP_Dropdown dropdown;

        public void Next()
        {
            var i = dropdown.value + 1;
            if (i >= dropdown.options.Count) i = 0;
            dropdown.value = i;
        }

        public void Prev()
        {
            var i = dropdown.value - 1;
            if (i < 0) i = dropdown.options.Count - 1;
            dropdown.value = i;
        }
    }
}