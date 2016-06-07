using UnityEngine;

public class ButtonBehaviours : MonoBehaviour
{
    void Awake()
    {
        if (!PlayerPrefs.HasKey("PlayerMoney"))
        {
            GameObject.Find("Canvas/Load Game Button").SetActive(false);
        }
    }

    public void newGame()
    {
        Debug.Log("New Game Pressed");
        PlayerPrefs.DeleteAll();
        GetComponent<GameInstanceInformation>().loadedGame = false;
        GameObject.Find("Canvas").GetComponent<ClickToLoadAsync>().ClickAsync(1);
    }
    public void quitGame()
    {
        Debug.Log("Quit Game Pressed");
        Application.Quit();
    }

    public void loadGame()
    {
        Debug.Log("Load Game Pressed");
        GetComponent<GameInstanceInformation>().loadedGame = true;
        GameObject.Find("Canvas").GetComponent<ClickToLoadAsync>().ClickAsync(1);
    }


}
