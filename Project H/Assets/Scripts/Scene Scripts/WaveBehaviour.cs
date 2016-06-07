using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveBehaviour : MonoBehaviour
{
    public Behaviour[] disableComponents;

    private Transform playerStartingTransform;
    private Transform officialStartingTransform;
    private Transform moneySpawnTransform;

    private HideWaveScript hideScript;
    [SerializeField]
    private GameObject official;
    [SerializeField]
    private GameObject EndWaveTexture;

    [SerializeField]
    private Text Text1;
    [SerializeField]
    private Text Text2;
    [SerializeField]
    private Text Text3;

    bool textUp = false;
    private Text currentText;
    private float currentCash = 50000;
    bool successfulPlayer = false;
    bool levelFinished = false;
    GameInstanceInformation instanceInfo;
    public uint successfulWaves { get; private set; }
    void Awake()
    {
        setComponents(false);                                                         //Disable the character components so he won't move.
        hideScript = GameObject.Find("LevelManager").GetComponent<HideWaveScript>();
        GameObject.Destroy(GameObject.Find("BackgroundMusic")); //Kill the title music.
        playerStartingTransform = GameObject.Find("SpawnPositions/PlayerSpawnPosition").GetComponent<Transform>();
        officialStartingTransform = GameObject.Find("SpawnPositions/OfficialSpawnPosition").GetComponent<Transform>();
        moneySpawnTransform = GameObject.Find("SpawnPositions/MoneySpawnPosition").GetComponent<Transform>();

        official.SetActive(false);
        instanceInfo = GameObject.Find("Game Information").GetComponent<GameInstanceInformation>();

        AssetMenuScript assetScript = GetComponent<AssetMenuScript>();

        if (instanceInfo.loadedGame) //If player clicked on load game button
        {
            LoadInformation.LoadAllInformation();
            official.GetComponent<OfficialAttention>().officialAttention = GameInstanceInformation.PlayerAttentionLevel;

            successfulWaves = (uint)GameInstanceInformation.SuccessfulWaves;

            foreach (var mon in GameInstanceInformation.MoneyLocations)
                GameObject.Instantiate(moneyPrefab, mon, Quaternion.identity);


            if (GameInstanceInformation.PlayerMoney % 10000 != 0)
                GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyScript10K>().setMoneyAmout(Mathf.FloorToInt(GameInstanceInformation.PlayerMoney % 10000));


            if (GameInstanceInformation.SledgehammerBought)
                assetScript.sledgeBought = true;

            if (GameInstanceInformation.BasementBought)
                assetScript.basementKeyBought = true;

            assetScript.placeBoughtItems();
        }
        else
        {
            official.GetComponent<OfficialAttention>().officialAttention = 0;
            assetScript.sledgeBought = false;
            assetScript.basementKeyBought = false;
            assetScript.itemYBought = false;
            assetScript.itemZBought = false;
            successfulWaves = 0;
        }

        StartTheWave();
    }


    void Update()
    {
        if (official.activeSelf && !textUp) //Official is enabled && level end texts are not up
        {
            //TODO: Fix this logic.
            EndTheWave(); // Game will not end until a stack is found OR inspection is complete. Those checks are inside EndTheWave()  
        }

        if (textUp)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //Skip option.
                currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, 1f); //Make the current text's alpha 1.


            if (currentText.color.a >= 0.992f)   //If the current text is almost full alpha.
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

                        if (successfulPlayer) //If player is successful
                        {
                            ResetTheWave();
                            successfulWaves++;
                            StartTheWave();
                        }

                        else if (!successfulPlayer) // if player is not successful
                        {
                            GameObject.FindGameObjectWithTag("Player").GetComponent<Crosshair>().CursorLock = false;
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
        SaveGame();
        //Stop the fadeIn coroutine so the next 3 text's will be transparent in the next frame.
        StopAllCoroutines();
        // A coroutine artifact started from previous fadeIn makes 3rd text visible before the 1st and 2nd.

        Text1.text = "You need to hide the evidence.";
        Text2.text = "This week it's " +currentCash+ "$.\n Next week there will be more.";
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

        hideScript.hideTime = (((int)successfulWaves + 1f) * 10f) + 5f;              // +5 is for the text screen.
        spawnMoney(((int)successfulWaves + 1) * 10000 * 5, moneySpawnTransform.position); //Spawn the money
    }

    void EndTheWave()
    {
        if (official.GetComponentInChildren<NPCDetection>().moneyFound && official.GetComponentInChildren<NPCDetection>().endWave) //Money found in the House.
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
            currentCash = ((int)successfulWaves + 2) *10000 * 5;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera.main.enabled = true;
        official.SetActive(true);
        Debug.Log("official state " + official.activeSelf);

        player.transform.position = playerStartingTransform.position; //Reset the position of the player.
        player.transform.rotation = playerStartingTransform.rotation; //Reset the rotation of the player.

        official.gameObject.transform.position = officialStartingTransform.position; //Reset the position of the official.
        official.gameObject.transform.rotation = officialStartingTransform.rotation; //Reset the rotation of the official.
        official.GetComponent<AIControl>().inspectionComplete = false;
        official.GetComponent<OfficialStateMachine>().ChangeCurrentState(new GoToDoor());
        levelFinished = false;

        official.SetActive(false); //Disable the official.
        GetComponent<AssetMenuScript>().placeBoughtItems();
        Debug.Log("official state " + official.activeSelf);
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

    /// <summary>
    /// Sets the components of the player to desired state.
    /// </summary>
    /// <param name="_state"></param>
    public void setComponents(bool _state)
    {
        foreach (Behaviour item in disableComponents)
            item.enabled = _state;
    }

    [SerializeField]
    private Vector3 SpawnSquare;

    [SerializeField]
    private GameObject moneyPrefab;

    /// <summary>
    /// Spawn specific money at a spawn position.
    /// </summary>
    /// <param name="amount">Money amount to be spawned.</param>
    /// <param name="spawnCenter">Location of the spawn, bottom left corner.</param>
    void spawnMoney(int amount, Vector3 spawnCenter)
    {
        int numberOfFullStacks = 0;
        int numberOfBelowMaxStacks = 0;
        int maxMoneyAmount = 10000;

        numberOfFullStacks = Mathf.FloorToInt(amount / maxMoneyAmount);
        if (numberOfFullStacks == 0 || amount % maxMoneyAmount > 0)
            numberOfBelowMaxStacks++; //This will be either zero or one, maybe boolean here.


        int spawnCount = 0;

        for (int j = 0; j < numberOfFullStacks; j++)
        {
            Vector3 post = spawnCenter;
            GameObject money = GameObject.Instantiate(moneyPrefab, post, Quaternion.identity) as GameObject;
            post.x += (spawnCount * money.GetComponent<BoxCollider>().bounds.size.x) % SpawnSquare.x;
            post.z += ((Mathf.FloorToInt((spawnCount * money.GetComponent<BoxCollider>().bounds.size.x) / SpawnSquare.x)) * money.GetComponent<BoxCollider>().bounds.size.z) % SpawnSquare.z;
            post.y += (spawnCount / ((SpawnSquare.x / money.GetComponent<BoxCollider>().bounds.size.x) *
                                                     (SpawnSquare.z / money.GetComponent<BoxCollider>().bounds.size.z)) * money.GetComponent<BoxCollider>().bounds.size.y);
            money.transform.position = post;
            money.GetComponent<MoneyScript10K>().setMoneyAmout(maxMoneyAmount);
            spawnCount++;
        }

        for (int i = 0; i < numberOfBelowMaxStacks; i++)
        {
            Vector3 post = spawnCenter;
            GameObject money = GameObject.Instantiate(moneyPrefab, post, Quaternion.identity) as GameObject;
            post.x += (spawnCount * money.GetComponent<BoxCollider>().bounds.size.x) % SpawnSquare.x;
            post.z += ((Mathf.FloorToInt((spawnCount * money.GetComponent<BoxCollider>().bounds.size.x) / SpawnSquare.x)) * money.GetComponent<BoxCollider>().bounds.size.z) % SpawnSquare.z;
            post.y += (spawnCount / ((SpawnSquare.x / money.GetComponent<BoxCollider>().bounds.size.x) *
                                                     (SpawnSquare.z / money.GetComponent<BoxCollider>().bounds.size.z)) * money.GetComponent<BoxCollider>().bounds.size.y);
            money.transform.position = post;
            money.GetComponent<MoneyScript10K>().setMoneyAmout(amount % maxMoneyAmount);
        }
    }

    /// <summary>
    /// Sets the variables' values in GameInstanceInformation and calls SaveInformation.
    /// </summary>
    void SaveGame()
    {
        List<Vector3> moneyPosList = new List<Vector3>();
        Vector3[] moneyPosArray;
        foreach (var item in GameObject.FindGameObjectsWithTag("Money"))
            moneyPosList.Add(item.transform.position);

        moneyPosArray = moneyPosList.ToArray();

        GameInstanceInformation.MoneyLocations = moneyPosArray;

        GameInstanceInformation.PlayerAttentionLevel = GameObject.Find("LevelManager").GetComponent<OfficialAttention>().officialAttention;
        GameInstanceInformation.PlayerMoney = GameObject.Find("LevelManager").GetComponent<MoneyTransactionScript>().getTotalCash();

        GameInstanceInformation.SledgehammerBought = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<AssetMenuScript>().sledgeBought;
        GameInstanceInformation.BasementBought = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<AssetMenuScript>().basementKeyBought;
        GameInstanceInformation.SuccessfulWaves = (int)successfulWaves;

        SaveInformation.SaveAllInformation();
    }


}