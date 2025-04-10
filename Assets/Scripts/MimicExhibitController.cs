using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MimicExhibitController : MonoBehaviour
{
    public GameObject invokedMimic;
    public GameObject mimicInfoPanel;
    public GameObject mimicSelectorPanel;
    public Transform mimicSpawnPoint;
    public int exhibitNumber;

    public void ChangeMimicPanel()
    {
        if (mimicInfoPanel.activeSelf)
        {
            if (invokedMimic != null)
            {
                Destroy(invokedMimic);
            }
            mimicInfoPanel.SetActive(false);
            mimicSelectorPanel.SetActive(true);
        }
        else
        {

            mimicInfoPanel.SetActive(true);
            mimicSelectorPanel.SetActive(false);
        }
    }

    public void InvokeMimic(GameObject mimic)
    {
        if (invokedMimic != null)
        {
            Destroy(invokedMimic);
        }
             
        invokedMimic = Instantiate(mimic, mimicSpawnPoint.position, mimicSpawnPoint.rotation);
        ChangeMimicPanel();

    }



}
