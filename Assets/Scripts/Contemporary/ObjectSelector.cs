using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color selectionColor = new Color(0.38f, 0.38f, 0.38f, 1f);
    private Color originalColor;
    public bool isSelected = false;
    public bool greenPermanently = false;

    public bool getIsSelected() {
        return isSelected;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Update()
    {
        if(!greenPermanently) {
            if (isSelected)
            {
                sr.color = selectionColor;
            }
            else
            {
                sr.color = originalColor;
            }
        }
    }

    void OnMouseDown()
    {
        isSelected = !isSelected;
    }
}
