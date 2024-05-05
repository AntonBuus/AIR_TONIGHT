using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compassbar : MonoBehaviour
{
    public RawImage compass;
    public Transform drone;

    void Update()
    {
        compass.uvRect = new Rect(drone.localEulerAngles.y / 360f, 0, 1, 1);

    }
}
