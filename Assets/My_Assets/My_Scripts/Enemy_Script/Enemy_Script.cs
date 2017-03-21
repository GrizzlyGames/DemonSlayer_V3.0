using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy_Script : MonoBehaviour
{
    public bool alive = true;
    public bool canWalk = true;
    public float lookSpeed;
    public int health = 100;
    public int maxHealth = 100;
    public float rayLength = 50;
    public int damage = 15;

    public Image healthBarImage;

    private bool haveAttacked = false;
    private bool bAttacking = false;
    private Vector3 previousPosition;
    private Animator anim;
    private NavMeshAgent navMeshAgent;

    private Transform _playerTransform;
    private Vector3 _target;
    private LayerMask _LayerMask = 1 << 8;

    void Awake()
    {
        previousPosition = transform.position;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.Find("Player").transform;
    }
    void Update()
    {
        if (health > 0) // check if enemy is alive
        {
            _target = _playerTransform.position;
            #region Attack
            float distance = Vector3.Distance(transform.position, _target);     // Get distence from player // Debug.Log(distance);
            anim.SetFloat("targetDistence", distance);
            if (distance <= 3.1f && !bAttacking)
                StartCoroutine(AttackDelay());
            #endregion
            #region Determine-Actor-Speed
            float curSpeed;
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;      // Debug.Log("curSpeed: " + curSpeed);
            anim.SetFloat("speed", curSpeed);
            previousPosition = transform.position;
            #endregion      // Set actor movement animation state
            #region Chase-Player
            if (alive && canWalk)  // check if actor is alive
                navMeshAgent.destination = _target;
            else
                navMeshAgent.velocity = Vector3.zero;
            #endregion
            #region Look-At-Player
            Vector3 lookPos = _target - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
            #endregion
            HitRay();
        }
        else
        {
            navMeshAgent.velocity = Vector3.zero;
        }
    }
    public void TakeDamage(int dmg)
    {
        health -= dmg;  // Take damage Debug.Log(transform.name + " took " + dmg + " dmg. Remaing health: " + health);        
        healthBarImage.fillAmount = (float)health / (float)maxHealth;
        if (health > 0)
        {
            canWalk = false;
            StartCoroutine(WalkDelay());
            int rnd = Random.Range(1, 3);
            switch (rnd)
            {
                case 1:
                    anim.SetTrigger("hit1");
                    break;
                case 2:
                    anim.SetTrigger("hit2");
                    break;
            }
        }
        else
        {
            if (alive)
            {
                healthBarImage.enabled = false;
                canWalk = false;
                anim.SetTrigger("dead");
                Game_Manager_Script.instance.ChangeEnemyNumber(1);
                StartCoroutine(DeathDelay());
                alive = false;  // Killed   
            }
        }
    }
    
    private void HitRay()
    {
        Vector3 rayOrigin = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
        Debug.DrawRay(rayOrigin, transform.forward * 2, Color.green);      // Draw a line in the Scene View  from the point rayOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
        RaycastHit _hit;     // Declare a raycast hit to store information about what our raycast has hit
        if (Physics.Raycast(rayOrigin, transform.forward, out _hit, 2, _LayerMask))
        {
            if (_hit.transform.tag.Equals("Player") && bAttacking && !haveAttacked)
                if (_hit.transform.GetComponent<PlayerHealth_Script>())
                {
                    _hit.transform.GetComponent<PlayerHealth_Script>().Damage(damage);
                    haveAttacked = true;
                }
        }
    }

    IEnumerator AttackDelay()
    {
        bAttacking = true;
        int rnd = Random.Range(1, 4);
        anim.SetInteger("attack", rnd);
        yield return new WaitForSeconds(1);
        anim.SetInteger("attack", 0);
        bAttacking = false;
        haveAttacked = false;
    }
    IEnumerator WalkDelay()
    {
        yield return new WaitForSeconds(.5f);
        canWalk = true;
    }
    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
