using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //This allows us to cast an array to a List, which is needed for us to use the GetComponentsInChildren

//Source: https://www.youtube.com/watch?v=fXQNc0q6jGM&list=PL5V9qxkY_RnK7R1pjh0cByUCB0Adiwown&index=12&ab_channel=Indie-Pixel

namespace MariusNameSpace
{
    [RequireComponent(typeof(Drone_Inputs))]
    public class Drone_Controller : Base_Rigidbody //We still get access to Monobehaviour because the Base_Rigidbody class inherits from it
    {
        #region Variables
        [Header("Control Properties")]
        [SerializeField] private float minMaxPitch = 30f;
        [SerializeField] private float minMaxRoll = 30f;
        [SerializeField] private float yawPower = 4f;

        private Drone_Inputs input;
        private List<IEngine> engines = new List<IEngine>(); //We make a list here to store the engines and avoid assigning them manually in the inspector

        #endregion

        #region Main Methods
        private void Start()
        {
            input = GetComponent<Drone_Inputs>();
            engines = GetComponentsInChildren<IEngine>().ToList<IEngine>(); //We are converting an array (GetComponentsInChildren only accepts arrays) to a list here

        }
        #endregion

        #region Custom Methods
        //Now we're overriding the virtual method HandlePhysics from Base_Rigidbody 
        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }

        protected virtual void HandleEngines()
        {
            //rb.AddForce(Vector3.up * (rb.mass * Physics.gravity.magnitude)); //Makes the drone hover, with the equation: mass * gravity, so we take the mass of our rigidbody and multiplies it with the magnitude of gravity. Gravity is -9.81, so the magnitude is 9.81.
            
            foreach(IEngine engine in engines)
            {
                engine.UpdateEngine(rb, input); //This accesses the UpdateEngine method from our interface IEngine, whose logic is ultimately defined in the Drone_Engine class
            }
            
        }

        protected virtual void HandleControls()
        {

        }
        #endregion
    }
}

