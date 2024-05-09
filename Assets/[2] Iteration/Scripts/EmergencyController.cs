using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmergencyController : MonoBehaviour
{
    public GameObject drone;
    public GameObject tablet;
    public TextMeshProUGUI _lostConnectionText;
    private EmergencyManager _emergencyManager;
    private FixedWing_Controller _fixedWing_Controller;
    private Menu_Manager _menu_Manager;
    private NewHUD _newHUD;
    private DroneVariables _droneVariables;

    [SerializeField] private Toggle autoPilotToggle;
    [SerializeField] private Button rtlButton;
    [SerializeField] private Button landButton;
    [SerializeField] private Button disarmButton;
    [SerializeField] private GameObject MiniMapGraphic_Drone;

    [SerializeField] private Transform gpsJamInitWaypoint;
    [SerializeField] private Transform tabletConnLossInitWaypoint;
    [SerializeField] private Transform brokenOnTakeoffInitWaypoint;
    [SerializeField] private float _threshold = 50;

    [SerializeField] private AudioClip brokenDroneSoundWoosh;

    bool fwcEnabled = false;
    bool autopilotToggled = false;
    bool throttleToggled = false;

    private int _activeEmergency;
    public int emergencyBegun = 0;

    void Start()
    {
        _lostConnectionText.gameObject.SetActive(false);

        _emergencyManager = FindObjectOfType<EmergencyManager>();
        _fixedWing_Controller = FindObjectOfType<FixedWing_Controller>();
        _droneVariables = FindObjectOfType<DroneVariables>();
        _menu_Manager = FindObjectOfType<Menu_Manager>();
        _newHUD = FindObjectOfType<NewHUD>();
    }

    private void Update()
    {
        switch (_emergencyManager.currentEmergency)
        {
            case 1:
                GpsJam();
                break;
            case 2:
                TabletConnLoss();
                break;
            case 3:
                BrokenOnTakeoff();
                break;
        }
    }

    // disable autopilot to imitate no gps signal.
    private void GpsJam()
    {
        if (Vector3.Distance(drone.transform.position,gpsJamInitWaypoint.position) < _threshold)
        {
            emergencyBegun = 1;
            if (fwcEnabled == false)
            {
                fwcEnabled = true;
                _fixedWing_Controller.autoPilot = false;

                autoPilotToggle.enabled = false;
                rtlButton.enabled = false;
                landButton.enabled = false;
                _droneVariables.enabled = false;
                //_newHUD.GpsText();
                //_newHUD.FailsafeText();
                _menu_Manager.disarmNotAllowed = true;
                MiniMapGraphic_Drone.SetActive(false);
            }
        }
        else if (emergencyBegun == 1)
        {

        }
        
        /*
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            _menu_Manager.MissionEndScreen();
        }*/
    }
    
    // disable autopilot and manualControl to imitate GPS and controller connection loss.
    private void TabletConnLoss()
    {
        //Should prevent ToggleThrottle button from working
        if (Vector3.Distance(drone.transform.position, tabletConnLossInitWaypoint.position) < _threshold)
        {
            emergencyBegun = 2;
            if (autopilotToggled == false)
            {
                autopilotToggled = true;
                // No manual or autopilot controls.
                //_fixedWing_Controller.controlFailure = true;
                _fixedWing_Controller.ActivateRTL();
                autoPilotToggle.enabled = false;
                tablet.GetComponentInChildren<Button>().enabled = false;
                tablet.GetComponentInChildren<Toggle>().enabled = false;
                _newHUD.enabled = false;
                _droneVariables.enabled = false;
                //_newHUD.FailsafeText();
                _lostConnectionText.gameObject.SetActive(true);
                //LostConnectionCount();
            }
        }/*
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            _menu_Manager.MissionEndScreen();
        }*/
    }

    // disable throttle to imitate engine or propeller issue.
    private void BrokenOnTakeoff()
    {
        //broken sound
        //Should prevent ToggleThrottle button from working
        if (throttleToggled == false)
        {
            emergencyBegun = 3;
            throttleToggled = true;
            //_fixedWing_Controller.ToggleThrottle();
            drone.GetComponent<AudioSource>().clip = brokenDroneSoundWoosh; // Tell the AudioSource to become that sound
            _menu_Manager.disarmNotAllowed = false;

        }/*
        if (Vector3.Distance(drone.transform.position, waypoint3.position) < _threshold)
        {
            _menu_Manager.MissionEndScreen();
        }*/
    }

    private void LostConnectionCount() 
    {
        //bool timer = new Timer(1000);
        _lostConnectionText.text = "Lost Connection ";
    }
}