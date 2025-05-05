using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPSX : MonoBehaviour
{
    private float PSXLevel = 0.3f;
    public GameObject onBackground;
    public GameObject offBackground;


    void Start()
    {
        if (PlayerPrefs.HasKey("PSXLevel"))
        {
            PSXLevel = PlayerPrefs.GetFloat("PSXLevel");
        }
        else
        {
            PlayerPrefs.SetFloat("PSXLevel",PSXLevel);
        }

        if (PSXLevel >= 1)
        {
            PSXOff();
        }
        else
        {
            if (PSXLevel < 0.1f)
            {
                SetPSXLevel(0.1f);
            }

            PSXOn();
        }
    }

    public void SetPSXLevel(float level)
    {
        PSXLevel = level;
        PlayerPrefs.SetFloat("PSXLevel", PSXLevel);
        PlayerPrefs.Save();
    }

    public void PSXOn()
    {
        if (PlayerPrefs.HasKey("PSXLevel"))
        {
            PSXLevel = PlayerPrefs.GetFloat("PSXLevel");
        }

        if (PSXLevel >= 1)
        {
            SetPSXLevel(0.3f);
        }
        else
        {
            SetPSXLevel(PSXLevel);
        }
        
        onBackground.SetActive(true);
        offBackground.SetActive(false);
    }

    public void PSXOff()
    {
        SetPSXLevel(1);
        onBackground.SetActive(false);
        offBackground.SetActive(true);
    }
}
