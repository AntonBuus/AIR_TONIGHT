using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyButtons_Manager : MonoBehaviour
{
    [SerializeField] private GameObject gpsJam;
    [SerializeField] private GameObject brokenOnTakeOff;
    [SerializeField] private GameObject tabConnLoss;

    private EmergencyManager emergencyManager;
    // Start is called before the first frame update
    void Start()
    {
        emergencyManager = GetComponent<EmergencyManager>();

        if(PlayerPrefs.GetInt("GPSjam done") == 0)
        {
            gpsJam.SetActive(false);
        }
        else
        {
            gpsJam.SetActive(true);
        }

        if (PlayerPrefs.GetInt("TabletConnLoss done") == 0)
        {
            brokenOnTakeOff.SetActive(false);
        }
        else
        {
            brokenOnTakeOff.SetActive(true);
        }

        if (PlayerPrefs.GetInt("BrokenOnTakeoffEmergency done") == 0)
        {
            tabConnLoss.SetActive(false);
        }
        else
        {
            tabConnLoss.SetActive(true);
        }
    }

}
