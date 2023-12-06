using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class CreatureAnimator : MonoBehaviour
{
    private float ogScaleX;
    private float ogScaleY;
    public bool animating;
    private float animateSpeed = 0.3f;

    private void Start()
    {
        ogScaleX = transform.localScale.x;
        ogScaleY = transform.localScale.y;
    }
    public IEnumerator Eat()
    {
        while(animating)
        {
            transform.DOScaleX(ogScaleX * 1.2f, animateSpeed);
            transform.DOScaleY(ogScaleY/1.1f, animateSpeed);
            yield return new WaitForSeconds(animateSpeed);
            transform.DOScaleX(ogScaleX, animateSpeed);
            transform.DOScaleY(ogScaleY, animateSpeed);
            yield return new WaitForSeconds(animateSpeed);
        }
    }
    public IEnumerator Move()
    {
        while(animating)
        {
            transform.DOScaleX(ogScaleX * 1.2f, animateSpeed);
            yield return new WaitForSeconds(animateSpeed);
            transform.DOScaleX(ogScaleX, animateSpeed);
            yield return new WaitForSeconds(animateSpeed);
        }
    }
    public IEnumerator AnimatingCoroutine(IEnumerator e)
    {
        animating= true;
        StartCoroutine(e);
        yield return new WaitForSeconds(3f);
        animating= false;
        StopCoroutine(e);
    }
}
