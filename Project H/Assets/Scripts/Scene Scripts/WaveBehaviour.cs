using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class WaveBehaviour : MonoBehaviour
{
    public Behaviour[] disableComponents;

    private Transform playerStartingTransform;
    private Transform officialStartingTransform;
    private Transform moneySpawnTransform;

    [SerializeField]
    private GameObject official;
    [SerializeField]
    private GameObject EndWaveTexture;

    private HideWaveScript WaveScript;

    [SerializeField]
    private Text Text1;
    [SerializeField]
    private Text Text2;
    [SerializeField]
    private Text Text3;

    bool textUp = false;
    private Text currentText;

    bool successfulPlayer = false;
    bool levelFinished = false;

    public uint successfulWaves { get; private set; }

    void Awake()
    {
        setComponents(false); //Disable the character so it won't move.
        WaveScript = GetComponent<HideWaveScript>();
        GameObject.Destroy(GameObject.Find("BackgroundMusic"));//Kill the title music.
        playerStartingTransform = GameObject.Find("SpawnPositions/PlayerSpawnPosition").GetComponent<Transform>();
        officialStartingTransform = GameObject.Find("SpawnPositions/OfficialSpawnPosition").GetComponent<Transform>();
        moneySpawnTransform = GameObject.Find("SpawnPositions/MoneySpawnPosition").GetComponent<Transform>();

        StartTheWave();
    }


    void Update()
    {
        if (official.activeSelf && !textUp) //Official is enabled && level end texts are not up
        {
            EndTheWave();
        }

        if (textUp)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //Skip option.
                currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 1f); //Make the current text's alpha 1.


            if (currentText.color.a >= 0.992f)
            //If texts are showing and the current text is almost full alpha.
            {

                if (currentText == Text1)
                {
                    StartCoroutine(FadeTo(Text2, 1.0f, 3.0f));
                    currentText = Text2;
                }

                else if (currentText == Text2)
                {
                    StartCoroutine(FadeTo(Text3, 1.0f, 3.0f));
                    currentText = Text3;
                }
                else if (currentText == Text3)
                {
                    textUp = false;
                    EndWaveTexture.SetActive(false);
                    setComponents(true);

                    if (levelFinished)
                    {
                        ResetTheWave();

                        if (successfulPlayer) //If player is successful
                        {
                            StartTheWave();
                        }

                        else if (!successfulPlayer) // if player is not successful
                        {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.None;
                            // Unlock the cursor before loading the title screen.
                            GetComponent<ClickToLoadAsync>().ClickAsync(0); //Load the title screen.
                        }
                    }

                }

            }
        }
    }

    void StartTheWave()
    {
        //Stop the fadeIn coroutine so the next 3 text's will be transparent in the next frame.
        StopAllCoroutines();
        // A coroutine artifact started from previous fadeIn makes 3rd text visible before the 1st and 2nd.

        Text1.text = "You need to hide the evidence.";
        Text2.text = "This week it's 50.000$.\n Next week there will be more.";
        Text3.text = "You don't have much time.";

        Color c = Text1.color;
        c.a = 0f;           //Make the texts transparent.
        Text1.color = c;
        Text2.color = c;
        Text3.color = c;

        textUp = true;
        currentText = Text1;
        setComponents(false);
        StartCoroutine(FadeTo(Text1, 1.0f, 2.0f));
        EndWaveTexture.SetActive(true);
    }

    void EndTheWave()
    {
        if (official.GetComponentInChildren<NPCDetection>().moneyFound) //Money found in the House.
        {
            Text1.text = "Marked bills have been found in your house.";
            Text2.text = "It's over.\nYou have been declared a traitor. \nAll of your connections to the agency have been terminated.";
            Text3.text = "You are on your own.";

            Color c = Text1.color;
            c.a = 0f;
            Text1.color = c;
            Text2.color = c;
            Text3.color = c;

            textUp = true;
            currentText = Text1;

            EndWaveTexture.SetActive(true);
            setComponents(false);
            StartCoroutine(FadeTo(Text1, 1.0f, 2.0f));
            successfulPlayer = false;
            levelFinished = true;
        }

        if (official.GetComponent<AIControl>().inspectionComplete)  //Inspection Complete in house. So money was not found in the house.
        {
            levelFinished = true;
            successfulPlayer = true;
            Text1.text = "Great Job";
            Text2.text = "Your superiors are proud.";
            Text3.text = "But it's not over.";

            Color c = Text1.color;
            c.a = 0f;           //Make the texts transparent.
            Text1.color = c;
            Text2.color = c;
            Text3.color = c;

            textUp = true;
            currentText = Text1;

            EndWaveTexture.SetActive(true);
            setComponents(false); //Set the components false so player can't move.
            StartCoroutine(FadeTo(Text1, 1.0f, 2.0f)); //Fade the screen to black.
        }
    }

    /// <summary>
    /// Resets the locations of the player and official. Calculates the appropriate time and assigns that to the timer.
    /// </summary>
    void ResetTheWave()
    {
        GameObject official = GameObject.FindGameObjectWithTag("Official");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = playerStartingTransform.position; //Reset the position of the player.
        player.transform.rotation = playerStartingTransform.rotation; //Reset the rotation of the player.

        official.transform.position = playerStartingTransform.position; //Reset the position of the official.
        official.transform.rotation = playerStartingTransform.rotation; //Reset the rotation of the official.
        official.SetActive(false); //Disable the official.

        //TODO: Reset the hide timer.
        //TODO: Calculate the appropriate timer for the timer.
        //TODO: Calculate the appropriate money for the next wave.
    }


    IEnumerator FadeTo(Text _text, float aValue, float aTime)
    {
        float alpha = _text.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(_text.color.r, _text.color.g, _text.color.b, Mathf.Lerp(alpha, aValue, t));
            _text.color = newColor;
            yield return null;
        }
    }

    private void setComponents(bool _state)
    {
        foreach (Behaviour item in disableComponents)
            item.enabled = _state;
    }



}