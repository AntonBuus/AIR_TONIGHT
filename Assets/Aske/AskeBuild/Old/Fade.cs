// Source: https://www.youtube.com/watch?v=Ox0JCbVIMCQ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public CanvasGroup _canvasGroup;
    [SerializeField] private bool _fadeIn = false;
    public bool _fadeOut = false;
    [SerializeField] private float _fadeDuration;

    private void Start()
    {
        FadeOut();
    }

    public void FadeIn()
    {
        _fadeIn = true;
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
