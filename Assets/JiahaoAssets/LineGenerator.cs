using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LineGenerator : MonoBehaviour
{

    public GameObject linePrefab;

    Line activeLine;

    public float paintAmount;
    public TextMeshProUGUI paintAmountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (paintAmount > 0)
            {
                GameObject newLine = Instantiate(linePrefab);
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

        if(activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);
        }


    }
}
