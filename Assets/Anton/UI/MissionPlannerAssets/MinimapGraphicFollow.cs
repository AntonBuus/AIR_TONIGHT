using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapGraphicFollow : MonoBehaviour
{
    
    [SerializeField] Transform _playerPositionGraphic;
    [SerializeField] bool _rotateSprite = true;

    void LateUpdate()
    {
        Vector3 _newPosition = _playerPositionGraphic.position;

        _newPosition.y = transform.position.y;
        transform.position = _newPosition;
        if ( _rotateSprite )
        {
            transform.rotation = Quaternion.Euler(90f, _playerPositionGraphic.eulerAngles.y, 0f);
        }
        


    }
}
