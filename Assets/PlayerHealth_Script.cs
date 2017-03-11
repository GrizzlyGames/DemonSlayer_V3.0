using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth_Script : MonoBehaviour {

    public Image _hurtImage;

    public bool _alive = true;
    public int _health = 100;
    public int _maxHealth = 100;

    public void Damage(int damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _health = 0;
            _alive = false;
        }
        else
        {
            HealthHUD();
        }        
    }
    private void HealthHUD()
    {
        float _transparencyAmount;
        _transparencyAmount = ((float)_health / (float)_maxHealth) * 255;
        _transparencyAmount = Mathf.Abs(255 - _transparencyAmount);
        Debug.Log("Hurt image transparency amount: " + _transparencyAmount);
        _hurtImage.color = new Color32(255, 255, 255, (byte)_transparencyAmount);
    }
}
