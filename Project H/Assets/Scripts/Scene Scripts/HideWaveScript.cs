using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideWaveScript : MonoBehaviour {
    
    [SerializeField]
    private float hideTime=30f;

    private float minute;
    private float second;
    private float timer;

    public Text timerDisplay;
    public Text objectiveDisplay;

    private bool officialSpawned = false;


    public GameObject Official;
    //public Transform officialPosition;

    void Start()
    {
        Official.SetActive(false);
    }
	void Update () {
        if (hideTime > 0)
        {
            hideTime -= 1.0f * Time.deltaTime;
            minute = Mathf.Floor(hideTime / 60f);
            second = (hideTime % 60f);
            timerDisplay.text = "Time Left: " + string.Format("{0:00}:{1:00}", minute, second);
            objectiveDisplay.text = "Objective: Hide the money until the time runs out.";
        }
        else
        {
            timerDisplay.text = "";
            objectiveDisplay.text = "Objective: Act natural.";
            if (officialSpawned == false)
            {
                Official.SetActive(true);
                officialSpawned = true;
            }
        }
	}
}
