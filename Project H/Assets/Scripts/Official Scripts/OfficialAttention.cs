using UnityEngine;
using UnityEngine.UI;
public class OfficialAttention : MonoBehaviour
{

    public float officialAttention;
    public Text attentionText;
    public Slider attentionSlider;
    void Start()
    {

        attentionText.text = "Attention";

        attentionSlider.wholeNumbers = false;
        attentionSlider.maxValue = 100;
        attentionSlider.minValue = 0f;
        attentionSlider.interactable = false;

        attentionSlider.value = officialAttention;
    }


    void Update()
    {
        attentionSlider.value = officialAttention;

        if (Input.GetKeyDown(KeyCode.I)) //TODO: debug, remove
        {
            IncrementAttention(25f);
        }
    }

    public float getAttentionValue()
    {
        return officialAttention;
    }

    public void IncrementAttention(float _value)
    {
        if (officialAttention + _value >= attentionSlider.maxValue)
        {
            officialAttention = attentionSlider.maxValue;
        }
        else
        {
            officialAttention += _value;
        }
    }
}
