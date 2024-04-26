// Source: https://www.youtube.com/watch?v=UeTlJyBz7h8

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace AskeNameSpace
{
    [System.Serializable]
    public class EmergencyManager : MonoBehaviour
    {
        private GameObject GPSJamScript;
        private GameObject TabletConnLossScript;
        private GameObject BrokenOnTakeoffScript;

        public bool takeOffPlayedBefore;
        [Header("Emergency events")]
        // Array containing emergency events defined in the class "EmergencyEvents".
        [SerializeField] public EmergencyEvents[] _emergencyEvents;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // Method where the emergency is caused.
        public void RandomEmergency()
        {
            bool placeholder = true;
            if (/*no checklist*/ placeholder == false && takeOffPlayedBefore == false)
            {
                BrokenOnTakeoffEmergency();
            }
            else if (_emergencyEvents[0]._emergencyChance == 0 && _emergencyEvents[1]._emergencyChance == 0)
            {
                BrokenOnTakeoffEmergency();
            }
            else
            {
                float _totalChance = 0f;
                foreach (EmergencyEvents _events in _emergencyEvents)
                {
                    _totalChance += _events._emergencyChance;
                }

                // Picks a random number between 0 and the value of _totalChance.
                float random = Random.Range(0f, _totalChance);
                float cumulativeEmergencyChance = 0f;

                // Goes through each event.
                foreach (EmergencyEvents _events in _emergencyEvents)
                {
                    cumulativeEmergencyChance += _events._emergencyChance;

                    if (random <= cumulativeEmergencyChance)
                    {
                        _events._emergencyEvent.Invoke();
                        return;
                    }
                }
            }
        }
    #region Emergency event methods
        public void GPSjam()
        {
            Debug.Log("GPS jam");
            _emergencyEvents[0]._emergencyChance = 0f;
            PlayerPrefs.SetInt("GPSjam done", 1);
            PlayerPrefs.Save();
            GPSJamScript.SetActive(true);
        }

        public void TabletConnLoss()
        {
            Debug.Log("Tablet connection loss");
            _emergencyEvents[1]._emergencyChance = 0f;
            PlayerPrefs.SetInt("TabletConnLoss done", 1);
            PlayerPrefs.Save();
            TabletConnLossScript.SetActive(true);
        }

        public void BrokenOnTakeoffEmergency()
        {
            Debug.Log("Broken on takeoff");
            PlayerPrefs.SetInt("BrokenOnTakeoffEmergency done", 1);
            PlayerPrefs.Save();
            ResetEmergenciesSession();
            BrokenOnTakeoffScript.SetActive(true);
            // Audio of broken drone, crash when hit waypoint.
        }
        #endregion

        public void ResetEmergenciesSession()
        {
            Debug.Log("Reset");
            takeOffPlayedBefore = false;
            foreach (EmergencyEvents _events in _emergencyEvents)
            {
                _events._emergencyChance = _events._originalEmergencyChance;
            }
        }
    }

    // Class defining the array "_emergencyEvents".
    [System.Serializable]
    public class EmergencyEvents
    {
        [SerializeField] private string _emergencyName;
        [Space]
        [Space]
        [SerializeField] [Range(0f, 1f)] public float _originalEmergencyChance;
        [SerializeField] [Range(0f, 1f)] public float _emergencyChance;
        [SerializeField] public UnityEvent _emergencyEvent;
    }
}