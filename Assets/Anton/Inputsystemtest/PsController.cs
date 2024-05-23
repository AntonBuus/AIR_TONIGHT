using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PsController : MonoBehaviour
{
    DroneControls controls;

    //int acenddecendVariable = 1;

    Vector2 move;

    Vector2 rotate;


    void Awake()
    {
        controls = new DroneControls();

        //controls.Drone1.Acend.performed += ctx => Acend();
        //controls.Drone1.Decend.performed += ctx => Decend();
        controls.Drone1.Grow.performed += ctx => Grow();

        //controls.Drone1.Move1.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.Drone1.Move1.performed += ctx => move = Vector2.zero;



    }
    public void Update()
    {
        move = controls.Drone1.Move1.ReadValue<Vector2>();
        Vector2 m = new Vector2(0, move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);
        //Debug.Log(m);

        //rotate = controls.Drone1.Rotate.ReadValue<Vector2>();
        Vector2 r = new Vector2(move.x, 0) * 100f * Time.deltaTime;
        transform.Rotate(0,move.x*100f*Time.deltaTime,0, Space.World);
        Debug.Log(r);

        //transform.Rotate(0,0.1f,0,Space.World);


    }

    void Grow()
    {
        transform.localScale *= 1.1f;
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
