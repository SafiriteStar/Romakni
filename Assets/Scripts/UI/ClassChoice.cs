using UnityEngine;
using UnityEngine.UI;

public class ClassChoice : MonoBehaviour
{
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private GameObject targetClassPrefab;
    private Image background;
    private bool selected = false;

    public GameObject GetTargetClass() {  return targetClassPrefab; }

    public void SetSelected(bool selected)
    {
        if (background == null)
        {
            background = GetComponent<Image>();
        }

        this.selected = selected;
        if (selected)
        {
            background.color = selectedColor;
        }
        else
        {
            background.color = unselectedColor;
        }
    }
}
