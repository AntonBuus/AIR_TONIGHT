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
        [Header("Emergency events")]
        // Array containing emergency events defined in the class "EmergencyEvents"
        [SerializeField] public EmergencyEvents[] _emergencyEvents;

        /*
        private void Start()
        {
            RandomEmergency()
        }
        */

        // Method where the emergency is caused
        private void RandomEmergency()
        {
            float _totalChance = 0f;
            foreach (EmergencyEvents _events in _emergencyEvents)
            {
                _totalChance += _events._emergencyChance;
            }

            // Picks a random number between 0 and the value of _totalChance
            float random = Random.Range(0f, _totalChance);
            float cumulativeEmergencyChance = 0f;

            // Goes through each event
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

        #region Emergency event methods
        public void GPSjam()
        {
            Debug.Log("GPS jam");
            _emergencyEvents[0]._emergencyChance = 0f;
            BrokenOnTakeoffEmergency();
        }

        public void TabletConnLoss()
        {
            Debug.Log("Tablet connection loss");
            _emergencyEvents[1]._emergencyChance = 0f;
            BrokenOnTakeoffEmergency();
        }

        public void BrokenOnTakeoffEmergency()
        { 
            int placeholder = 0;
            if (/*no checklist*/ placeholder == 0)
            {
                Debug.Log("Broken on takeoff due to no checklist");
                _emergencyEvents[2]._emergencyChance = 0f;
            }
            else if (_emergencyEvents[1]._emergencyChance == 0f && _emergencyEvents[0]._emergencyChance == 0f)
            {
                Debug.Log("Broken on takeoff");
                _emergencyEvents[2]._emergencyChance = 0f;
            }
        }
    }
    #endregion

    // Class defining the array "_emergencyEvents"
    [System.Serializable]
    public class EmergencyEvents
    {
        [SerializeField] private string _emergencyName;
        [Space]
        [Space]
        [SerializeField] [Range(0f, 1f)] public float _emergencyChance = 0.5f;
        [SerializeField] public UnityEvent _emergencyEvent;
    }
}