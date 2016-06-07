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
        PlayerPrefs.DeleteAll();
        GetComponent<GameInstanceInformation>().loadedGame = false;
        GameObject.Find("Canvas").GetComponent<ClickToLoadAsync>().ClickAsync(1);
    }
    public void quitGame()
    {
        Application.Quit();
    }

    public void loadGame()
    {
        GetComponent<GameInstanceInformation>().loadedGame = true;
        GameObject.Find("Canvas").GetComponent<ClickToLoadAsync>().ClickAsync(1);
    }


}
