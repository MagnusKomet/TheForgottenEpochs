using PSXShaderKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPSXLevel : MonoBehaviour
{
    public void Start()
    {
        if(!PlayerPrefs.HasKey("PSXLevel"))
        {
            PlayerPrefs.SetFloat("PSXLevel", 0.3f);
        }
        GetComponent<PSXPostProcessEffect>()._PixelationFactor = PlayerPrefs.GetFloat("PSXLevel");
    }

    public void UpdatePSX()
    {
        GetComponent<PSXPostProcessEffect>()._PixelationFactor = PlayerPrefs.GetFloat("PSXLevel");
    }


    public void ResetPSX()
    {
        PlayerPrefs.SetFloat("PSXLevel",0.3f);
        UpdatePSX();        
    }
}
