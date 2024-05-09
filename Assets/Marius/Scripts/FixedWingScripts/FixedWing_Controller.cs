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

    public bool grabbedController = false;
    public bool rtlActive = false;
    public bool manualControl = false;


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

    public GameObject _toggleButton;

    private Transform activeWaypoint;
    private Transform activeRTLWaypoint;

    private float incrementalWaypointHeight;
    private bool isLanding = false;

    private AircraftPhysics aircraftPhysics;
    private Rigidbody rb;
    private FixedWing_Inputs FWInputs;
    private Waypoints waypoints;
    private RTLWaypoints rtlWaypoints;
    private CrashLandingDetection crashLandingDetection;
    private EmergencyController emergencyController;


    #endregion

    #region Main Methods
    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>(); //Reference to a script component
        FWInputs = GetComponent<FixedWing_Inputs>(); //Reference to a script component
        rb = GetComponent<Rigidbody>(); //Reference to a Ridgidbody
        propellerSound = GetComponent<AudioSource>(); //Refernce to an audiosource
        waypoints = FindObjectOfType<Waypoints>(); //Reference to the object with the waypoints script component
        rtlWaypoints = FindObjectOfType<RTLWaypoints>();
        emergencyController = FindAnyObjectByType<EmergencyController>();
        crashLandingDetection = GetComponent<CrashLandingDetection>();

        activeWaypoint = waypoints.GetNextWaypoint(activeWaypoint); //Sets the active waypoint to the first waypoint in the hierachy
        propellerSound.pitch = 0;

        incrementalWaypointHeight = waypoints.transform.GetChild(1).position.y; //Saves the initial height of the second waypoint to be referenced in TakeOff();

        //If autopilot is On by default, set manual control Off, else On
        if (autoPilot)
        {
            manualControl = false;
        }
        else
        {
            manualControl= true;
        }
    }

    private void Update()
    {
        propellerR.transform.Rotate(0f, thrustPercent * propellerSpeed * Time.fixedDeltaTime, 0f); //Rotates the right propeller counter clockwise
        propellerL.transform.Rotate(0f, -1 * thrustPercent * propellerSpeed * Time.fixedDeltaTime, 0f); //Rotates the left propeller clockwise
        
        propellerSound.pitch = Mathf.Clamp(thrustPercent * 5, 0f, 5f);
    }

    public void BoolGrabbedController(bool isGrabbed)
    {
        grabbedController = isGrabbed;
    }

    public void FixedUpdate()
    {
        WheelBrakes();
        if (controlFailure == false)
        {
            if (autoPilot)
            {
                AutoPilot();
            }

                ManualControl();
            

            if(rtlActive == true)
            {
                RTL();
            }
        }

        //Resets the roll of the drone when it is not actively adjusted and autopilot is off
        if (transform.localRotation.z != 0f && FWInputs.Roll == 0 && FWInputs.Pitch == 0 && !autoPilot)
        {
            // Calculate the rotation needed to align with 0 on the x- and z-axis 
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

    private void ManualControl()
    {
        if (!manualControl && grabbedController == true && autoPilot == false && rtlActive == false)
        {
            manualControl = true;
            autoPilot = false;
            rtlActive = false;
        }
        else if(grabbedController == false)
        {
            manualControl = false;
        }
        
        thrustPercent = Mathf.Clamp(thrustPercent + FWInputs.Throttle, 0, ThrustPercent(1f)); //This line sets thrustPercent according to the value of the Throttle input, limited to a value between 0 and 1.
        SetControlSurfacesAngles(FWInputs.Pitch, FWInputs.Roll, -FWInputs.Yaw, Flap);
    }

    private void AutoPilot()
    {
        SetControlSurfacesAngles(0, 0, 0, 0);

        if (crashLandingDetection.droneLanded == true)
        {
            thrustPercent = ThrustPercent(0.5f);
        }       

        Vector3 tempWaypoint = (activeWaypoint.position - transform.position).normalized; //Calculates and stores the direction of the next waypoint 

        Quaternion lookRotation = Quaternion.LookRotation(tempWaypoint); //Calculates and stores the rotation towards the next waypoint

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime); //Rotates the this object from its current rotation to the desired rotation at time.deltatime

        if (Vector3.Distance(transform.position, activeWaypoint.position) < distanceToWaypointThresh / 4) //If the distance between this object and the current waypoint is less than the distance to waypoint threshhold
        {
            activeWaypoint = waypoints.GetNextWaypoint(activeWaypoint); //Sets active waypoint to the next waypoint

            if(isLanding == false)
            {
                //Ensures that all waypoints are set at the same height
                for (int i = 0; i < waypoints.transform.childCount; i++)
                {
                    Vector3 waypointsYpos = new Vector3(waypoints.transform.GetChild(i).position.x, incrementalWaypointHeight, waypoints.transform.GetChild(i).position.z);

                    waypoints.transform.GetChild(i).position = waypointsYpos;
                }
            }
        }
    }

    public void RTL()
    {
        Vector3 tempWaypoint = (activeRTLWaypoint.position - transform.position).normalized; //Calculates and stores the direction of the next waypoint 

        Quaternion lookRotation = Quaternion.LookRotation(tempWaypoint); //Calculates and stores the rotation towards the next waypoint

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f); //Rotates the this object from its current rotation to the desired rotation at time.deltatime * 0.5f which is the rotationspeed

        if (Vector3.Distance(transform.position, activeRTLWaypoint.position) < distanceToWaypointThresh) //If the distance between this object and the current waypoint is less than the distance to waypoint threshhold
        {
            activeWaypoint = rtlWaypoints.GetNextWaypoint(activeRTLWaypoint); //Sets active waypoint to the next waypoint
        }
    }

    public void ActivateRTL()
    {
        autoPilot = false;
        manualControl = false;
        rtlActive = true;
        thrustPercent = ThrustPercent(0.5f); ;
        
        activeRTLWaypoint = rtlWaypoints.GetNextWaypoint(activeRTLWaypoint); //Sets the active waypoint to the first waypoint in the hierachy
    }

    public void Takeoff()
    {
        isLanding = false;
        thrustPercent = ThrustPercent(0.5f); ; //Corresponds to 50% throttle

        activeWaypoint.position = new Vector3(activeWaypoint.position.x, 0, activeWaypoint.position.z);
    }

    public void Landing()
    {
        isLanding = true;
        thrustPercent = 0;

        for (int i = 0; i < waypoints.transform.childCount; i++)
        {
            Vector3 waypointsYpos = new Vector3(waypoints.transform.GetChild(i).position.x, 0, waypoints.transform.GetChild(i).position.z);

            waypoints.transform.GetChild(i).position = waypointsYpos;
            
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
        thrustPercent = thrustPercent > 0 ? 0 : 1f;
    }

    public void ToggleAutoPilot()
    {
        //SHould only set throttle to 0.5f if the drone is in the air, and not on the ground
        if(thrustPercent < 0)
        {
            thrustPercent = ThrustPercent(0.5f);
        }

        rtlActive = false;
        manualControl = false;

        if (autoPilot == true)
        {
            autoPilot = false;
        }
        else if(autoPilot == false) //OG emergencybegun ikke er tabconnloss (int = 2)
        {
            autoPilot = true;
        }
    }

    public float ThrustPercent(float percent)
    {
        if(emergencyController.emergencyBegun == 3) //3 corresponds to the brokenontakeoff emergency
        {
            percent = 0.25f; //The max throttle value when the BrokenOnTakeoff emergency occurs
            return percent;
        }
        else
        {
            return percent;
        }
    }
    #endregion

}


