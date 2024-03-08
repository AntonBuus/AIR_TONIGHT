using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Source: https://www.youtube.com/watch?v=MHVDGMqwKF0&list=PL5V9qxkY_RnK7R1pjh0cByUCB0Adiwown&index=11&ab_channel=Indie-Pixel

namespace MariusNameSpace
{
    [RequireComponent(typeof(Rigidbody))] //Makes sure that a rigidbody is assigned to our gameobject

    public class Base_Rigidbody : MonoBehaviour
    {
        #region Variables
        [Header("Rigidbody Properties")]
        [SerializeField] private float weightInKg = 1f;

        protected Rigidbody rb;

        protected float startDrag; //We have this variable because we want dynamic drag, meaning that the drag should be updated as something moves faster - the faster it goes, the more drag we're creating
        protected float startAngularDrag; //Protected means that any script that inherits from this class has access to it

        #endregion

        #region Main Methods
        void Awake()
        {
            rb = GetComponent<Rigidbody>();

            if (rb)
            {
                rb.mass = weightInKg;
                startDrag = rb.drag;
                startAngularDrag = rb.angularDrag;
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!rb)
            {
                return;
            }

            HandlePhysics();
        }

        #endregion

        #region Costum Methods
        //Virtual because we want to overwrite it in other scripts
        //Protected because we want to protect this method but still want all classes that inherit from the Base_Rigidbody class are able to see it
        protected virtual void HandlePhysics() 
        {

        }

        #endregion
    }
}

