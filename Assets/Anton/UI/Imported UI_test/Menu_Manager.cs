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

    [SerializeField] private GameObject endScreenCanvas;
    [SerializeField] private GameObject gpsJamCanvas;
    [SerializeField] private GameObject tabletConnLossCanvas;
    [SerializeField] private GameObject brokenOnTakeoffEmergencyCanvas;
    [SerializeField] private TMP_Text reportStatusText;

    private EmergencyManager _emergencyManager;
    private int _activeEmergency;

    private CrashLandingDetection _crashLandingDetection;
    private bool _droneCrashed;

    public Transform _head;
    public float _spawnDistance = 2;
    public InputActionProperty _showMenuButton;

    private Vector3 _relativePosition;

    private void Start()
    {
        _emergencyManager = FindObjectOfType<EmergencyManager>();
        _activeEmergency = _emergencyManager.currentEmergency;

        _emergencyManager = FindObjectOfType<CrashLandingDetection>();
        _droneCrashed = CrashLandingDetection.droneCrashed;
    }

    void Update()
    {
        if (_showMenuButton.action.WasPressedThisFrame())
        {
            ActivateDeactivateMenu();
            Debug.Log("Menu Pressed!");

            if (_menu.activeSelf)
            {
                _menu.transform.position = _head.position + new Vector3(_head.forward.x, 0, _head.forward.z).normalized * _spawnDistance;
            }
        }

        _menu.transform.LookAt(new Vector3(_head.position.x, _menu.transform.position.y, _head.position.z));
        _menu.transform.forward *= -1;
    }
    //----------------------------------------

    public static bool GameIsPaused = false;
    
    public void ActivateDeactivateMenu()
    {
        _menu.SetActive(!_menu.activeSelf);
        if(GameIsPaused)
        {
            ResumeTime();
        }
        else
        {
            PauseTime();
        }
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
        ResumeTime();
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT Game!");
    }

    public void MissionEndScreen()
    {
        Debug.Log("Emergency success");
        PauseTime();

        if (_droneCrashed == true)
        {
            reportStatusText.text = "Scenario failed!";
        }
        else
        {
            reportStatusText.text = "Scenario successful!";
        }

        endScreenCanvas.SetActive(true);

        switch (_activeEmergency)
        {
            case 1:
                gpsJamCanvas.SetActive(gpsJamCanvas);
                gpsJamCanvas.transform.position = _head.position + new Vector3(_head.forward.x, 0, _head.forward.z).normalized * _spawnDistance;
                gpsJamCanvas.transform.LookAt(new Vector3(_head.position.x, _menu.transform.position.y, _head.position.z));
                gpsJamCanvas.transform.forward *= -1;
                break;
            case 2:
                tabletConnLossCanvas.SetActive(tabletConnLossCanvas);
                tabletConnLossCanvas.transform.position = _head.position + new Vector3(_head.forward.x, 0, _head.forward.z).normalized * _spawnDistance;
                tabletConnLossCanvas.transform.LookAt(new Vector3(_head.position.x, _menu.transform.position.y, _head.position.z));
                tabletConnLossCanvas.transform.forward *= -1;
                break;
            case 3:
                brokenOnTakeoffEmergencyCanvas.SetActive(brokenOnTakeoffEmergencyCanvas);
                brokenOnTakeoffEmergencyCanvas.transform.position = _head.position + new Vector3(_head.forward.x, 0, _head.forward.z).normalized * _spawnDistance;
                brokenOnTakeoffEmergencyCanvas.transform.LookAt(new Vector3(_head.position.x, _menu.transform.position.y, _head.position.z));
                brokenOnTakeoffEmergencyCanvas.transform.forward *= -1;
                break;
        }
    }
}
