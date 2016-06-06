using UnityEngine;
using System.Collections;

public class ButtonBehaviours : MonoBehaviour {

    public void quitGame()
    {
        Debug.Log("Quit Game Pressed");
        Application.Quit();
    }

    public void loadGame()
    {
        Debug.Log("Load Game Pressed");   
    }
}
