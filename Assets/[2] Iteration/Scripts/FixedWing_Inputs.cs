using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


    [RequireComponent(typeof(PlayerInput))] //Checks if a PlayerInput component is attached to the gameobject this script is assigned to, and if not makes one

public class FixedWing_Inputs : MonoBehaviour
{
    #region Variables

      
    private float _pitch;
    private float _roll;
    private float _yaw; //Corresponds to the yaw action in our action map, is a float value because our pedals value goes from -1 to 1.
    private float _throttle; //Correspond to the throttle action in our action map, is a float value because our pedals value goes from -1 to 1.

    //Below lines are encapsulating properties that essentially just assigns the values of the input to the variables.
    public float Pitch { get => _pitch; }
    public float Roll { get => _roll; }
    public float Yaw { get => _yaw; }
    public float Throttle { get => _throttle; }

    #endregion

    //The input methods below will listen for messages sent by the player input - see PLayerInput component under "Behavior". 
    #region Input Methods
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


