using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchRes : MonoBehaviour
{
    private Vector2 screenResolution;
    // Start is called before the first frame update
    void Start()
    {
        screenResolution = new Vector2(Screen.width, Screen.height);
        MatchScreenRes();
    }

    // Update is called once per frame
    void Update()
    {
        if (screenResolution.x != Screen.width || screenResolution.y != Screen.height)
        {
            MatchScreenRes();
        }
    }
    private void MatchScreenRes()
    {
        gameObject.transform.localScale = new Vector3(Screen.width, Screen.height);
    }

}
