using System.Collections;
using System.Collections.Generic;
using PlayerSpace;
using TMPro;
using UnityEngine;

public class SpellsCreatorController : MonoBehaviour
{
    [SerializeField]
    TMP_Text txt_spellName;
    [SerializeField]
    TMP_Text txt_spellDescription;
    [SerializeField]
    TMP_Text txt_combo;


    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private GameObject tornado;
    [SerializeField]
    private GameObject theWall;
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private GameObject waterRing;
    [SerializeField]
    private GameObject fireballConjurer;
    [SerializeField]
    private GameObject windBlade;


    [SerializeField]
    TMP_Text fireText;
    [SerializeField]
    TMP_Text airText;
    [SerializeField]
    TMP_Text earthText;
    [SerializeField]
    TMP_Text waterText;
    [SerializeField]
    TMP_Text fireTextPrice;
    [SerializeField]
    TMP_Text airTextPrice;
    [SerializeField]
    TMP_Text earthTextPrice;
    [SerializeField]
    TMP_Text waterTextPrice;
    int firePrice = 0;
    int airPrice = 0;
    int earthPrice = 0;
    int waterPrice = 0;
    GameObject lastSpell;

    string combo = "";

    PlayerController playerController;
    InventoryDataController inventoryData;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        inventoryData = InventoryVisualManager.Instance.inventoryData;
    }

    public void AddElementToCombo(string element)
    {
        if(combo.Length < 7)
        {
            switch (element)
            {
                case "F":
                    if (fireText.text.Length < 5)
                    {
                        combo += "F";
                        fireText.text += "*";
                        firePrice += 10;
                        fireTextPrice.text = firePrice.ToString();
                        CastSpell();
                    }
                    break;

                case "W":
                    if (waterText.text.Length < 5)
                    {
                        combo += "W";
                        waterText.text += "@";
                        waterPrice += 10;
                        waterTextPrice.text = waterPrice.ToString();
                        CastSpell();
                    }
                    break;

                case "E":
                    if (earthText.text.Length < 15)
                    {
                        combo += "E";
                        earthText.text += ";!;";
                        earthPrice += 10;
                        earthTextPrice.text = earthPrice.ToString();
                        CastSpell();
                    }
                    break;

                case "A":
                    if (airText.text.Length < 5)
                    {
                        combo += "A";
                        airText.text += "0";
                        airPrice += 10;
                        airTextPrice.text = airPrice.ToString();
                        CastSpell();
                    }
                    break;
            }
        }
         
    }

    public void CastSpell()
    {
        Destroy(lastSpell);

        txt_combo.text = combo;

        switch (combo)
        {
            case "F":
                lastSpell = Instantiate(fireball, transform.position, transform.rotation);
                break;

            case "A":
                lastSpell = Instantiate(tornado, transform.position, transform.rotation);
                break;

            case "E":
                lastSpell = Instantiate(theWall, transform.position, transform.rotation);
                break;

            case "W":
                lastSpell = Instantiate(bubble, transform.position, transform.rotation);
                break;

            case "WAWA":
                lastSpell = Instantiate(waterRing, transform.position, transform.rotation);
                break;

            case "FFAFFA":
                lastSpell = Instantiate(fireballConjurer, transform.position, transform.rotation);
                break;

            case "AAAE":
                lastSpell = Instantiate(windBlade, transform.position, transform.rotation);
                break;
        }
    }

    public void CreateSpell()
    {
        if (!string.IsNullOrEmpty(combo))
        {
            if (!playerController.unlockedSpells.Contains(combo))
            {
                if (inventoryData.GetObjectCount("Fire Crystal") >= firePrice &&
                inventoryData.GetObjectCount("Air Crystal") >= airPrice &&
                inventoryData.GetObjectCount("Earth Crystal") >= earthPrice &&
                inventoryData.GetObjectCount("Water Crystal") >= waterPrice)
                {
                    inventoryData.RemoveItem("Fire Crystal", firePrice);
                    inventoryData.RemoveItem("Air Crystal", airPrice);
                    inventoryData.RemoveItem("Earth Crystal", earthPrice);
                    inventoryData.RemoveItem("Water Crystal", waterPrice);

                    playerController.unlockedSpells.Add(combo);
                    playerController.SaveData();
                    inventoryData.SaveData();
                    ResetSpell();

                    ToastNotification.Show("New spell created, good job sirrrr");
                }
                else
                {
                    ToastNotification.Show("No crystals sirrrr?");
                }
            }
            else
            {
                ToastNotification.Show("You already know that sirrrr");
            }
        }
        else
        {
            ToastNotification.Show("That doesn't exist sirrrr");
        }


    }

    public void ResetSpell()
    {
        combo = "";
        fireText.text = "";
        airText.text = "";
        earthText.text = "";
        waterText.text = "";
        txt_combo.text = combo;
        firePrice = 0;
        airPrice = 0;
        earthPrice = 0;
        waterPrice = 0;
        fireTextPrice.text = "";
        airTextPrice.text = "";
        earthTextPrice.text = "";
        waterTextPrice.text = "";
        Destroy(lastSpell);
    }

    public void ActivatePanel()
    {
        InventoryVisualManager.Instance.MenuActivated(true);

    }
}
