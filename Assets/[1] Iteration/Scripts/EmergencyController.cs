using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyController : MonoBehaviour
{
    private GameObject drone;
    private EmergencyManager emergencyManager;
    [SerializeField] private gpsJam gpsJamScript;
    [SerializeField] private TabletConnLoss tabletConnLossScript;
    [SerializeField] private BrokenOnTakeoff brokenOnTakeoffScript;
    // Start is called before the first frame update
    void Start()
    {
        emergencyManager = FindObjectOfType<EmergencyManager>();

        //emergencyManager.enabled = false;

        int activeEmergency = emergencyManager.currentEmergency;

        switch (activeEmergency)
        {
            case 0:
                gpsJamScript.enabled = false;
                tabletConnLossScript.enabled = false;
                brokenOnTakeoffScript.enabled = false;
                break;
            case 1:
                gpsJamScript.enabled = true;
                break;
            case 2:
                tabletConnLossScript.enabled = true;
                break;
            case 3:
                brokenOnTakeoffScript.enabled = true;
                break;
        }
    }

    private void Update()
    {


    }
}
