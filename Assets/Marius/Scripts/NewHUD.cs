using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewHUD : MonoBehaviour
{
    [SerializeField] private GameObject artificialHorizon;

    [SerializeField] private GameObject pitchMeter;
    [SerializeField] private GameObject pitchMeterAnchor; //Ensures that the rotation of pitchMeter hapens around the same point as the artificial horizon
    [SerializeField] private RawImage compassImage;

    [SerializeField] private TextMeshProUGUI gpsStatusText;
    [SerializeField] private TextMeshProUGUI gpsTimeText;


    [SerializeField] private GameObject altitudeNumbersContainer;
    [SerializeField] private TextMeshProUGUI altitudeMeterText;

    [SerializeField] private GameObject airspeedNumbersContainer;
    [SerializeField] private TextMeshProUGUI airspeedMeterText;
    [SerializeField] private TextMeshProUGUI ASText;

    //private GameObject bankAngleArrowAnchor;
    [SerializeField] private Transform drone;

    [SerializeField] private float amplitude = 500;

    private float pitchMeterStartYpos;

    private FixedWing_Controller fwController;
    private DroneVariables droneVariables;


    private Color costumRed = new Color(191f / 255, 0f / 255, 0f / 255, 100f);
    private void Start()
    {
        pitchMeterStartYpos = pitchMeter.transform.localPosition.y;

        fwController = FindAnyObjectByType<FixedWing_Controller>();
        droneVariables = FindAnyObjectByType<DroneVariables>();
    }


    void Update()
    {
        ArtificialHorizon();
        CompassBar();
        GpsText();
        GpsTime();
        AltitudeMeter();
        AirspeedMeter();
    }

    private void ArtificialHorizon()
    {
        //Stores the drone's rotation in a Quaternion
        float droneRotationZ = drone.localEulerAngles.z;

        //Rotates artificial horizon around the z-axis
        artificialHorizon.transform.Rotate(Vector3.forward, -droneRotationZ - artificialHorizon.transform.localEulerAngles.z, Space.Self);

        pitchMeterAnchor.transform.Rotate(Vector3.forward, -droneRotationZ - pitchMeterAnchor.transform.localEulerAngles.z, Space.Self);

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

    public void UpdateTextAndColor(TextMeshProUGUI textElement, string text, Color color)
    {
        textElement.text = text;
        textElement.color = color;
    }

    private void GpsText()
    {
        if(fwController.autoPilot == true)
        {
            UpdateTextAndColor(gpsStatusText, "GPS: Fixed", Color.white);
        }
        else
        {
            UpdateTextAndColor(gpsStatusText, "GPS: No Fix", costumRed);
        }
    }

    private void GpsTime()
    {
        UpdateTextAndColor(gpsTimeText, System.DateTime.Now.ToString("HH:mm:ss"), Color.white);
    }

    private void AltitudeMeter()
    {
        float desiredYpos = drone.position.y * 18.5f; //18.5 is 92.5 divided by 5, because the altitudeNumbers are in intervals of 5 and the distance between them in the panel is 92.5 on the y-axis

        altitudeNumbersContainer.transform.localPosition = new Vector2(0, Mathf.Clamp(-desiredYpos, -1850, 185)); //-desiredYpos because the altitudeNumber's y-pos should be inverted of the drones in order for the altitudeNumbers to rise

        altitudeMeterText.text = Mathf.RoundToInt(drone.position.y).ToString();
    }

    private void AirspeedMeter()
    {
        float scaleFactor = 92.5f / 5;

        float airSpeedMeterYpos = droneVariables._droneVelocity * scaleFactor;

        airspeedNumbersContainer.transform.localPosition = new Vector2(0, Mathf.Clamp(-airSpeedMeterYpos, -1850, 185));

        airspeedMeterText.text = Mathf.RoundToInt(airSpeedMeterYpos).ToString();

        ASText.text = droneVariables._droneVelocity.ToString();
    }
}
