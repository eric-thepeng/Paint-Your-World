using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableIdentifier", menuName = "ScriptableObjects/Placeable Identifier")]
public class PlaceableIdentifier : ScriptableObject
{
   public GameObject prefab;
   public string placeableName;
}
