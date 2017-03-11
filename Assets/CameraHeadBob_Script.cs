using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadBob_Script : MonoBehaviour {

    private float timer = 0.0f;
    float bobbingSpeed = 0.18f;
    float bobbingAmount = 0.05f;
    float midpoint = 1.6f;

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }

        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;

            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            float y = midpoint + translateChange;
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        else
        {
            float y = midpoint;
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }
    }
}
