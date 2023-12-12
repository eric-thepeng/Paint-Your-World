using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureHungerBar : MonoBehaviour
{
    public GameObject hungerBar;
    public GameObject hungerBarInstance;
    private Slider slider;
    private CreatureHunger creatureHunger;
    private Vector3 pos;
    public Canvas canvas;
    private float hungerBarOffset = 1f;

    private void Awake()
    {
        foreach(Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.gameObject.name == "CanvasHungerBar") canvas = c;

        }
        creatureHunger = GetComponent<CreatureHunger>();
        if (hungerBar == null)
        {
            hungerBar = Resources.Load<GameObject>("CreaturePrefabs/healthBar");
        }
        hungerBarInstance = Instantiate(hungerBar);
        hungerBarInstance.transform.SetParent(canvas.transform, false);
        pos = new Vector3(transform.position.x, transform.position.y + hungerBarOffset, transform.position.z);
        slider = hungerBarInstance.GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = creatureHunger.hungerMax;
        slider.value = creatureHunger.currentHunger;
    }


    // Update is called once per frame
    void Update()
    {
        HungerBarUpdate();
    }
    public void HungerBarUpdate()
    {
        float hunger = creatureHunger.currentHunger;
        pos = new Vector3(transform.position.x, transform.position.y + hungerBarOffset, transform.position.z);
        hungerBarInstance.transform.position = Camera.main.WorldToScreenPoint(pos);
        slider.value = creatureHunger.currentHunger;
        if(hunger <= 0)
        {
            Destroy(hungerBarInstance);
        }
    }
}
