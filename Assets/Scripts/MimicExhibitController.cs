using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicExhibitController : MonoBehaviour
{
    public GameObject mimicInfoPanel;
    public GameObject mimicSelectorPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMimicPanel()
    {
        if(mimicInfoPanel.activeSelf)
        {
            mimicInfoPanel.SetActive(false);
            mimicSelectorPanel.SetActive(true);
        }
        else
        {
            mimicInfoPanel.SetActive(true);
            mimicSelectorPanel.SetActive(false);
        }
    }
}
