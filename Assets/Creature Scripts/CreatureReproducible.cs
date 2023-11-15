using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureReproducible : MonoBehaviour
{
    [SerializeField] private float babyCooldownTime = 10f;
    private bool canMakeBaby = false;
    [SerializeField] GameObject babyPrefab;
    public bool justSpawnedBaby;

    private void Start()
    {
        StartCoroutine(BabyCooldown());
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
}
