using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsCreatorController : MonoBehaviour
{

    GameObject fireball;
    GameObject windblade;

    bool panelActivated = false;

    string combo = "";


    public void AddToCombo(char element)
    {
        combo += element;
    }

    public void CastSpell()
    {
        if (combo == "FF")
        {
            Instantiate(fireball, transform.position, transform.rotation);
        }
        else if (combo == "WW")
        {
            Instantiate(windblade, transform.position, transform.rotation);
        }
        else if (combo == "FW" || combo == "WF")
        {
            Instantiate(fireball, transform.position, transform.rotation);
            Instantiate(windblade, transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("Invalid combo");
        }

        combo = "";
    }

    public void ActivatePanel()
    {
        panelActivated = !panelActivated;

        Time.timeScale = panelActivated ? 0 : 1;
        Cursor.visible = panelActivated;
        Cursor.lockState = panelActivated ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
