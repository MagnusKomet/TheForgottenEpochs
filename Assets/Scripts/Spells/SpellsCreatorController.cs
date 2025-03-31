using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellsCreatorController : MonoBehaviour
{
    [SerializeField]
    TMP_Text txt_combo;
    [SerializeField]
    GameObject fireball;
    [SerializeField]
    GameObject windblade;

    GameObject playerModel;
    [SerializeField]
    TMP_Text interactionText;
    GameObject lastSpell;

    bool panelActivated = false;

    string combo = "";

    private void Start()
    {
        playerModel = GameObject.Find("CharacterModel");
        
    }

    public void AddFireToCombo()
    {
        combo += "F";
        CastSpell();
        CastSpell();
    }

    public void AddWaterToCombo()
    {
        combo += "W";
        CastSpell();
    }

    public void AddEarthToCombo()
    {
        combo += "E";
        CastSpell();
    }

    public void AddAirToCombo()
    {
        combo += "A";
        CastSpell();
    }

    public void CastSpell()
    {
        Destroy(lastSpell);

        txt_combo.text = combo;

        if (combo == "FFA")
        {
            lastSpell = Instantiate(fireball, transform.position, transform.rotation);
        }
        else if (combo == "AAAE")
        {
            lastSpell = Instantiate(windblade, transform.position, transform.rotation);
        }

    }

    public void ResetSpell()
    {
        combo = "";
        txt_combo.text = combo;
        Destroy(lastSpell);
    }

    public void ActivatePanel()
    {
        panelActivated = !panelActivated;

        Time.timeScale = panelActivated ? 0 : 1;
        Cursor.visible = panelActivated;
        Cursor.lockState = panelActivated ? CursorLockMode.None : CursorLockMode.Locked;

        playerModel.SetActive(!panelActivated);
        interactionText.text = !panelActivated ? "Press [RMB] to create new spell" : "";
    }
}
