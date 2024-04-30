using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class Menu_Manager : MonoBehaviour
{
    public GameObject _menu;
    
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
}
