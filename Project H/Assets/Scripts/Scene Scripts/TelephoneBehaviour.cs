using UnityEngine;
using System.Collections;

public class TelephoneBehaviour : MonoBehaviour {

    HideWaveScript WaveScript;

    void Awake()
    {
        WaveScript = GameObject.Find("LevelManager").GetComponent<HideWaveScript>();
    }

    void OnMouseDown()
    {
        if (WaveScript.hideTime > 0)
        {
            EndTheWave();
        }
    }
    public void EndTheWave()
    {
        
            WaveScript.hideTime = 0;
        
    }
}
