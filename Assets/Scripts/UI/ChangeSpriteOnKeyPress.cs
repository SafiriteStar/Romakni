using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteOnKeyPress : MonoBehaviour
{
    [SerializeField] private Image[] baseKeyImages;
    [SerializeField] private Sprite[] releasedKeySprites;
    [SerializeField] private Sprite[] pressedKeySprites;
    [SerializeField] private KeyCode[] keyCodes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < baseKeyImages.Length; i++)
        {
            baseKeyImages[i].sprite = releasedKeySprites[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
            {
                baseKeyImages[i].sprite = pressedKeySprites[i];
            }
            else
            {
                baseKeyImages[i].sprite = releasedKeySprites[i];
            }
        }
    }
}
