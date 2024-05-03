// Source: https://www.youtube.com/watch?v=Ox0JCbVIMCQ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _fadeIn = false;
    [SerializeField] private bool _fadeOut = false;
    [SerializeField] private float _fadeDuration;
    //[SerializeField] private bool _faded = false;

    public void FadeIn()
    {
        _fadeIn= true;
    }

    public void FadeOut()
    {
        _fadeOut = true;
    }

    void Update()
    {
        if (_fadeIn == true)
        {
            if (_canvasGroup.alpha <= 1)
            {
                _canvasGroup.alpha += _fadeDuration * Time.deltaTime;
                if (_canvasGroup.alpha == 1)
                {
                    _fadeIn = false;
                }
            }
        }

        if (_fadeOut == true)
        {
            if (_canvasGroup.alpha >= 0)
            {
                _canvasGroup.alpha -= _fadeDuration * Time.deltaTime;
                if (_canvasGroup.alpha == 0)
                {
                    _fadeOut = false;
                }
            }
        }
    }
}
