// Fade inspired by: https://www.youtube.com/watch?v=Ox0JCbVIMCQ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _fadeIn = false;
    [SerializeField] private bool _fadeOut = false;
    [SerializeField] private float _fadeDuration = 1;
    [SerializeField] private int sceneIndex;
    [SerializeField] private bool sceneChange = false;
    public bool disarmNotAllowed = true;

    public GameObject _menu;

    [SerializeField] private GameObject gpsJamCanvas;
    [SerializeField] private GameObject tabletConnLossCanvas;
    [SerializeField] private GameObject brokenOnTakeoffEmergencyCanvas;
    [SerializeField] private GameObject endScreenCanvas;
    [SerializeField] private TMP_Text reportStatusText;

    [SerializeField] private Button gpsJamButton;
    [SerializeField] private Button brokenOnTakeoffEmergencyButton;
    [SerializeField] private Button tabletConnLossButton;
    //[SerializeField] private Button randomEmergencyButton;

    [SerializeField] private GameObject _leftRayInteractor;
    [SerializeField] private GameObject _rightRayInteractor;

    private EmergencyManager _emergencyManager;

    public Transform _mainCamera;
    public float _spawnDistance = 1;
    public InputActionProperty _showMenuButton;

    private void Start()
    {
        try
        {
            _emergencyManager = FindObjectOfType<EmergencyManager>();

            //This assigns the corresponding methods from EmergencyManager to their respective buttons in the main menu
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {   
                gpsJamButton.onClick.AddListener(delegate { _emergencyManager.GPSjam(); });
                tabletConnLossButton.onClick.AddListener(delegate { _emergencyManager.TabletConnLoss(); });
                brokenOnTakeoffEmergencyButton.onClick.AddListener(delegate { _emergencyManager.BrokenOnTakeoffEmergency(); });
                //randomEmergencyButton.onClick.AddListener(delegate { _emergencyManager.RandomEmergency(); });
            }

        }
        catch
        {
            Debug.Log("emergencymanagerNotFound");
            
        }

        FadeOut();
    }

    void Update()
    {
        if (_showMenuButton.action.WasPressedThisFrame())
        {
            ActivateDeactivateMenu();

            if (_menu.activeSelf)
            {
                _menu.transform.position = _mainCamera.position + new Vector3(_mainCamera.forward.x, 0, _mainCamera.forward.z).normalized * _spawnDistance;
            }
        }

        _menu.transform.LookAt(new Vector3(_mainCamera.position.x, _menu.transform.position.y, _mainCamera.position.z));
        _menu.transform.forward *= -1;

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
    //----------------------------------------

    public void FadeIn()
    {
        _fadeIn = true;
    }

    public void FadeOut()
    {
        _fadeOut = true;
    }

    public static bool GameIsPaused = false;
    
    public void ActivateDeactivateMenu()
    {
        _menu.SetActive(!_menu.activeSelf);
        //_leftRayInteractor.SetActive(!_leftRayInteractor.activeSelf);
        //_rightRayInteractor.SetActive(!_rightRayInteractor.activeSelf);
        //More clunky way to disable objects, though these are script specific
        if(_rightRayInteractor.GetComponent<XRRayInteractor>().enabled == false || _leftRayInteractor.GetComponent<XRRayInteractor>().enabled == false)
        {
            _rightRayInteractor.GetComponent<XRRayInteractor>().enabled = true;
            _leftRayInteractor.GetComponent<XRRayInteractor>().enabled = true;
        }
        else if (_rightRayInteractor.GetComponent<XRRayInteractor>().enabled == true || _leftRayInteractor.GetComponent<XRRayInteractor>().enabled == true)
        {
            _rightRayInteractor.GetComponent<XRRayInteractor>().enabled = false;
            _leftRayInteractor.GetComponent<XRRayInteractor>().enabled = false;
        }        
        

        /*if (GameIsPaused)
        {
            ResumeTime();
        }
        else
        {
            PauseTime();
        }*/
    }
   
    void ResumeTime(){
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void PauseTime(){ 
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadScene(int index)
    {
        sceneIndex = index;
        //sceneChange = true;
        StartCoroutine(ChangeScene());
    }

    public IEnumerator ChangeScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneIndex);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //ResumeTime();
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void MissionEndScreen()
    {
        endScreenCanvas.SetActive(true);

        CrashLandingDetection _crashLandingDetection = FindObjectOfType<CrashLandingDetection>();

        if (_crashLandingDetection.droneCrashed == true /*&& disarmNotAllowed == true*/)
        {
            reportStatusText.text = "Fail!";
        }
        else
        {
            reportStatusText.text = "Success!";
        }

        switch (_emergencyManager.currentEmergency)
        {
            case 0:
                break;
            //free flight endscreencanvas
            case 1:
                gpsJamCanvas.SetActive(gpsJamCanvas);

                break;
            case 2:
                tabletConnLossCanvas.SetActive(tabletConnLossCanvas);
                break;
            case 3:
                brokenOnTakeoffEmergencyCanvas.SetActive(brokenOnTakeoffEmergencyCanvas);
                break;
        }
        endScreenCanvas.transform.position = _mainCamera.position + new Vector3(_mainCamera.forward.x, 0, _mainCamera.forward.z).normalized * _spawnDistance;
        endScreenCanvas.transform.LookAt(new Vector3(_mainCamera.position.x, _menu.transform.position.y, _mainCamera.position.z));
        endScreenCanvas.transform.forward *= -1;


    }
}
