using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory_Script : MonoBehaviour {

    public GameObject _dagger;

    public GameObject[] _weapon;

    private int _weaponIndex = 0;

    public void Dagger()
    {
        if (_dagger.activeInHierarchy)
        {
            _dagger.SetActive(false);
            _weapon[_weaponIndex].SetActive(true);
        }            
        else if (!_dagger.activeInHierarchy)
        {
            _dagger.SetActive(true);
        }          
    }
}
