using UnityEngine;
using UnityEngine.UI;
public class OfficialAttention : MonoBehaviour
{

    private float officialAttention;
    public Text attentionText;
    public Slider attentionSlider;
    void Start()
    {
        officialAttention = 15f;

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
    }

    public float getAttentionValue()
    {
        return officialAttention;
    }

    public void IncrementAttention(float _value)
    {
        officialAttention += _value;
    }
}
