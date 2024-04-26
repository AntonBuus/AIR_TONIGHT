using AskeNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyController : MonoBehaviour
{
    private GameObject drone;
    private EmergencyManager emergencyManager;
    private GameObject gpsJamScript;
    private GameObject tabletConnLossScript;
    private GameObject brokenOnTakeoffScript;
    // Start is called before the first frame update
    void Start()
    {
        gpsJamScript = GameObject.Find("GPSJam_Script");
        tabletConnLossScript = GameObject.Find("TabletConnLoss_Script");
        brokenOnTakeoffScript = GameObject.Find("BrokenOnTakeoff_Script");
        emergencyManager = GetComponent<EmergencyManager>();
        int activeEmergency = emergencyManager.currentEmergency;
        switch (activeEmergency)
        {
            case 0:
                gpsJamScript.SetActive(false);
                tabletConnLossScript.SetActive(false);
                brokenOnTakeoffScript.SetActive(false);
                break;
            case 1:
                gpsJamScript.SetActive(true);
                break;
            case 2:
                tabletConnLossScript.SetActive(true);
                break;
            case 3:
                brokenOnTakeoffScript.SetActive(true);
                break;
        }
    }
}
