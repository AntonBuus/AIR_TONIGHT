using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MariusNameSpace
{
    [RequireComponent(typeof(PlayerInput))] //Checks if a PlayerInput component is attached to the gameobject this script is assigned to, and if not makes one

    public class Drone_Inputs : MonoBehaviour
    {
        #region Variables

        private Vector2 cyclic; //Corresponds to the cyclic action in our Drone action map. This is a Vector2 because the cyclic action is set to a Vector2.
        private float pedals; //Corresponds to the pedals action in our action map, is a float value because our pedals value goes from -1 to 1.
        private float throttle; //Correspond to the throttle action in our action map, is a float value because our pedals value goes from -1 to 1.

        //Below lines are encapsulating properties. Not entirely sure of that concept yet... Search "Encapsulating property" to find out
        public Vector2 Cyclic { get => cyclic; }
        public float Pedals { get => pedals; }
        public float Throttle { get => throttle; }

        #endregion

        #region Main Methods

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        //The input methods below will listen for messages sent by the player input - see PLayerInput component under "Behavior". 
        #region Input Methods 
        private void OnCyclic(InputValue value)
        {
            cyclic = value.Get<Vector2>();
        }

        private void OnPedals(InputValue value)
        {
            pedals = value.Get<float>();
        }

        private void OnThrottle(InputValue value)
        {
            throttle = value.Get<float>();
        }
        #endregion
    }
}

