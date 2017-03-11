using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD_Script : MonoBehaviour {

    public static PlayerHUD_Script instance;

    public Text _ammoText;

    private void Awake()
    {
        instance = this;
        MouseLock();
    }

    public void AmmoText(string message)
    {
        _ammoText.text = message;
    }

    private void MouseLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
