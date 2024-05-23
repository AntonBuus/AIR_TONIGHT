using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject artificialHorizon;

    [SerializeField] private GameObject pitchMeter;
    [SerializeField] private GameObject bankAngleArrowAnchor;

    //private GameObject bankAngleArrowAnchor;
    [SerializeField] private Transform drone;

    [SerializeField] private float amplitude = 500;

    private void Start()
    {
        //bankAngleArrowAnchor = GameObject.Find("BankAngleArrowAnchor");

    }


    void Update()
    {
        AH();
/*        float droneRotationZ = drone.localEulerAngles.z; // Get the rotation around the z-axis of the drone

        artificialHorizon.transform.localRotation = Quaternion.Euler(0f, 0f, -droneRotationZ); //Rotate the artificial horizon on the z-axis opposite to the drone

        pitchMeter.transform.localRotation = Quaternion.Euler(0f, 0f, -droneRotationZ); //Rotate the pitchMeter on the z-axis opposite to the drone

        //bankAngleArrowAnchor.transform.localRotation = Quaternion.Euler(0f, 0f, droneRotation);


        //-------------------- Move objects based on the rotation angle of the drone ------------------------------
        float rotationAngle = drone.localEulerAngles.x; //X-rotation of drone

        float desiredYPosition = Mathf.Sin(rotationAngle * Mathf.Deg2Rad) * amplitude; //amplitude controls the maximum distance the object moves up or down from its initial position.

        //Move artificial horizon along the Y-axis based on drones x rotation
        //artificialHorizon.transform.localPosition = droneRotationZ <

        desiredYPosition = droneRotationZ < 90 ? desiredYPosition : -desiredYPosition;

        artificialHorizon.transform.localPosition = new Vector3(artificialHorizon.transform.localPosition.x, desiredYPosition, artificialHorizon.transform.localPosition.z);

        //Move the pitch meter along the Y-axis based on drones x rotation
        pitchMeter.transform.position = new Vector3(pitchMeter.transform.position.x, desiredYPosition, pitchMeter.transform.position.z);*/
    }

    private void ArtificialHorizon()
    {
        artificialHorizon.transform.localRotation = Quaternion.Euler(0f, 0f, -drone.localEulerAngles.z); //-droneRotation because the horizon should rotate opposite of the drone

        //Calculate the horizons y-position based on the drone's x-position
        float horizonYposition = Mathf.Sin(drone.localEulerAngles.x * Mathf.Deg2Rad) * amplitude;
        horizonYposition = drone.localEulerAngles.z < 90 ? horizonYposition : -horizonYposition;

        //Set the artificial horizon's y-position
        artificialHorizon.transform.localPosition = new Vector3(artificialHorizon.transform.localPosition.x, horizonYposition, artificialHorizon.transform.localPosition.z);
    }

    [SerializeField] float zRotationSensitivity;
    private void AH()
    {
        //Stores the drone's rotation in a Quaternion
        float droneRotationZ = drone.localEulerAngles.z;

        //Rotates artificial horizon around the z-axis
        artificialHorizon.transform.Rotate(Vector3.forward, -droneRotationZ - artificialHorizon.transform.localEulerAngles.z, Space.Self);

        //Calculates the desiredYposition based on the x-rotation of the drone
        float horizonYposition = Mathf.Sin(drone.localEulerAngles.x * Mathf.Deg2Rad) * amplitude;
        
        //horizonYposition = droneRotationZ < 90 ? horizonYposition : -horizonYposition;

        //Sets the y-position of the artificial horizon 
        artificialHorizon.transform.localPosition = new Vector3(artificialHorizon.transform.localPosition.x, horizonYposition, artificialHorizon.transform.localPosition.z);


    }

}
