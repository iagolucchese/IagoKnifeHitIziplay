using UnityEngine;

namespace IagoUnityScriptsRepo
{
    public class InputManager : ASingleton<InputManager>
    {
        [Header("Player")] 
        public float horizontalInput;
        public float verticalInput;
        public bool horizontal;
        public bool vertical;

        public bool fire1Down;
        public bool fire1;
        
        private static bool IsAxisCloseEnoughToZero(string axisName) =>
            (Mathf.Abs(Input.GetAxis(axisName))) <= Mathf.Epsilon;

        private void GrabPlayerInputs()
        {
            horizontalInput = GetAxisInput("Mouse X", out horizontal);
            verticalInput = GetAxisInput("Mouse Y", out vertical);

            GetButtonHoldAndDownInputs("Fire1",out fire1Down, out fire1);
        }

        private float GetAxisInput(string axisName, out bool inputBool)
        {
            float output;
            if (!IsAxisCloseEnoughToZero(axisName))
            {
                output = Input.GetAxis(axisName);
                inputBool = true;
            }
            else
            {
                output = 0f;
                inputBool = false;
            }
            return output;
        }

        private bool GetButtonDownInput(string buttonName)
        {
            return Input.GetButtonDown(buttonName);;
        }

        private bool GetButtonInput(string buttonName)
        {
            return Input.GetButton(buttonName);
        }

        private void GetButtonHoldAndDownInputs(string buttonName, out bool buttonDown, out bool button)
        {
            buttonDown = GetButtonDownInput(buttonName);
            button = GetButtonInput(buttonName);
        }

        private void Update()
        {
            GrabPlayerInputs();
        }
    }
}