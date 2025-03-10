using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Invoke("OnDisable", 1f);
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    public void StartExplosion(string target)
    {
        //anim.SetTrigger("onExplosion");
        switch(target)
        {
            case "enemyA":
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "enemyB":
            case "player":
                transform.localScale = Vector3.one * 1f;
                break;
            case "enemyC":
                transform.localScale = Vector3.one * 2f;
                break;
            case "boss":
                transform.localScale = Vector3.one * 3f;
                break;
        }
    }
}
