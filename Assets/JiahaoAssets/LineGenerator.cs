using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private bool isScreenshot = false;

    private GameObject lineContainer;

    // Start is called before the first frame update
    void Start()
    {
        lineContainer = new GameObject("Line Container " + gameObject.name);
    }

    public void Screenshot()
    {
        string filename = string.Format("Assets/Resources/Screenshots/capture.png");
        if (!Directory.Exists("Assets/Resources/Screenshots"))
        {
            Directory.CreateDirectory("Assets/Resources/Screenshots");
        }
        TakeTransparentScreenshot(screenshotCamera, Screen.width, Screen.height, filename);
        foreach (var lineGenerator in FindObjectsOfType<LineGenerator>())
        {
            lineGenerator.ClearLines();
        }
    }

    public void ClearLines()
    {
        foreach (Transform tf in lineContainer.transform)
        {
            Destroy(tf.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //(Physics.Raycast(ray, out hit) && hit.collider.CompareTag("DrawCanvas"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (paintAmount > 0)
                {
                    GameObject newLine = Instantiate(linePrefab,lineContainer.transform);
                    activeLine = newLine.GetComponent<Line>();
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (paintAmount > 0)
                {
                    paintAmount -= Time.deltaTime * 10f;
                }
            }

            paintAmountText.text = Mathf.RoundToInt(paintAmount).ToString();

            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
            }

            if (activeLine != null)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                activeLine.UpdateLine(mousePos);
                activeLine.GetComponent<LineRenderer>().widthMultiplier = lineWidth;
            }

            lineWidth = lineWidthSlider.value * 10f;
        }
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
}
