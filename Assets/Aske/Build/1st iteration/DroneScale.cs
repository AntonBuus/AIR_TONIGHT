using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DroneScale : MonoBehaviour
{
    [SerializeField] private float distBetweenPlayerAndDrone;
    [SerializeField] private float maxScale = 10f;
    [SerializeField] private float droneScaleDivide = 100f;
    [SerializeField] private float minDistance = 100f;
    //[SerializeField] private GameObject playerPosition;

    void Update()
    {
        distBetweenPlayerAndDrone = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (distBetweenPlayerAndDrone > minDistance)
        {
            float droneScaling = Vector3.Distance(Camera.main.transform.position, transform.position) / droneScaleDivide;
            float droneScalingMax = Mathf.Clamp(droneScaling, 1, maxScale); //this restricts the value going above or below set numbers.
            transform.localScale = new Vector3(droneScalingMax, droneScalingMax, droneScalingMax);
        }
    }
}