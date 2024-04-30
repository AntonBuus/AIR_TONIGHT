using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AppearInFrontUI : MonoBehaviour
{
    public GameObject _movingCanvas;
    
    public Transform _playerHead;
    public float _distanceFromPlayer = 2;

    private Vector3 relativePosition;

    void Update()
    {
        /*
            if (movingCanvas.activeSelf)
            {
                movingCanvas.transform.position = playerHead.position + new Vector3(playerHead.forward.x, 0, playerHead.forward.z).normalized * distanceFromPlayer;
            }
        */

        _movingCanvas.transform.LookAt(new Vector3(_playerHead.position.x, _movingCanvas.transform.position.y, _playerHead.position.z));
        _movingCanvas.transform.forward *= -1;
    }


    public void OnEnable() 
    {
        _movingCanvas.transform.position = _playerHead.position + new Vector3(_playerHead.forward.x, 0, _playerHead.forward.z).normalized * _distanceFromPlayer;
    }
    //make an on enable function that sets the position of the canvas to the player's head
     



    
}

