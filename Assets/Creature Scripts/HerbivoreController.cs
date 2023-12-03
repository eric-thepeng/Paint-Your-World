using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbivoreController : MonoBehaviour
{
    private CreatureController myController;
    private string myFoodType = "Grass";
    public enum HerbivoreTypes
    {
        Sheep
    }

    private void Awake()
    {
        myController= GetComponent<CreatureController>();

    }
    private void Start()
    {
        CreatureManager.Instance.Herbivores.Add(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //using tags is stupid change to smth else
        if (collision.gameObject.CompareTag(myFoodType))
        {
            Debug.Log("eat");
            myController.creatureHunger.HungerGoUp(100f);
            collision.gameObject.SetActive(false);
            StartCoroutine(ReloadGrass(collision.gameObject));
            myController.creatureMovement.StartCoroutine(myController.creatureMovement.CreatureEating());
        }
    }
    private void OnDestroy()
    {
        CreatureManager.Instance.Herbivores.Remove(this.gameObject);
    }
    private IEnumerator ReloadGrass(GameObject grass)
    {
        yield return new WaitForSeconds(5);
        grass.SetActive(true);
    }

}
