using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHUD : MonoBehaviour
{
    [SerializeField] private GameObject artificialHorizon;

    [SerializeField] private GameObject pitchMeter;
    [SerializeField] private GameObject pitchMeterPanel;
    [SerializeField] private RawImage compassImage;

    //private GameObject bankAngleArrowAnchor;
    [SerializeField] private Transform drone;

    [SerializeField] private float amplitude = 500;

    private float pitchMeterStartYpos;

    private void Start()
    {
        pitchMeterStartYpos = pitchMeter.transform.localPosition.y;
    }


    void Update()
    {
        ArtificialHorizon();
        CompassBar();
    }

    private void ArtificialHorizon()
    {
        //Stores the drone's rotation in a Quaternion
        float droneRotationZ = drone.localEulerAngles.z;

        //Rotates artificial horizon around the z-axis
        artificialHorizon.transform.Rotate(Vector3.forward, -droneRotationZ - artificialHorizon.transform.localEulerAngles.z, Space.Self);

        pitchMeterPanel.transform.Rotate(Vector3.forward, -droneRotationZ - pitchMeterPanel.transform.localEulerAngles.z, Space.Self);

        //Calculates the desiredYposition based on the x-rotation of the drone
        float horizonYposition = Mathf.Sin(drone.localEulerAngles.x * Mathf.Deg2Rad) * amplitude;

        //horizonYposition = droneRotationZ < 90 ? horizonYposition : -horizonYposition;

        //Sets the y-position of the artificial horizon 
        artificialHorizon.transform.localPosition = new Vector3(artificialHorizon.transform.localPosition.x, horizonYposition, artificialHorizon.transform.localPosition.z);


        pitchMeter.transform.localPosition = new Vector3(pitchMeter.transform.localPosition.x, pitchMeterStartYpos + horizonYposition, pitchMeter.transform.localPosition.z);
    }

    private void CompassBar()
    {
        compassImage.uvRect = new Rect(drone.localEulerAngles.y / 360f, 0, 1, 1);
    }
}
