using UnityEngine;
using TMPro;

public class TextFade : MonoBehaviour
{
    [SerializeField] private float fadeLowerLimit = 0;
    [SerializeField] private float fadeUpperLimit = 1;
    [SerializeField] private float fadeSpeed = 0.1f;
    private TextMeshProUGUI targetText;
    private int fadeDirection = -1;
    private Color textColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetText = GetComponent<TextMeshProUGUI>();
        textColor = targetText.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetText.color.a <= fadeLowerLimit)
        {
            fadeDirection = 1;
        }
        else if (targetText.color.a >= fadeUpperLimit)
        {
            fadeDirection = -1;
        }

        textColor.a += fadeSpeed * fadeDirection;
        targetText.color = textColor;
    }
}
