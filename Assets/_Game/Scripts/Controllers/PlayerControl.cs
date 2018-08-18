using System.Collections;
using Fungus;
using UnityEngine;

/// <summary>
/// This class is responsible for handling player movement and player interaction controls. Anything that involves input access can be invoked/handled through here.
/// </summary>
public class PlayerControl : BaseMovementController
{

    private void OnEnable()
    {
        EventManager.OnSetPlayerControl += SetPlayerControl;
    }

    private void OnDisable()
    {
        EventManager.OnSetPlayerControl -= SetPlayerControl;
    }

    public bool isControlDisabled = false;
    private bool isMoving = false;
    private Vector3 _MovementPath;

    public void Start()
    {
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
        HandleInput();
    }

    public void HandleInput()
    {
        if (!isControlDisabled)
        {
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
        }



    }

    public void SetPlayerControl(bool value) => isControlDisabled = !value;

}