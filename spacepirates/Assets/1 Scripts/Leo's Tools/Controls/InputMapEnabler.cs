using UnityEngine;

namespace _1_Scripts.Leo_s_Tools.Controls
{
    public class InputMapEnabler : MonoBehaviour
    {
        private static InputMapEnabler inputMapEnabler;
        
        public static InputMapEnabler Instance
        {
            get
            {
                if (!inputMapEnabler)
                {
                    // Find the instance of InputMapEnabler in the scene
                    inputMapEnabler = FindAnyObjectByType<InputMapEnabler>();
                    
                    // If the instance doesn't exist, log an error
                    if (!inputMapEnabler)
                    {
                        Debug.LogError("There is no InputMapEnabler in the scene!");
                    }
                    else
                    {
                        DontDestroyOnLoad(inputMapEnabler); // Preserve between scene loads
                    }
                }
                return inputMapEnabler;
            }
        }

        public InputSystem_Actions inputActions;

        private void Awake()
        {
            // Initialize the input actions when the InputMapEnabler is awakened
            if (inputActions == null)
            {
                inputActions = new InputSystem_Actions();
            }

            // Ensure input actions are enabled
            inputActions.Enable();
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
    }
}