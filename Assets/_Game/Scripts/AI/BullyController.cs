using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine;

public class BullyController : BaseMovementController
{
    public enum EnemyState
    {
        Idle = 0,
        Think = 1,
        Chase = 2,
        Charge = 3,
        ThrowBarrel = 4,
    }

    public float barrelDistThreshold = 1.5f, throwSpeed = 10f, chargePause = 1f, chargeDuration = 1.5f, chargeSpdMult = 2;
    public int damage = 2;

    public Status status;
    public EnemyState state;
    public Transform player;

    [FMODUnity.EventRef]
    //"stepsEvent" stores event path
    public string stepsEvent;

    //Event instance
    FMOD.Studio.EventInstance steps;

    public bool isControlDisabled = false;
    private bool isMoving = false, canHurt = true;
    private Vector3 _MovementPath;

    private void OnEnable()
    {
        //FMOD
        //Instances "steps" and enables it
        steps = FMODUnity.RuntimeManager.CreateInstance(stepsEvent);
        steps.start();
        //Attaches instance to object
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(steps, GetComponent<Transform>(), GetComponent<Rigidbody>());

        //starts "speed" as 0
        steps.setParameterValue("speed", 0);
        //starts "tile" as 1 (wood)
        steps.setParameterValue("tile", 1);
    }

    private void OnDisable()
    {
        //FMOD
        //Releases "steps" resources
        steps.release();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Think());
    }

    void HandleAnimation()
    {
        if (_Animator != null)
        {
            if (isControlDisabled) { _MovementPath = Vector3.zero; isMoving = false; }

            _Animator.SetBool("moving", isMoving);

            if (isMoving)
            {
                _Animator.SetFloat("input_x", _MovementPath.x);
                _Animator.SetFloat("input_y", _MovementPath.y);
            }
        }
    }

    protected override void Move()
    {
        _TileMove.CheckArea(transform);
        HandleAnimation();
        if (state == EnemyState.Chase)
        {
            AiInput();
        }
    }

    public void AiInput()
    {
        var dist = Vector3.Distance(transform.position, player.transform.position);
        var dirDist = (player.transform.position - transform.position).normalized;

        //Debug.DrawLine(transform.position, player.transform.position, Color.red, Mathf.Infinity);

        if (dist > 0)
        {
            _MovementPath.x = dirDist.x;
            _MovementPath.y = dirDist.y;
        }

        isMoving = (_MovementPath != Vector3.zero) ? true : false;


        if (isMoving)
        {
            if (_MovementPath == Vector3.right) { _TileMove._FacingDirection = MoveDirection.RIGHT; }
            if (_MovementPath == Vector3.left) { _TileMove._FacingDirection = MoveDirection.LEFT; }
            if (_MovementPath == Vector3.up) { _TileMove._FacingDirection = MoveDirection.UP; }
            if (_MovementPath == Vector3.down) { _TileMove._FacingDirection = MoveDirection.DOWN; }
            transform.position += _MovementPath * Time.deltaTime * _TileMove.Speed;

            //FMOD
            //if player is moving change speed parameter to hear sound
            steps.setParameterValue("speed", 80);
        }

        if (!isMoving)
        {
            //FMOD
            //if player is not moving change speed to 0 so no sound is heard
            steps.setParameterValue("speed", 0);
        }
    }

    void LateUpdate()
    {
        _Rb.velocity = Vector2.zero;
    }

    public void ApplyStatus(int health, int rads)
    {
        status.Health += health;
        status.Rads += rads;
    }

    IEnumerator Think()
    {
        state = EnemyState.Think;
        while (true)
        {
            int randomVal = Random.Range(0, 3);

            switch (randomVal)
            {
                case 0: yield return Chase(); break;
                case 1: yield return Charge(); break;
                case 2: yield return ThrowBarrel(); break;
                default: break;
            }

        }
    }

    IEnumerator Chase()
    {
        state = EnemyState.Chase;
        var randomSec = Random.Range(3, 5);
        yield return new WaitForSeconds(randomSec);
    }

    IEnumerator Charge()
    {
        state = EnemyState.Charge;
        isMoving = false;
        HandleAnimation();
        yield return new WaitForSeconds(chargePause);

        float t = 0;
        Vector3 dir = (player.transform.position - transform.position).normalized;
        _MovementPath = dir;
        isMoving = true;
        HandleAnimation();
        while (t < chargeDuration)
        {
            transform.position += dir * TileMove.Speed * chargeSpdMult * Time.deltaTime;
            t += Time.deltaTime;

            //If we hit something stop
            if (TileMove.Up != null && TileMove.Up.tag != "Item" || TileMove.Right != null && TileMove.Right.tag != "Item" || TileMove.Down != null && TileMove.Down.tag != "Item" || TileMove.Left != null && TileMove.Left.tag != "Item" ||
                TileMove.UpRight != null && TileMove.UpRight.tag != "Item" || TileMove.UpLeft != null && TileMove.UpLeft.tag != "Item" || TileMove.DownRight != null && TileMove.DownRight.tag != "Item")
                break;
            yield return null;
        }

        isMoving = false;
        HandleAnimation();
        yield return new WaitForSeconds(chargePause);
        isMoving = true;
        HandleAnimation();
    }

    IEnumerator ThrowBarrel()
    {
        state = EnemyState.ThrowBarrel;
        var barrels = GameObject.FindGameObjectsWithTag("Barrel");
        GameObject targetBarrel = null;
        float minDist = float.PositiveInfinity;

        if (!barrels.Any()) yield break;

        //Find nearest barrel
        for (int i = 0; i < barrels.Length; i++)
        {
            var barrelDist = Vector3.Distance(transform.position, barrels[i].transform.position);
            if (barrelDist < minDist)
            {
                minDist = barrelDist;
                targetBarrel = barrels[i];
            }
        }

        isMoving = true;
        _MovementPath = (targetBarrel.transform.position - transform.position).normalized;
        HandleAnimation();

        while (targetBarrel != null)
        {
            transform.position += (targetBarrel.transform.position - transform.position).normalized * Time.deltaTime * TileMove.Speed;
            if (Vector3.Distance(transform.position, targetBarrel.transform.position) <= barrelDistThreshold)
            {
                GetComponent<Collider2D>().enabled = false;

                targetBarrel.GetComponent<Barrel>().Throw((player.position - transform.position).normalized, throwSpeed);
                yield return new WaitForSeconds(0.3f);

                GetComponent<Collider2D>().enabled = true;
                yield break;
            }

            yield return null;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (canHurt && collision.transform.tag == "Player")
            StartCoroutine(DealDamage(collision.gameObject, state == EnemyState.Charge ? damage * 2 : damage));
    }

    IEnumerator DealDamage(GameObject obj, int amount)
    {
        canHurt = false;
        obj.GetComponent<PlayerControl>().ApplyStatus(-amount, 0);
        yield return new WaitForSeconds(2f);
        canHurt = true;
    }
}