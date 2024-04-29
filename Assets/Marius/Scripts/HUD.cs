using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject artificialHorizon;
    [SerializeField] private GameObject altitudeMeter;

    private GameObject bankAngleArrowAnchor;
    [SerializeField] private Transform drone;

    [SerializeField] private float amplitude = 1;

    private float artHozStartYpos;
    private float altMeterStartYpos;

    private void Start()
    {
        bankAngleArrowAnchor = GameObject.Find("BankAngleArrowAnchor");
        artHozStartYpos = artificialHorizon.transform.position.y;
        altMeterStartYpos = altitudeMeter.transform.position.y;
    }

    void Update()
    {
        float droneRotation = drone.eulerAngles.z; // Get the rotation around the z-axis of the drone
        artificialHorizon.transform.rotation = Quaternion.Euler(0f, 0f, -droneRotation); //Rotate the artificial horizon on the z-axis opposite to the drone

        bankAngleArrowAnchor.transform.rotation = Quaternion.Euler(0f, 0f, droneRotation);


        // Move the other object based on the rotation angle of this object
        float rotationAngle = drone.transform.eulerAngles.x;
        float desiredYPosition = Mathf.Sin(rotationAngle * Mathf.Deg2Rad) * amplitude;

        artificialHorizon.transform.position = new Vector3(artificialHorizon.transform.position.x, artHozStartYpos + desiredYPosition, artificialHorizon.transform.position.z);

        altitudeMeter.transform.position = new Vector3(altitudeMeter.transform.position.x, altMeterStartYpos + desiredYPosition, altitudeMeter.transform.position.z);
    }
}
