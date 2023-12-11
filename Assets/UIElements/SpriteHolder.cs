using UnityEngine;
using UnityEngine.UI;

public class SpriteHolder : MonoBehaviour
{
    public Sprite originalSprite;
    public Sprite alternateSprite;
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        // Set the original sprite initially
        image.sprite = originalSprite;
    }

    public void ChangeToAlternateSprite()
    {
        image.sprite = alternateSprite;
    }

    public void ResetToOriginalSprite()
    {
        image.sprite = originalSprite;
    }
}
