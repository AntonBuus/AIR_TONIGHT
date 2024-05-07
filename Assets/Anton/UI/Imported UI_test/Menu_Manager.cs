using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Menu_Manager : MonoBehaviour
{
    public GameObject _menu;

    [SerializeField] private GameObject gpsJamCanvas;
    [SerializeField] private GameObject tabletConnLossCanvas;
    [SerializeField] private GameObject brokenOnTakeoffEmergencyCanvas;
    [SerializeField] private GameObject reportStatusGO;
    [SerializeField] private TMP_Text reportStatusText;

    [SerializeField] private GameObject _leftRayInteractor;
    [SerializeField] private GameObject _rightRayInteractor;

    private EmergencyManager _emergencyManager;
    private int _activeEmergency;

    public Transform _mainCamera;
    public float _spawnDistance = 1;
    public InputActionProperty _showMenuButton;

    private Vector3 _relativePosition;

    private void Start()
    {
        _emergencyManager = FindObjectOfType<EmergencyManager>();
        _activeEmergency = _emergencyManager.currentEmergency;
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
    }
    //----------------------------------------

    public static bool GameIsPaused = false;
    
    public void ActivateDeactivateMenu()
    {
        _menu.SetActive(!_menu.activeSelf);
        _leftRayInteractor.SetActive(!_leftRayInteractor.activeSelf);
        _rightRayInteractor.SetActive(!_rightRayInteractor.activeSelf);
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
        SceneManager.LoadScene(index);
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
        Debug.Log("mission end screen called");
        CrashLandingDetection _crashLandingDetection = FindObjectOfType<CrashLandingDetection>();
        bool _droneCrashed = _crashLandingDetection.droneCrashed;

        if (_droneCrashed == true)
        {
            reportStatusText.text = "Scenario failed!";
        }
        else
        {
            reportStatusText.text = "Scenario successful!";
        }

        reportStatusGO.SetActive(true);

        switch (_activeEmergency)
        {
            case 1:
                gpsJamCanvas.SetActive(gpsJamCanvas);
                gpsJamCanvas.transform.position = _mainCamera.position + new Vector3(_mainCamera.forward.x, 0, _mainCamera.forward.z).normalized * _spawnDistance;
                gpsJamCanvas.transform.LookAt(new Vector3(_mainCamera.position.x, _menu.transform.position.y, _mainCamera.position.z));
                gpsJamCanvas.transform.forward *= -1;
                break;
            case 2:
                tabletConnLossCanvas.SetActive(tabletConnLossCanvas);
                tabletConnLossCanvas.transform.position = _mainCamera.position + new Vector3(_mainCamera.forward.x, 0, _mainCamera.forward.z).normalized * _spawnDistance;
                tabletConnLossCanvas.transform.LookAt(new Vector3(_mainCamera.position.x, _menu.transform.position.y, _mainCamera.position.z));
                tabletConnLossCanvas.transform.forward *= -1;
                break;
            case 3:
                brokenOnTakeoffEmergencyCanvas.SetActive(brokenOnTakeoffEmergencyCanvas);
                brokenOnTakeoffEmergencyCanvas.transform.position = _mainCamera.position + new Vector3(_mainCamera.forward.x, 0, _mainCamera.forward.z).normalized * _spawnDistance;
                brokenOnTakeoffEmergencyCanvas.transform.LookAt(new Vector3(_mainCamera.position.x, _menu.transform.position.y, _mainCamera.position.z));
                brokenOnTakeoffEmergencyCanvas.transform.forward *= -1;
                break;
        }
    }
}
