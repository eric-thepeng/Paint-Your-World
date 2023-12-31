using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHunger : MonoBehaviour
{
    public float hungerMax = 1000f;
    public float currentHunger;
    [SerializeField] private float hungerDownTime = 10f;
    private CreatureDestructible destruct;
    private CreatureAnimator creatureAnimator;
  
   




    private void Awake()
    {
        currentHunger= hungerMax;
        destruct = GetComponent<CreatureDestructible>();
        creatureAnimator = GetComponent<CreatureAnimator>();
       
    }
    private void Start()

    {
        
        StartCoroutine(HungerGoDown());
    }
    public void HungerGoUp(float hungerIncrease)
    {
        creatureAnimator.StartCoroutine(creatureAnimator.AnimatingCoroutine(creatureAnimator.Eat()));
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
            currentHunger-= 0.1f;
        }
        if(currentHunger <= 0) {
            destruct.DestroyObject();
        }
    }
   

        
    
}
