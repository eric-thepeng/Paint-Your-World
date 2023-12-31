using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneStartButton : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        transform.parent.GetComponent<WorldPlane>().BuildFinished();
        AudioManager.instance.Play("Planet Generate");
        gameObject.SetActive(false);
    }
}
