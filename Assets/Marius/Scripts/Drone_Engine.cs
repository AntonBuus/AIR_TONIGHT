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

        [Header("Propeller Properties")]
        [SerializeField] private Transform propeller;
        [SerializeField] private float propRotSpeed = 300f;
        #endregion


        #region Interface Methods
        public void InitEngine()
        {
            throw new System.NotImplementedException();
        }

        //Source: https://www.youtube.com/watch?v=2we5plbs_DU&list=PL5V9qxkY_RnK7R1pjh0cByUCB0Adiwown&index=14&ab_channel=Indie-Pixel
        public void UpdateEngine(Rigidbody rb, Drone_Inputs input) //In order to control the drone we need access to the rigidbody of each motor and the inputs
        {
            //The below five lines create the extra upward force that needs for the drone not to lose altitude when pitching and rolling. We might not need this. 
            //Source: https://www.youtube.com/watch?v=wgg9UblRCP8&list=PL5V9qxkY_RnK7R1pjh0cByUCB0Adiwown&index=16&ab_channel=Indie-Pixel
            //To enable or disable this, add "+ finalDiff" to the engineForce equation inside the mass * gravitry parenthesis -> it seems the finalDiff value is too big, because the drone rises in altittude when using pitch and row, instead of remaining in the same elevation.
            Vector3 upVec = transform.up;
            upVec.x = 0f;
            upVec.z = 0f;
            float diff = 1 - upVec.magnitude;
            float finalDiff = Physics.gravity.magnitude * diff;


            Vector3 engineForce = Vector3.zero;
            engineForce = transform.up * ((rb.mass * Physics.gravity.magnitude + finalDiff) + (input.Throttle * maxPower)) / 4f; //We know, that: mass * gravity = hover, and the Throttle is between -1 or 1, so we either add or take away from that equation by maxpower to lift or descend. We divide by four, because there are four motors, så we only want a quarter of that power for each motor.
            
            rb.AddForce(engineForce, ForceMode.Force);

            HandlePropeller();
        }

        void HandlePropeller()
        {
            if (!propeller) //If there are no propellers assigned, do not run this code - prevents errors
            {
                return;
            }

            propeller.Rotate(Vector3.up, propRotSpeed);
        }
        #endregion


    }
}

