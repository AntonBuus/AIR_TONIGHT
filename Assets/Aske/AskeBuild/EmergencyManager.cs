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

        // Placeholder method

        private void Start()
        {
            Emergency();
        }

        private void Emergency()
        {
            CauseEmergency();
        }

        // Method where the emergency is caused
        private void CauseEmergency()
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
        #region Event Logic
        #endregion

        #region GPS jam
        public void GPSjam()
        {
            Debug.Log("GPS jam");
        }
        #endregion
        #region Tablet connection loss
        public void TabletConnLoss()
        {
            Debug.Log("Tablet connection loss");
        }
        #endregion
        #region Low battery
        public void LowBattery()
        {
            Debug.Log("Low battery");
        }
        #endregion
    }

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