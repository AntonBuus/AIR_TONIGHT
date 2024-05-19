using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public Transform _playerPosition;


    void LateUpdate()
    {
        Vector3 _newPosition = _playerPosition.position;

        _newPosition.y = transform.position.y;
        transform.position = _newPosition;
        
        //transform.rotation = Quaternion.Euler(90f, _playerPosition.eulerAngles.y, 0f);
        
    
    }





}

//-----------WIP deactivate rotation-----------------------
//public bool _minimapRotateActivated = true;

//void LateUpdate()
//{
//    Vector3 _newPosition = _playerPosition.position;

//    _newPosition.y = transform.position.y;
//    transform.position = _newPosition;
//    if (_minimapRotateActivated)
//    {
//        transform.rotation = Quaternion.Euler(90f, _playerPosition.eulerAngles.y, 0f);
//    }
//    else
//    {
//        return;
//    }

//}
