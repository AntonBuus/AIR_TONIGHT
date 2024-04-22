using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


    [RequireComponent(typeof(PlayerInput))] //Checks if a PlayerInput component is attached to the gameobject this script is assigned to, and if not makes one

    public class FixedWing_Inputs : MonoBehaviour
    {
        #region Variables

        private Vector2 _cyclic; //Corresponds to the cyclic action in our Drone action map. This is a Vector2 because the cyclic action is set to a Vector2.
    private float _pitch;
    private float _roll;
        private float _yaw; //Corresponds to the pedals action in our action map, is a float value because our pedals value goes from -1 to 1.
        
    private float _throttle; //Correspond to the throttle action in our action map, is a float value because our pedals value goes from -1 to 1.

        //Below lines are encapsulating properties. Not entirely sure of that concept yet... Search "Encapsulating property" to find out
        public Vector2  Cyclic { get => _cyclic; }

    public float Pitch { get => _pitch; }

    public float Roll { get => _roll; }

        public float Yaw { get => _yaw; }
        public float Throttle { get => _throttle; }

        #endregion

        //The input methods below will listen for messages sent by the player input - see PLayerInput component under "Behavior". 
        #region Input Methods 
        private void OnCyclic(InputValue value)
        {
            _cyclic = value.Get<Vector2>();
        }

    private void OnPitch(InputValue value)
    {
        _pitch = value.Get<float>();
    }

    private void OnRoll(InputValue value)
    {
        _roll = value.Get<float>();
    }

        private void OnYaw(InputValue value)
        {
            _yaw = value.Get<float>();
        }

        public void OnThrottle(InputValue value)
        {
            _throttle = value.Get<float>() * 0.01f; //multiplied by 0.01f to calibrate the throttle so it is changed more softly
        }
        #endregion
    }


