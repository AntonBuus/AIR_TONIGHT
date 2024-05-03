using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Menu_Manager : MonoBehaviour
{
    public GameObject _menu;

    public GameObject _rayInteractor1;
    public GameObject _rayInteractor2;

    public Transform _head;
    public float _spawnDistance = 2;
    public InputActionProperty _showMenuButton;

    private Vector3 _relativePosition;

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
        _rayInteractor1.SetActive(!_rayInteractor1.activeSelf);
        _rayInteractor2.SetActive(!_rayInteractor2.activeSelf);
        //if(GameIsPaused)
        //{
        //    ResumeTime();
        //}
        //else
        //{
        //    PauseTime();
        //}
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

    public void EmergencySuccess()
    {
        Debug.Log("Emergency success");
    }

    public void EmergencyFail()
    {
        Debug.Log("Emergency fail");
    }
}
