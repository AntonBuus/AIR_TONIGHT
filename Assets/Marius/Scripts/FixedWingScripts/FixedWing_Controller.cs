using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AircraftPhysics))]
[RequireComponent(typeof(FixedWing_Inputs))]
[RequireComponent(typeof(CrashLandingDetection))]

public class FixedWing_Controller : MonoBehaviour
{
    #region Variables
    [Tooltip("When active, the drone flies autonomously")] 
    public bool autoPilot = true;

    [SerializeField] private float rotationResetSpeed = 25;

    public bool controlFailure = false;

    [SerializeField] List<AeroSurface> controlSurfaces = null;
    [SerializeField] List<SphereCollider> wheels = null;

    [Tooltip("If Wheel Brake is 0, the brakes are Off. If value is 1, the brakes are On")]
    private int wheelBrake = 1;

    [SerializeField] float rollControlSensitivity = 0.2f;
    [SerializeField] float pitchControlSensitivity = 0.2f;
    [SerializeField] float yawControlSensitivity = 0.2f;


    [SerializeField] private float distanceToWaypointThresh = 50;

    [Range(0, 1)] public float Flap;

    //[SerializeField] Text displayText = null;

    [SerializeField] private GameObject propellerR;
    [SerializeField] private GameObject propellerL;
    [SerializeField] private float propellerSpeed = 500f;

    private AudioSource propellerSound;
    [SerializeField] private float maxPitch = 5f;

    public float thrustPercent;

    private Transform activeWaypoint;

    private AircraftPhysics aircraftPhysics;
    private Rigidbody rb;
    private FixedWing_Inputs FWInputs;
    private Waypoints waypoints;

    public bool grabbedController = false;

    #endregion

    #region Main Methods
    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>(); //Reference to a script component
        FWInputs = GetComponent<FixedWing_Inputs>(); //Reference to a script component
        rb = GetComponent<Rigidbody>(); //Reference to a Ridgidbody
        propellerSound = GetComponent<AudioSource>(); //Refernce to an audiosource
        waypoints = FindObjectOfType<Waypoints>(); //Reference to the object with the waypoints script component

        activeWaypoint = waypoints.GetNextWaypoint(activeWaypoint); //Sets the active waypoint to the first waypoint in the hierachy
        propellerSound.pitch = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        propellerR.transform.Rotate(0f, thrustPercent * propellerSpeed * Time.fixedDeltaTime, 0f); //Rotates the right propeller counter clockwise
        propellerL.transform.Rotate(0f, -1 * thrustPercent * propellerSpeed * Time.fixedDeltaTime, 0f); //Rotates the left propeller clockwise



/*        //Resets the pitch of the drone when it is not actively adjusted and autopilot is off
        if (transform.localRotation.x != 0f && FWInputs.Pitch == 0 && !autoPilot)
        {
            // Calculate the rotation needed to align with zero on the z-axis
            Quaternion targetRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

            // Rotate towards the target rotation
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * pitchResetSpeed);
        }*/

                //Resets the roll of the drone when it is not actively adjusted and autopilot is off
/*        if (transform.localRotation.z != 0f && FWInputs.Roll == 0 && !autoPilot)
        {
            // Calculate the rotation needed to align with zero on the z-axis
            Quaternion targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0f);

            // Rotate towards the target rotation
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * rollResetSpeed);
        }*/

    }

    public void BoolGrabbedController(bool grabbedController)
    {
        grabbedController = grabbedController;
    }

    public void FixedUpdate()
    {
        WheelBrakes();
        if (controlFailure == false)
        { 
            if(autoPilot)
            {
                AutoPilot();
                SetControlSurfacesAngles(0, 0, 0, 0);
            }
            else if (autoPilot == false && grabbedController == true)
            {

                propellerSound.pitch = Mathf.Clamp(propellerSound.pitch + FWInputs.Throttle * 2f, 0f, 5f);

                thrustPercent = Mathf.Clamp(thrustPercent + FWInputs.Throttle, 0, 1); //This line sets thrustPercent according to the value of the Throttle input, limited to a value between 0 and 1.
                SetControlSurfacesAngles(FWInputs.Pitch, FWInputs.Roll, -FWInputs.Yaw, Flap);
            }
        }

        //Resets the roll of the drone when it is not actively adjusted and autopilot is off
        if (transform.localRotation.z != 0f && FWInputs.Roll == 0 && FWInputs.Pitch == 0 && !autoPilot)
        {
            // Calculate the rotation needed to align with zero on the z-axis
            Quaternion targetRotation = Quaternion.Euler(0f, transform.localRotation.eulerAngles.y, 0f);

            // Rotate towards the target rotation
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Time.deltaTime * rotationResetSpeed);
            //Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * pitchResetSpeed);
        }

        aircraftPhysics.SetThrustPercent(thrustPercent);
    }
    #endregion

    #region Costum Methods
    //Assigns pitch, roll, and yaw to the drone - this essentially what gives control over the drone
    public void SetControlSurfacesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue; //If the surface assigned in controlSurfaces is by mistake not a control surface, skip it and continue with the switch statement
            
            switch (surface.InputType) //InputType is a reference to the enumerator named "ControlInputType", which contains Pitch, Roll, Yaw, and Flap.
            {
                case ControlInputType.Pitch: //If controls input is pitch
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll: //If controls input is roll
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw: //If controls input is yaw
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap: ////If controls input is flap - OBS! We're not currently using this
                    surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                    break;
            }
        }
    }

    private void AutoPilot()
    {
        Vector3 tempWaypoint = (activeWaypoint.position - transform.position).normalized; //Calculates and stores the direction of the next waypoint 

        Quaternion lookRotation = Quaternion.LookRotation(tempWaypoint); //Calculates and stores the rotation towards the next waypoint

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime); //Rotates the this object from its current rotation to the desired rotation at time.deltatime

        if (Vector3.Distance(transform.position, activeWaypoint.position) < distanceToWaypointThresh) //If the distance between this object and the current waypoint is less than the distance to waypoint threshhold
        {
            activeWaypoint = waypoints.GetNextWaypoint(activeWaypoint); //Sets active waypoint to the next waypoint

            //The below if-statement ensures, that the y position of the first waypoint is the same as the other waypoints once it has been reached the first time
            if(Vector3.Distance(transform.position, waypoints.transform.GetChild(0).transform.position) < distanceToWaypointThresh && waypoints.transform.GetChild(0).transform.position.y != waypoints.transform.GetChild(1).transform.position.y)
            {
                Vector3 firstWaypointPosition = waypoints.transform.GetChild(0).position; //Gets vector3 position of the first waypoint and stores in a variable
                firstWaypointPosition.y = waypoints.transform.GetChild(1).position.y; //Set the y position to the same of the second waypoint
                waypoints.transform.GetChild(0).position = firstWaypointPosition; //Actually assigns the y position of the first waypoint to the firs waypoint position
            }
        }
    }

    private void WheelBrakes()
    {
        if(thrustPercent == 0) //(thrustPercent instead of wheelBrake)
        {
            //Sets the spherecolliders on the wheels to "break" on the ground
            foreach (var wheel in wheels)
            {
                wheel.material.staticFriction = 1;
                wheel.material.frictionCombine = PhysicMaterialCombine.Average;
            }
        }
        else //if (wheelBrake == 1)
        {
            //Sets the spherecolliders on the wheels to be able to "drive" on the ground
            foreach (var wheel in wheels)
            {
                //The below parameters correspond to the "slippery" physcis material
                wheel.material.dynamicFriction = 0;
                wheel.material.staticFriction = 0;
                wheel.material.bounciness = 0;
                wheel.material.frictionCombine = PhysicMaterialCombine.Minimum;
                wheel.material.bounceCombine = PhysicMaterialCombine.Average;
            }
        }
    }

    public void ToggleThrottle() //To be assigned to a button in the inspector
    {
        propellerSound.pitch = propellerSound.pitch > 0 ? 0 : 5f;
        thrustPercent = thrustPercent > 0 ? 0 : 1f;
    }

    public void ToggleAutoPilot()
    {
        autoPilot = autoPilot = false ? true : false;
    }
    #endregion

}


