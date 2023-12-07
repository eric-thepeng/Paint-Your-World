using UnityEngine;
using UnityEngine.UI;

public class OutlineEffect : MonoBehaviour
{
    void Start()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            Outline outline = image.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.black; // Set the color of the outline
            outline.effectDistance = new Vector2(1, -1); // Set the distance of the outline effect
            outline.useGraphicAlpha = true; // Use alpha of the graphic
        }
    }
}
