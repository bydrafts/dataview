#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Drafts.Menu
{
    public interface ICustomInputHandler
    {
        void HandleInput(GameObject selected, InputAction.CallbackContext ctx);
    }

    public class EventSystemCustomInput : MonoBehaviour
    {
        public PlayerInput playerInput;

        private void Awake()
        {
            playerInput.onActionTriggered += HandleAction;
        }

        private static void HandleAction(InputAction.CallbackContext ctx)
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            selected?.GetComponent<ICustomInputHandler>()?.HandleInput(selected, ctx);
        }
    }
}
#endif