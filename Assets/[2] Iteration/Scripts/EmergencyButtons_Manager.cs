using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyButtons_Manager : MonoBehaviour
{
    [SerializeField] private GameObject gpsJam;
    [SerializeField] private GameObject brokenOnTakeOff;
    [SerializeField] private GameObject tabConnLoss;

    [SerializeField] private GameObject reset;

    private EmergencyManager emergencyManager;
    // Start is called before the first frame update
    void Start()
    {
        getEmergencyStatus();
    }

    private void getEmergencyStatus()
    {
        emergencyManager = GetComponent<EmergencyManager>();

        if (PlayerPrefs.GetInt("GPSjam done") == 0)
        {
            gpsJam.SetActive(false);
        }
        else
        {
            gpsJam.SetActive(true);
        }

        if (PlayerPrefs.GetInt("TabletConnLoss done") == 0)
        {
            tabConnLoss.SetActive(false);
        }
        else
        {
            tabConnLoss.SetActive(true);
        }

        if (PlayerPrefs.GetInt("BrokenOnTakeoffEmergency done") == 0)
        {
            brokenOnTakeOff.SetActive(false);
        }
        else
        {
            brokenOnTakeOff.SetActive(true);
        }

        if (PlayerPrefs.GetInt("BrokenOnTakeoffEmergency done") == 0 && PlayerPrefs.GetInt("TabletConnLoss done") == 0 && PlayerPrefs.GetInt("GPSjam done") == 0)
        {
            reset.SetActive(false);
        }
        else
        {
            reset.SetActive(true);
        }
    }

    public void ResetEmergenciesPlayerPrefs()
    {
        PlayerPrefs.SetInt("GPSjam done", 0);
        PlayerPrefs.SetInt("TabletConnLoss done", 0);
        PlayerPrefs.SetInt("BrokenOnTakeoffEmergency done", 0);
        PlayerPrefs.Save();

        getEmergencyStatus();
    }
}