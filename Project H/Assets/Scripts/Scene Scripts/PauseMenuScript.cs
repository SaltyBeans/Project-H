using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject player;
    private bool pauseMenuIsOn;

    void Start()
    {
        pauseMenu.SetActive(false);
        pauseMenuIsOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Input.GetKey(KeyCode.Tab))
        {
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
        pauseMenu.SetActive(false);
        pauseMenuIsOn = false;
        GetComponent<WaveBehaviour>().SaveGame();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void resumeGame()
    {
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
