using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source: https://www.youtube.com/watch?v=QzOTFX3uOP4&list=PL5V9qxkY_RnK7R1pjh0cByUCB0Adiwown&index=13&ab_channel=Indie-Pixel

namespace MariusNameSpace
{
    [RequireComponent(typeof(BoxCollider))]

    //NOTICE that we do not have a Start or Update method because we run this in the Drone_Controller!
    public class Drone_Engine : MonoBehaviour, IEngine
    {
        #region Variables
        [Header("Engine Properties")]
        [SerializeField] private float maxPower = 4f;

        #endregion


        #region Interface Methods
        public void InitEngine()
        {
            throw new System.NotImplementedException();
        }

        //Source: https://www.youtube.com/watch?v=2we5plbs_DU&list=PL5V9qxkY_RnK7R1pjh0cByUCB0Adiwown&index=14&ab_channel=Indie-Pixel
        public void UpdateEngine(Rigidbody rb, Drone_Inputs input) //In order to control the drone we need access to the rigidbody of each motor and the inputs
        {
            Vector3 engineForce = Vector3.zero;
            engineForce = transform.up * ((rb.mass * Physics.gravity.magnitude) + (input.Throttle * maxPower)) / 4f; //We know, that: mass * gravity = hover, and the Throttle is between -1 or 1, so we either add or take away from that equation by maxpower to lift or descend. We divide by four, because there are four motors, så we only want a quarter of that power for each motor.
            
            rb.AddForce(engineForce, ForceMode.Force);
        }
        #endregion
    }
}

