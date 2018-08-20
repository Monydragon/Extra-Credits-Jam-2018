using System.Collections;
using FMODUnity;
using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for handling player movement and player interaction controls. Anything that involves input access can be invoked/handled through here.
/// </summary>
public partial class PlayerControl : BaseMovementController
{
    public static PlayerControl instance;

    public Status status;

    public GameObject statusPanel;

    private Transform healthUI;
    private Transform radsUI;

    //FMOD
    //UI
    [EventRef]
    //"stepsEvent" stores event path
    public string stepsEvent, hurtSfx, dieSfx;

    //Event instance
    FMOD.Studio.EventInstance steps;

    //TILE VARIABLE HERE (substitute number)
    int tile = 1;

    private void OnEnable()
    {
        EventManager.OnSetPlayerControl += SetPlayerControl;

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
        EventManager.OnSetPlayerControl -= SetPlayerControl;

        //FMOD
        //Releases "steps" resources
        steps.release();
    }

    public bool isControlDisabled = false;
    private bool isMoving = false;
    private Vector3 _MovementPath;

    protected override void Awake()
    {
        base.Awake();
        if (statusPanel != null)
        {
            healthUI = statusPanel.transform.GetChild(0);
            radsUI = statusPanel.transform.GetChild(1);
            UpdateStatusUI();
        }
    }

    private void HandleAnimation()
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
        HandleInput();
        UpdateStatusUI();
    }

    protected void HandleInput()
    {
        if (!isControlDisabled)
        {
            if (!Input.anyKey)
                _Rb.velocity = Vector2.zero;

            if (TileMove.SmoothMovement)
            {
                //TODO: Replace movement control with InContol handling?
                _MovementPath.x = Input.GetAxisRaw("Horizontal");
                _MovementPath.y = Input.GetAxisRaw("Vertical");

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
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.DownArrow)) { _TileMove.Move(transform, MoveDirection.UP, _TileMove.Speed); }
                if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.LeftArrow)) { _TileMove.Move(transform, MoveDirection.RIGHT, _TileMove.Speed); }
                if (Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.UpArrow)) { _TileMove.Move(transform, MoveDirection.DOWN, _TileMove.Speed); }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.RightArrow)) { _TileMove.Move(transform, MoveDirection.LEFT, _TileMove.Speed); }

                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.DownArrow)) { _TileMove.Move(transform, MoveDirection.UP_LEFT, _TileMove.Speed); }
                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.DownArrow)) { _TileMove.Move(transform, MoveDirection.UP_RIGHT, _TileMove.Speed); }
                if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.UpArrow)) { _TileMove.Move(transform, MoveDirection.DOWN_RIGHT, _TileMove.Speed); }
                if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.UpArrow)) { _TileMove.Move(transform, MoveDirection.DOWN_LEFT, _TileMove.Speed); }
            }

            if (Input.GetKeyDown(KeyCode.E) ||
                Input.GetKeyDown(KeyCode.KeypadEnter) ||
                Input.GetKeyDown(KeyCode.Return))
            {
                var interactedObject = _TileMove.InteractedGameObject();
                if (interactedObject != null)
                {
                    EventManager.ObjectInteract(gameObject, interactedObject);
                }
            }

            //REFACTOR: CLean this up
            if (Input.GetKeyDown(KeyCode.Alpha1)) { UseItem(Inventory.Inv.UseItem(0)); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { UseItem(Inventory.Inv.UseItem(1)); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { UseItem(Inventory.Inv.UseItem(2)); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { UseItem(Inventory.Inv.UseItem(3)); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { UseItem(Inventory.Inv.UseItem(4)); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { UseItem(Inventory.Inv.UseItem(5)); }
            if (Input.GetKeyDown(KeyCode.Alpha7)) { UseItem(Inventory.Inv.UseItem(6)); }
            if (Input.GetKeyDown(KeyCode.Alpha8)) { UseItem(Inventory.Inv.UseItem(7)); }
            if (Input.GetKeyDown(KeyCode.Alpha9)) { UseItem(Inventory.Inv.UseItem(8)); }
            if (Input.GetKeyDown(KeyCode.Alpha0)) { UseItem(Inventory.Inv.UseItem(9)); }
        }
    }

    public void UseItem(Item item)
    {
        if (item != null)
        {
            switch (item.itemID)
            {
                //                case (int)ItemItems.Carrot:
                //                    Debug.Log("THIS IS A CAROT");
                //                    break;
                //                case (int)ItemItems.GoodBread:
                //                    Debug.Log("THIS IS A GOOD BREAD");
                //                    break;
                //                case (int)ItemItems.Pudding:
                //                    Debug.Log("THIS IS A PUDDING");
                //                    break;
                default:
                    status.UseItem(item);
                    RuntimeManager.PlayOneShot(item.useSfx, transform.position);
                    UpdateStatusUI();
                    break;
            }
        }

    }

    public void ApplyStatus(int health, int rads)
    {
        status.Health += health;
        status.Rads += rads;
        RuntimeManager.PlayOneShot(hurtSfx, transform.position);

        if (health <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        RuntimeManager.PlayOneShot(dieSfx, transform.position);
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScene");
    }

    public void UpdateStatusUI()
    {

        if (status.Health <= 100 && status.Health >= 0)
        {
            healthUI.GetChild(0).GetComponent<Image>().fillAmount = status.Health / 100;
            healthUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = status.Health.ToString();
        }

        if (status.Rads >= 0)
        {
            radsUI.GetChild(0).GetComponent<Image>().fillAmount = status.Rads / 100;
            radsUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = status.Rads.ToString();
        }
    }

    public void SetPlayerControl(bool value) => isControlDisabled = !value;

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.transform.tag == "Barrel")
            StartCoroutine(BarrelHit());
    }

    IEnumerator BarrelHit()
    {
        isControlDisabled = true;
        yield return new WaitForSeconds(0.4f);
        _Rb.velocity = Vector2.zero;
        isControlDisabled = false;
    }
}