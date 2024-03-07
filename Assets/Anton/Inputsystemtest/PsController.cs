using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PsController : MonoBehaviour
{
    DroneControls controls;

    int acenddecendVariable = 1;

    Vector2 move;


    void Awake()
    {
        controls = new DroneControls();

        //controls.Drone1.Acend.performed += ctx => Acend();
        //controls.Drone1.Decend.performed += ctx => Decend();
    

        controls.Drone1.Move1.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Drone1.Move1.performed += ctx => move = Vector2.zero;

    }
    public void Update()
    {
        Vector2 m = new Vector2(-move.x, move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);
        Debug.Log(m);
    }

    void Acend()
    {
        //Vector2 m = new Vector2(move.x, acenddecendVariable) * Time.deltaTime;
        transform.localPosition *= 1.1f;
        //transform.Translate(m, Space.World);
        Debug.Log("Acending");
    }

    void Decend()
    {
        transform.localPosition *= 0.9f;
        Debug.Log("Decending");
    }


    private void OnEnable()
    {
        controls.Drone1.Enable();
    }

    private void OnDisable()
    {
        controls.Drone1.Disable();
    }

}
