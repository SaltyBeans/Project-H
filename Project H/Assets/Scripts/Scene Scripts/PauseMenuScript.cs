using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject player;
    private bool pauseMenuIsOn;

	void Start ()
	{
        pauseMenu.SetActive(false);
	    pauseMenuIsOn = false;
	}
	
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape)&&!Input.GetKey(KeyCode.Tab))
	    {
            Debug.Log("Escape Pressed");
	        if (Time.timeScale == 1)
	        {
                pauseMenu.SetActive(true);
                player.GetComponent<Crosshair>().OriginalOn = false;
                player.GetComponent<Crosshair>().CursorLock = false;
                pauseMenuIsOn = true;
                Time.timeScale = 0;
            }
            else if (Time.timeScale == 0)
            {
                pauseMenu.SetActive(false);
                player.GetComponent<Crosshair>().OriginalOn = true;
                player.GetComponent<Crosshair>().CursorLock = true;
                pauseMenuIsOn = false;
                Time.timeScale = 1;
            }
	    }
	}

    public void quitToDesktop()
    {
        Debug.Log("Quit to desktop Pressed");
        Application.Quit();
    }

    public void quitToMainMenu()
    {
        Debug.Log("Quit To Main Menu Pressed");
        pauseMenu.SetActive(false);
        pauseMenuIsOn = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void saveGame()
    {
        Debug.Log("Save Game Pressed");
    }

    public void resumeGame()
    {
        Debug.Log("Resume Game Pressed");
        pauseMenu.SetActive(false);
        player.GetComponent<Crosshair>().OriginalOn = true;
        player.GetComponent<Crosshair>().CursorLock = true;
        pauseMenuIsOn = false;
        Time.timeScale = 1;
    }

    public bool getPauseMenuStatus()
    {
        return pauseMenuIsOn;
    }
}
