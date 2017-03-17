using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot_Script : MonoBehaviour
{
    public Camera _fpsCam;
    public Animator _animator;
    public ArcReactor_Launcher _arcReactorLauncher;

    public int _currentAmmo = 12;
    public int _magazineCapacity = 12;
    public int _maximumAmmo = 32;

    public int _damage = 1;                                     // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float _fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float _reloadDelay = 0.5f;
    public float _weaponRange = 50f;                                    // Distance in Unity units over which the player can fire

    private LayerMask _LayerMask = 1 << 8;
    private float _nextFire;
    private bool _bReloading;

    private void Start()
    {
        if(!_animator)
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Shoot();
        Reload();
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentAmmo >= 1 && !_bReloading && Time.time > _nextFire)
            {                                        
                _nextFire = Time.time + _fireRate;        // Update the time when our player can fire next

                Vector3 rayOrigin = _fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));     // Create a vector at the center of our camera's viewport
                Debug.DrawRay(rayOrigin, _fpsCam.transform.forward * _weaponRange, Color.green);      // Draw a line in the Scene View  from the point rayOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
                RaycastHit _hit;     // Declare a raycast hit to store information about what our raycast has hit
                if (Physics.Raycast(rayOrigin, _fpsCam.transform.forward, out _hit, _weaponRange, _LayerMask))
                {
                    Debug.Log("Raycast debug: " + _hit.transform.name);
                }
                StartCoroutine(ShotEffect());
                PlayerHUD_Script.instance.AmmoText(_currentAmmo.ToString("00") + " / " + _magazineCapacity.ToString("00") + " | " + _maximumAmmo.ToString("000"));
            }
            if (_currentAmmo < 1 && _maximumAmmo > 1 && !_bReloading)
            {
                PlayerHUD_Script.instance.AmmoText("'R' to  reload");
            }
            else if (_maximumAmmo < 1 && _currentAmmo < 1)
            {
                PlayerHUD_Script.instance.AmmoText("No ammo!");
            }
        }            
    }
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_bReloading)
            {
                StartCoroutine("ReloadDelay");
            }
        }
    }

    IEnumerator ShotEffect()
    {
        // Play audio
        _currentAmmo--;
        _animator.SetTrigger("ShootRifle");
        yield return new WaitForSeconds(0.25f);
        _arcReactorLauncher.LaunchRay();
    }
    IEnumerator ReloadDelay()
    {
        _bReloading = true;
        PlayerHUD_Script.instance.AmmoText("Reloading...");

        yield return new WaitForSeconds(_reloadDelay);

        if (_currentAmmo + _maximumAmmo >= _magazineCapacity)
        {
            _maximumAmmo = _maximumAmmo - (_magazineCapacity - _currentAmmo);
            _currentAmmo = _magazineCapacity;
        }
        else if (_currentAmmo + _maximumAmmo < _magazineCapacity)
        {
            _currentAmmo += _maximumAmmo;
            _maximumAmmo = 0;
        }
        PlayerHUD_Script.instance.AmmoText(_currentAmmo.ToString() + " / " + _magazineCapacity.ToString() + "|" + _maximumAmmo.ToString());       // Updates reload text
        _bReloading = false;
    }
}
