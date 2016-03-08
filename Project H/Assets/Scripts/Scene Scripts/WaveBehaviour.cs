using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class WaveBehaviour : MonoBehaviour
{
    public Behaviour[] disableComponents;


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
    void Awake()
    {
        setComponents(false);

        WaveScript = GetComponent<HideWaveScript>();

        if (Application.loadedLevel == 1)
        {
            Text1.text = "You need to hide the evidence.";
            Text2.text = "This week it's 50.000$.\n Next week there will be more.";
            Text3.text = "You don't have much time.";

            currentText = Text1;
            StartCoroutine(FadeTo(Text1, 1.0f, 2.0f));

            EndWaveTexture.SetActive(true);
            textUp = true;
        }
        if (Application.loadedLevel == 2)
        {
            Text1.text = "You need to hide the evidence.";
            Text2.text = "This week it's 100.000$.\n Next week there will be more.";
            Text3.text = "You don't have much time.";

            currentText = Text1;
            StartCoroutine(FadeTo(Text1, 1.0f, 2.0f));

            EndWaveTexture.SetActive(true);
            textUp = true;
        }
        if (Application.loadedLevel == 3)
        {
            Text1.text = "You need to hide the evidence.";
            Text2.text = "This is your last week.";
            Text3.text = "You don't have much time.";

            currentText = Text1;
            StartCoroutine(FadeTo(Text1, 1.0f, 2.0f));

            EndWaveTexture.SetActive(true);
            textUp = true;
        }
    }


    void Update()
    {
        if (official.activeSelf && !textUp)
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

            if (official.GetComponent<AIControl>().inspectionComplete)  //Inspection Complete in house.
            {
                levelFinished = true;
                successfulPlayer = true;

                if (Application.loadedLevel == 1 || Application.loadedLevel == 2)   //First and Second end level end wave texts.
                {
                    Text1.text = "Great Job";
                    Text2.text = "Your superiors are proud.";
                    Text3.text = "But it's not over.";

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
                }

                if (Application.loadedLevel == 3)   //Final end level text.
                {
                    Text1.text = "Great Job.\nYour president is proud.";
                    Text2.text = "The assets you've acquired will be transferred to the agency.";
                    Text3.text = "You will keep the rest of the money.";

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

                }

            }
        }




        if (textUp == true && currentText.color.a >= 0.990f)
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
                    if (successfulPlayer && Application.loadedLevel != 3)   //If player is succesfull in 1st-2nd waves.
                    {
                        GetComponent<ClickToLoadAsync>().ClickAsync(Application.loadedLevel + 1);
                    }

                    else if (successfulPlayer && Application.loadedLevel == 3) //If the player is successful at 3rd wave.
                    {
                        Application.Quit();
                    }

                    else if (!successfulPlayer) // if player is not successsful
                    {
                        Application.Quit();
                    }
                }

            }

        }
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