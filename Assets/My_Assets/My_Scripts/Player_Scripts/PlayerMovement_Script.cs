using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Script : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;

    [SerializeField]
    private MeshRenderer _weaponRender;

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

    private float _runMultiplier = 2.5f;

    [SerializeField]
    private float _gravity = 1.0f;

    private Vector3 _jumpDirection;

    private bool _isCrouching = false;
    private bool _isSprinting = false;
    private bool _wantToSprint = false;
    private bool _isKicking = false;
    private float _yRotation;
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

        PlayerMovement(direction);
        SetAnimator(direction);
    }

    private void PlayerMovement(Vector3 direction)
    {
        Vector3 velocity = direction * (_moveSpeed * _runMultiplier);

        velocity = transform.TransformDirection(velocity);

        _controller.Move(velocity * Time.deltaTime);
    }
    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !_isSprinting)
        {
            _isCrouching = true;
            _anim.SetBool("Crouching", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _isCrouching = false;
            _anim.SetBool("Crouching", false);
        }
    }
    private void Sprint(Vector3 direction)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _wantToSprint = true;
            _runMultiplier = 2.5f;
        }


        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _wantToSprint = false;
            _runMultiplier = 1;
        }

        if (Mathf.Abs(direction.x) > 0 || Mathf.Abs(direction.z) > 0)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) || _wantToSprint && !_isCrouching)
            {
                _anim.SetBool("Sprinting", true);
                _isSprinting = true;
                _bodyRotationTrans.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) || !_wantToSprint)
            {
                _anim.SetBool("Sprinting", false);
                _isSprinting = false;
                _bodyRotationTrans.localRotation = Quaternion.Euler(0, 33, 0);
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
                _isCrouching = false;
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
            _bodyRotationTrans.localRotation = Quaternion.Euler(0, 5, 0);
            _weaponRender.enabled = false;
            _anim.SetTrigger("Kick");
        }
    }
    private void SetAnimator(Vector3 direction)
    {
        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("standing_melee_kick"))
        {
            _anim.SetFloat("VelX", direction.x);
            _anim.SetFloat("VelZ", direction.z);
        }
        else
        {
            _anim.SetFloat("VelX", 0);
            _anim.SetFloat("VelZ", 0);
        }

        _anim.SetBool("Ground", GroundedCheck());
        if (direction.x != 0 || direction.z != 0)
            _anim.SetBool("Walking", true);
        else
            _anim.SetBool("Walking", false);
        Crouch();
        Sprint(direction);
        Jump();
        if (!_isSprinting && !_isCrouching)
            Kick();

        if (_isKicking)
            _bodyRotationTrans.localRotation = Quaternion.Euler(0, 5, 0);
    }
    public float SetYRotation(float y)
    {
        _yRotation = y;
        return (_yRotation);
    }
    public void SetMovementSpeed(float speed)
    {
        _moveSpeed = speed;
    }
    public void BodyRotation()
    {
        _bodyRotationTrans.localRotation = Quaternion.Euler(0, _yRotation, 0);
    }
    public void SetKick()
    {
        if (_isKicking)
            _isKicking = false;
        else if (!_isKicking)
            _isKicking = true;
    }
    private bool GroundedCheck()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.09f);
    }

    public GameObject _playerMesh;

    public void ChangeCameraCullingMask(int mask)
    {
        _playerMesh.layer = mask;
    }
    public void EnableWeaponMeshRender()
    {
        _weaponRender.enabled = true;
    }
}