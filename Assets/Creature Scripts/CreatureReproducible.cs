using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureReproducible : MonoBehaviour
{
    [SerializeField] private float babyCooldownTime = 20f;
    private bool canMakeBaby = false;
    public string myMate;
    [SerializeField] GameObject babyPrefab;
    public bool justSpawnedBaby;
    public CreatureManager creatureManager;

    private void Start()
    {
        StartCoroutine(BabyCooldown());
        creatureManager = GetCreatureManager();
    }

    public CreatureManager GetCreatureManager()
    {
        return GetComponent<CreatureController>().creatureMan;
    }
    
    public void MakeBaby()
    {
        if (canMakeBaby)
        {
            var baby = Instantiate(babyPrefab);
            canMakeBaby = false;
            justSpawnedBaby= true;

            StartCoroutine(BabyCooldown());
        }
    }
    private IEnumerator BabyCooldown()
    {
        yield return new WaitForSeconds(babyCooldownTime);
        canMakeBaby= true;
        justSpawnedBaby= false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(myMate))
        {
            if(!justSpawnedBaby)
            {
                collision.gameObject.GetComponent<CreatureReproducible>().justSpawnedBaby = true;
                MakeBaby();
                justSpawnedBaby = true;
            }
        }
    }
}
