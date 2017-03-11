using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Script : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private Transform _bodyRotationTrans;

    [SerializeField]
    private Camera _fpsCamera;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private float _moveSpeed = 5.0f;

    [SerializeField]
    private float _jumpSpeed = 1.0f;

    private float _gravity = 1.0f;

    private Vector3 _jumpDirection;

    private bool _isCrouching = false;
    private bool _isSprinting = false;
    private bool _wantToSprint = false;


    // Use this for initialization
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("standing_melee_kick"))
            PlayerMovement(direction);

        SetAnimator(direction);
    }

    private void PlayerMovement(Vector3 direction)
    {
        Vector3 velocity = direction * _moveSpeed;

        velocity = transform.TransformDirection(velocity);

        _controller.Move(velocity * Time.deltaTime);
    }
    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isSprinting)
        {
            _isCrouching = true;
            _fpsCamera.transform.localPosition = new Vector3(0, 1.2f, 0.25f);
            _anim.SetBool("Crouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
            _fpsCamera.transform.localPosition = new Vector3(0, 1.6f, 0.25f);
            _anim.SetBool("Crouching", false);
        }
    }
    private void Sprint(Vector3 direction)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _wantToSprint = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            _wantToSprint = false;

        if (Mathf.Abs(direction.x) > 0 || Mathf.Abs(direction.z) > 0)
        {
            Debug.Log("Want to sprint: " + _wantToSprint);
            if (Input.GetKeyDown(KeyCode.LeftShift) || _wantToSprint && !_isCrouching)
            {
                _anim.SetBool("Sprinting", true);
                _isSprinting = true;
                _fpsCamera.transform.localPosition = new Vector3(0, 1.6f, 0.5f);
                _bodyRotationTrans.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) || !_wantToSprint)
            {
                _anim.SetBool("Sprinting", false);
                _isSprinting = false;
                _fpsCamera.transform.localPosition = new Vector3(0, 1.6f, 0.3f);
                _bodyRotationTrans.localRotation = Quaternion.Euler(0, 30, 0);
            }
        }
    }
    private void Jump()
    {
        if (Input.GetButton("Jump") && GroundedCheck())
        {
            if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("jump_up") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("jump_loop") && !_anim.GetCurrentAnimatorStateInfo(0).IsName("jump_down"))
            {
                _jumpDirection.y = _jumpSpeed;
                _anim.SetTrigger("Jump");
            }
        }
        else
        {
            _jumpDirection.y -= _gravity * Time.deltaTime;
            _controller.Move(_jumpDirection * Time.deltaTime);
        }
    }
    private void Kick()
    {
        if (Input.GetKeyDown(KeyCode.F) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("standing_melee_kick"))
        {
            _anim.SetTrigger("Kick");
        }
    }
    private void SetAnimator(Vector3 direction)
    {
        _anim.SetFloat("VelX", direction.x);
        _anim.SetFloat("VelZ", direction.z);
        _anim.SetBool("Ground", GroundedCheck());
        if (direction.x != 0 || direction.z != 0)
            _anim.SetBool("Walking", true);
        else
            _anim.SetBool("Walking", false);
        Crouch();
        Sprint(direction);
        Jump();
        Kick();
    }
    private bool GroundedCheck()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.09f);
    }
}