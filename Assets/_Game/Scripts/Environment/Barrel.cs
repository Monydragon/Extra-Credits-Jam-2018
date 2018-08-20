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
    Vector3 dir;
    int bounces;
    bool thrown;

    void Awake()
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
    public void Throw(Vector3 dir, float spd)
    {
        if (thrown)
            return;

        this.dir = dir;
        StartCoroutine(Thrown(spd));
        thrown = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (thrown && (collision.transform.tag == "Player" || collision.transform.tag == "Enemy" || bounces > 3))
            Explode();
        else
        {
            dir = dir - (Vector3)collision.contacts[0].normal * 2 * Vector3.Dot(dir, collision.contacts[0].normal);
            bounces++;
        }
    }

    IEnumerator Thrown(float spd)
    {
        while (true)
        {
            rb.MovePosition(transform.position + dir * spd * Time.deltaTime);
            yield return null;
        }
    }
}