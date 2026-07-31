#if ENABLE_INPUT_SYSTEM
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Drafts.Menu
{
    public class CustomInputHandler : MonoBehaviour, ICustomInputHandler
    {
        [Serializable]
        public class Input
        {
            public string name;
            public InputActionReference action;
            public InputActionPhase phase = InputActionPhase.Started;
            public UnityEvent<GameObject, InputAction.CallbackContext> trigger;
        }

        public Input[] inputs;

        public void HandleInput(GameObject selected, InputAction.CallbackContext ctx)
        {
            foreach (var input in inputs)
            {
                if (input.action.action != ctx.action) continue;
                if (ctx.phase != input.phase) continue;
                input.trigger.Invoke(selected, ctx);
            }
        }
    }
}
#endif