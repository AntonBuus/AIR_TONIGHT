using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RcController : MonoBehaviour
{
    [SerializeField] private GameObject autoPilotOnOverlay;

    FixedWing_Controller fixedWing_Controller;

    private void Start()
    {
        fixedWing_Controller = FindObjectOfType<FixedWing_Controller>();
    }


    void Update()
    {
        if(fixedWing_Controller.autoPilot == true)
        {
            autoPilotOnOverlay.SetActive(true);
        }
        else
        {
            autoPilotOnOverlay.SetActive(false);
        }
    }
}
