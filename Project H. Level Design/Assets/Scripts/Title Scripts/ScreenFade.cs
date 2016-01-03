using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class ScreenFade : MonoBehaviour
{
    private Texture2D pixel;
    public Color color = Color.black;
    public float startAlpha = 0.0f;
    public float maxAlpha = 1f;
    public float rampUpTime = 1.0f;
    private float holdTime = 3f; //this is private because it should be equal to respawnTime.
    public float rampDownTime = 0.1f;

    enum FLASHSTATE { OFF, UP, HOLD, DOWN }
    TimerDeath timer;
    FLASHSTATE state = FLASHSTATE.OFF;

    public int fadeState = 10;

    // Use this for initialization
    void Start()
    {
        pixel = new Texture2D(1, 1);
        color.a = startAlpha;
        pixel.SetPixel(0, 0, color);
        pixel.Apply();
    }

    public void Update()
    {
        fadeState = (int)state;
        switch (state)
        {
            case FLASHSTATE.UP:
                if (timer.UpdateAndTest())
                {
                    state = FLASHSTATE.HOLD;
                    timer = new TimerDeath(holdTime);
                }
                break;
            case FLASHSTATE.HOLD:
                if (timer.UpdateAndTest())
                {
                    state = FLASHSTATE.DOWN;
                    timer = new TimerDeath(rampDownTime);
                }
                break;
            case FLASHSTATE.DOWN:
                if (timer.UpdateAndTest())
                {
                    state = FLASHSTATE.OFF;
                    timer = null;
                }
                break;
        }
    }

    private void SetPixelAlpha(float a)
    {
        color.a = a;
        pixel.SetPixel(0, 0, color);
        pixel.Apply();
    }

    public void OnGUI()
    {

        switch (state)
        {
            case FLASHSTATE.UP:
                SetPixelAlpha(Mathf.Lerp(startAlpha, maxAlpha, timer.Elapsed));
                break;
            case FLASHSTATE.DOWN:
                SetPixelAlpha(Mathf.Lerp(maxAlpha, startAlpha, timer.Elapsed));
                break;
        }
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pixel);
    }

    public void FadeToBlack()
    {
        timer = new TimerDeath(rampUpTime);
        state = FLASHSTATE.UP;
    }

}

public class TimerDeath
{
    float _timeElapsed;
    float _totalTime;

    public TimerDeath(float timeToCountInSec)
    {
        _totalTime = timeToCountInSec;
    }

    public bool UpdateAndTest()
    {
        _timeElapsed += Time.deltaTime;
        return _timeElapsed >= _totalTime;
    }

    public float Elapsed
    {
        get { return Mathf.Clamp(_timeElapsed / _totalTime, 0, 1); }
    }
}
