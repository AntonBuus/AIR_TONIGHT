using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if(fixedWing_Controller.autoPilot == true ||fixedWing_Controller.rtlActive == true)
        {
            autoPilotOnOverlay.SetActive(true);
        }
        else
        {
            autoPilotOnOverlay.SetActive(false);
        }
    }

/*    public void AutoPilotOnOffButton(bool onOff)
    {
        autoPilotOnOff.GetComponent<Button>().enabled = onOff;
    }*/
}
