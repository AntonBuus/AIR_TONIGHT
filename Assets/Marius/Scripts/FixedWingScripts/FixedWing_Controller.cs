using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof (AircraftPhysics))]
    [RequireComponent(typeof(FixedWing_Inputs))]

    public class FixedWing_Controller : MonoBehaviour
    {
        [SerializeField] List<AeroSurface> controlSurfaces = null;
        [SerializeField] List<WheelCollider> wheels = null;

        [SerializeField] float rollControlSensitivity = 0.2f;
        [SerializeField] float pitchControlSensitivity = 0.2f;
        [SerializeField] float yawControlSensitivity = 0.2f;

        [Range(0, 1)] public float Flap;

        [SerializeField] Text displayText = null;

        [Range(0, 100)] float thrustPercent;

        float brakesTorque;

        private AircraftPhysics aircraftPhysics;
        private Rigidbody rb;
        private FixedWing_Inputs FWInputs;

        private void Start()
        {
            aircraftPhysics = GetComponent<AircraftPhysics>();
            rb = GetComponent<Rigidbody>();
            FWInputs = GetComponent<FixedWing_Inputs>();
        }

        private void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                thrustPercent = FWInputs.Throttle;
                //thrustPercent = thrustPercent > 0 ? 0 : 1f;
            }*/

            if (Input.GetKeyDown(KeyCode.F))
            {
                Flap = Flap > 0 ? 0 : 0.3f;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                brakesTorque = brakesTorque > 0 ? 0 : 100f;
            }

            displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
            displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
            displayText.text += "T: " + (int)(thrustPercent * 100) + "%\n";
            displayText.text += brakesTorque > 0 ? "B: ON" : "B: OFF";
        }

        private void FixedUpdate()
        {
         thrustPercent = Mathf.Clamp(thrustPercent + FWInputs.Throttle, 0, 1); //This line sets thrustPercent according to the value of the Throttle input, limited to a value between 0 and 1.
        

        SetControlSurfecesAngles(FWInputs.Pitch, FWInputs.Roll, -FWInputs.Yaw, Flap);
            aircraftPhysics.SetThrustPercent(thrustPercent);
            foreach (var wheel in wheels)
            {
                wheel.brakeTorque = brakesTorque;
                // small torque to wake up wheel collider
                wheel.motorTorque = 0.01f;
            }
        }

        public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
        {
            foreach (var surface in controlSurfaces)
            {
                if (surface == null || !surface.IsControlSurface) continue;
                switch (surface.InputType)
                {
                    case ControlInputType.Pitch:
                        surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                        break;
                    case ControlInputType.Roll:
                        surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                        break;
                    case ControlInputType.Yaw:
                        surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                        break;
                    case ControlInputType.Flap:
                        surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                        break;
                }
            }
        }

        /*private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                SetControlSurfecesAngles(FWInputs.Pitch, FWInputs.Roll, FWInputs.Yaw, Flap);
        }*/
    }


