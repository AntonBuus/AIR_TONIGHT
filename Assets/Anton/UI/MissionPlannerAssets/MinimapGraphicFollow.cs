using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGraphicFollow : MonoBehaviour
{
    public Transform _playerPositionGraphic;


    void LateUpdate()
    {
        Vector3 _newPosition = _playerPositionGraphic.position;

        _newPosition.y = transform.position.y;
        transform.position = _newPosition;

        transform.rotation = Quaternion.Euler(90f, _playerPositionGraphic.eulerAngles.y, 0f);


    }
}
