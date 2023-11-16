using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHunger : MonoBehaviour
{
    [SerializeField] private float hungerMax = 15f;
    [SerializeField] private float currentHunger;
    [SerializeField] private float hungerDownTime = 10f;
    private CreatureDestructible destruct;

    private void Awake()
    {
        currentHunger= hungerMax;
        destruct = GetComponent<CreatureDestructible>();
    }
    private void Start()
    {
        StartCoroutine(HungerGoDown());
    }
    public void HungerGoUp(float hungerIncrease)
    {
        currentHunger+= hungerIncrease;
        if(currentHunger> hungerMax)
        {
            currentHunger= hungerMax;
        }
    }
    private IEnumerator HungerGoDown()
    {
        while(currentHunger > 0)
        {
            yield return new WaitForSeconds(hungerDownTime);
            currentHunger--;
        }
        if(currentHunger <= 0) {
            destruct.DestroyObject();
        }
    }
}
