using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepStageTrigger : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        transform.parent.GetComponent<WorldPlane>().EnterBuilding();
    }
}
