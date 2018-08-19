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
        Chase = 1,
        Charge = 2,
        ThrowBarrel = 3,
    }

    public Status status;
    public EnemyState state;

    [FMODUnity.EventRef]
    //"stepsEvent" stores event path
    public string stepsEvent;

    //Event instance
    FMOD.Studio.EventInstance steps;

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

    public bool isControlDisabled = false;
    private bool isMoving = false;
    private Vector3 _MovementPath;

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
        if (state == EnemyState.Chase || state == EnemyState.Charge)
        {
            AiInput();
        }
    }

    public void AiInput()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var dist = Vector3.Distance(transform.position, player.transform.position);
        var dirDist = (player.transform.position - transform.position).normalized;

        Debug.DrawLine(transform.position, player.transform.position, Color.red, Mathf.Infinity);

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

    public void ApplyStatus(int health, int hunger, int rads)
    {
        status.Health += health;
        status.Hunger += hunger;
        status.Rads += rads;
    }

    IEnumerator Think()
    {
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
        var randomSec = Random.Range(5, 11);
        yield return new WaitForSeconds(randomSec);
    }

    IEnumerator Charge()
    {

        state = EnemyState.Charge;
        var oldSpeed = TileMove.Speed;
        var newSpeed = TileMove.Speed * 3;
        TileMove.Speed = 0;
        yield return new WaitForSeconds(1.5f);
        TileMove.Speed = newSpeed;
        yield return new WaitForSeconds(5);
        TileMove.Speed = oldSpeed;
    }

    IEnumerator ThrowBarrel()
    {
        state = EnemyState.ThrowBarrel;
        var barrels = GameObject.FindGameObjectsWithTag("Barrel");
        GameObject targetBarrel = null;
        float minDist = float.PositiveInfinity;

        if (!barrels.Any()) yield break;

        for (int i = 0; i < barrels.Length; i++)
        {
            var barrelDist = Vector3.Distance(transform.position, barrels[i].transform.position);
            if ( barrelDist < minDist)
            {
                minDist = barrelDist;
                targetBarrel = barrels[i];
            }
        }

        while (targetBarrel != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetBarrel.transform.position, 1);
            var barrelDist = Vector3.Distance(transform.position, targetBarrel.transform.position);
            if (barrelDist <= 2)
            {
                Debug.Log("Try to throw barrel");
            }
            yield return null;
        }

    }

    private void Start()
    {
        StartCoroutine(Think());
    }


}