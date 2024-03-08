using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MariusNameSpace
{
    public class ChaseCam : MonoBehaviour
    {
        [SerializeField] private GameObject drone;

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.LookAt(drone.transform.position);
        }
    }
}

