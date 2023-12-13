using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;
using System;
using UnityEditor;
using UnityEngine.UI;

public class LineGenerator : MonoBehaviour
{

    public GameObject linePrefab;
    public GameObject ParentGameObject;
    private Line activeLine;

    public float paintAmount;
    public TextMeshProUGUI paintAmountText;
    public Camera screenshotCamera;
    //public PlaceableIdentifier targetPlaceableIdentifier;
    public GameObject entityParent;
    public float lineWidth;
    public Slider lineWidthSlider;

    private GameObject lineContainer;

    private List<GameObject> Lines = new List<GameObject>();
    private int currentLineIndex = 0;

    public Color LineColor1;
    public Color LineColor2;
    public byte brushAlpha = 255;
    public GameObject colorOptionHighlight;

    public int lineCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        lineContainer = new GameObject("Line Container " + gameObject.name);
        SetColorRed();
    }

    public void Screenshot()
    {
        if(lineCount == 0 ) return;
        string filename = string.Format("Assets/Resources/Screenshots/capture.png");
        if (!Directory.Exists("Assets/Resources/Screenshots"))
        {
            Directory.CreateDirectory("Assets/Resources/Screenshots");
        }
        TakeTransparentScreenshot(screenshotCamera, Screen.width, Screen.height, filename);
        ClearAllDrawings();
    }

    public void SetColorRed()
    {
        LineColor1 = new Color32(220, 119, 89, brushAlpha); ;
        LineColor2 = new Color32(220, 119, 89, brushAlpha);
    }

    public void SetColorBlue()
    {
        LineColor1 = new Color32(36, 215, 255, brushAlpha); 
        LineColor2 = new Color32(0, 80, 219, brushAlpha);
    }

    public void SetColorGreen()
    {
        LineColor1 = new Color32(118, 255, 38, brushAlpha);
        LineColor2 = new Color32(0, 209, 70, brushAlpha);
    }

    public void SetColorYellow()
    {
        LineColor1 = new Color32(220, 255, 46, brushAlpha);
        LineColor2 = new Color32(217, 90, 0, brushAlpha);
    }

    public void SetColorOptionHighlightPosition(Transform _transform)
    {
        if(Vector2.Distance(colorOptionHighlight.transform.position,_transform.position)<=0.1f)
        {
            colorOptionHighlight.SetActive(!colorOptionHighlight.activeSelf); // Toggle the active status
        }
        colorOptionHighlight.transform.position = _transform.position;
        
    }



    public void UndoLine()
    {
        if (currentLineIndex - 1 >= 0)
        {
            currentLineIndex -= 1;
            Lines[currentLineIndex].GetComponent<LineRenderer>().enabled = false;
        }

    }

    public void RedoLine()
    {
        if (currentLineIndex + 1 <= Lines.Count) {
            
            Lines[currentLineIndex].GetComponent<LineRenderer>().enabled = true;
            currentLineIndex += 1;
        }
    }


    public void ClearLines()
    {
        foreach (Transform tf in lineContainer.transform)
        {
            Destroy(tf.gameObject);
        }
    }


    void Update()
    {
        // Raycast to check if the mouse is over a 2D sprite with the tag "DrawCanvas"
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        Debug.Log(currentLineIndex);
        if (Input.GetMouseButtonDown(0) && hit.collider != null && hit.collider.CompareTag("DrawCanvas"))
        {
            if (paintAmount > 0)
            {
                GameObject newLine = Instantiate(linePrefab, lineContainer.transform);
                newLine.GetComponent<LineRenderer>().startColor = LineColor1;
                newLine.GetComponent<LineRenderer>().endColor = LineColor2;
                Lines.Add(newLine);
                currentLineIndex += 1;
                if(currentLineIndex != Lines.Count)
                {
                    for (int i = 0; i < Lines.Count; i++)
                    {
                        if(i > currentLineIndex)
                        {
                            Lines.Remove(Lines[i]);
                        }
                    }
                    currentLineIndex = Lines.Count;
                    Debug.Log("Memory cleared");
                }
                
                activeLine = newLine.GetComponent<Line>();
            }
        }

        if (Input.GetMouseButton(0) && activeLine != null)
        {
            if (!hit.collider.CompareTag("DrawCanvas"))
            {
                activeLine = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);
            activeLine.GetComponent<LineRenderer>().widthMultiplier = lineWidth;
            lineCount++;
        }

        lineWidth = lineWidthSlider.value * 10f;
    }



    public void TakeTransparentScreenshot(Camera cam, int width, int height, string savePath)
    {

        // Depending on your render pipeline, this may not work.
        var bak_cam_targetTexture = cam.targetTexture;
        var bak_cam_clearFlags = cam.clearFlags;
        var bak_RenderTexture_active = RenderTexture.active;

        var tex_transparent = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Must use 24-bit depth buffer to be able to fill background.
        var render_texture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
        var grab_area = new Rect(0, 0, width, height);

        RenderTexture.active = render_texture;
        cam.targetTexture = render_texture;
        cam.clearFlags = CameraClearFlags.SolidColor;

        // Simple: use a clear background
        cam.backgroundColor = Color.clear;
        cam.Render();
        tex_transparent.ReadPixels(grab_area, 0, 0);
        tex_transparent.Apply();
        
        // Encode the resulting output texture to a byte array then write to the file
        //byte[] pngShot = ImageConversion.EncodeToPNG(tex_transparent);
        byte[] pngShot = tex_transparent.EncodeToPNG();
        File.WriteAllBytes(savePath, pngShot);

        cam.clearFlags = bak_cam_clearFlags;
        cam.targetTexture = bak_cam_targetTexture;
        RenderTexture.active = bak_RenderTexture_active;
        RenderTexture.ReleaseTemporary(render_texture);

        Debug.Log("Texture Width: " + tex_transparent.width);
        Debug.Log("Texture Height: " + tex_transparent.height);
        Debug.Log("Color at (0, 0): " + tex_transparent.GetPixel(0, 0));

        ImageConversion.LoadImage(tex_transparent, pngShot);
        tex_transparent.Apply();

        Sprite spr = Sprite.Create(
            tex_transparent,
            new Rect(0.0f, 0.0f, tex_transparent.width, tex_transparent.height),
            new Vector2(0.5f, 0.5f), // Set the pivot to (0.5, 0.5) for center
            100.0f
        );

        // Assign and Adjust Placeable Identifier
        PlaceableIdentifier targetPlaceableIdentifier = InventoryManager.i.GetSelectedPlaceableIdentifier();
        targetPlaceableIdentifier.prefab.GetComponent<SpriteRenderer>().sprite = spr;
        targetPlaceableIdentifier.prefab.GetComponent<SpriteRenderer>().sharedMaterial.mainTexture = tex_transparent;
        InventoryManager.i.FinishPaintingCurrent();

        /*
        foreach (Transform child in entityParent.transform)
        {
            if (child.gameObject.CompareTag("Sheep"))
            {
                child.GetComponent<SpriteRenderer>().sprite = spr;
                Debug.Log("Sprite replacement done");
            }

        }*/

        //Texture2D.Destroy(tex_transparent);


        //AssetDatabase.Refresh();
    }

    public void ClearAllDrawings()
    {
        foreach (var lineGenerator in FindObjectsOfType<LineGenerator>())
        {
            lineGenerator.ClearLines();
        }

        lineCount = 0;
    }
}
