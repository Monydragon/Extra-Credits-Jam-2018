using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Barrel : MonoBehaviour
{
    [Range(-20, 20)]
    public float effectRadius, healthEffect, radEffect;
    Rigidbody2D rb;
    bool thrown;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        Destroy(gameObject);
    }

    /// <summary>
    /// Pass transform of the object that is throwing the barrel
    /// </summary>
    /// <param name="tr"></param>
    public void Throw(Vector3 pos, float spd)
    {
        if (thrown)
            return;

        StartCoroutine(Thrown(pos, spd));
        thrown = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (thrown)
            Explode();
    }

    IEnumerator Thrown(Vector3 pos, float spd)
    {
        var dir = -(pos - transform.position).normalized;
        while (true)
        {
            rb.MovePosition(transform.position + dir * spd * Time.deltaTime);
            yield return null;
        }
    }
}