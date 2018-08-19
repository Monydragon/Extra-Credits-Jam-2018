using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Barrel : MonoBehaviour
{
    [Range(-20, 20)]
    float effectRadius, healthEffect, radEffect;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Explode()
    {
        var pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        if (Vector3.Distance(transform.position, pc.transform.position) <= effectRadius)
        {
            //Hurt player
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            if (Vector3.Distance(transform.position, e.transform.position) <= effectRadius)
            {
                //e.GetComponent<BullyController>()
            }
        }
    }

    /// <summary>
    /// Pass transform of the object that is throwing the barrel
    /// </summary>
    /// <param name="tr"></param>
    public void Throw(Transform tr)
    {

    }
}