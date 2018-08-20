using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FMODUnity;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Barrel : MonoBehaviour
{
    [Range(-20, 20)]
    public float effectRadius;
    [Range(-20, 20)]
    public int healthEffect, radEffect;

    [EventRef]
    public string throwSfx, bounceSfx, explodeSfx;

    Transform playerTr;
    Rigidbody2D rb;
    Vector3 dir;
    int bounces;
    bool thrown, playerCanThrow, exploded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Explode()
    {
        if (exploded)
            return;
        exploded = true;

        var pc = playerTr.GetComponent<PlayerControl>();
        if (Vector3.Distance(transform.position, pc.transform.position) <= effectRadius)
            pc.ApplyStatus(healthEffect, radEffect);

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
            if (Vector3.Distance(transform.position, e.transform.position) <= effectRadius)
                e.GetComponent<BullyController>().ApplyStatus(healthEffect, radEffect);

        var barrels = GameObject.FindGameObjectsWithTag("Barrel");
        foreach (var b in barrels)
            if (Vector3.Distance(transform.position, b.transform.position) <= effectRadius)
                b.GetComponent<Barrel>().Explode();

        RuntimeManager.PlayOneShot(explodeSfx, transform.position);
        Destroy(gameObject);
        RuntimeManager.PlayOneShot(explodeSfx, transform.position);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;
        transform.GetChild(0).gameObject.SetActive(true);
        Destroy(gameObject, 3);
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
        RuntimeManager.PlayOneShot(throwSfx, transform.position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (thrown)
        {
            if (collision.transform.tag == "Player" || collision.transform.tag == "Enemy" || bounces > 3)
                Explode();
            else
            {
                dir = dir - (Vector3)collision.contacts[0].normal * 2 * Vector3.Dot(dir, collision.contacts[0].normal);
                bounces++;
                RuntimeManager.PlayOneShot(bounceSfx, transform.position);
            }
        }

        else if (collision.transform.tag == "Player")
        {
            playerCanThrow = true;
        }
    }

    void Update()
    {
        if (playerCanThrow && Input.GetKeyDown(KeyCode.E))
            Throw((transform.position - playerTr.position).normalized, 10);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
            playerCanThrow = false;
    }

    IEnumerator Thrown(float spd)
    {
        float time = 0;
        while (true)
        {
            rb.MovePosition(transform.position + dir * spd * Time.deltaTime);

            time += Time.deltaTime;
            if (time > 10)
                Destroy(gameObject);

            yield return null;
        }
    }
}