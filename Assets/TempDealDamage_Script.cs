using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDealDamage_Script : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.transform.GetComponent<PlayerHealth_Script>().Damage(10);
    }
}
