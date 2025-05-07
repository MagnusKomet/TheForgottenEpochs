using System.Collections;
using System.Collections.Generic;
using PlayerSpace;
using TMPro;
using UnityEngine;

public class SpellsCreatorController : MonoBehaviour
{

    private HashSet<string> allSpells = new HashSet<string> { "F", "W", "A", "E", "WAWA", "AAAE", "FFAFFA" };


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
        txt_spellDescription.text = "";
        txt_spellName.text = "";

        switch (combo)
        {
            case "F":
                txt_spellName.text = "Fireball";
                txt_spellDescription.text = "A spherical mass of pure fire that follows a straight trajectory, exploding on contact with a surface or enemy.";
                lastSpell = Instantiate(fireball, transform.position, transform.rotation);
                break;

            case "A":
                txt_spellName.text = "Tornado";
                txt_spellDescription.text = "The miniaturized version of the deathly natural disaster, the tongues once spoke, that the bravest wizard can claim the strength of the wind.";
                lastSpell = Instantiate(tornado, transform.position, transform.rotation);
                break;

            case "E":
                txt_spellName.text = "The Wall";
                txt_spellDescription.text = "Legends once spoke of an unbreakable barrier with countless purposes. Which brave wizards could use to challenge gravity.";
                lastSpell = Instantiate(theWall, transform.position, transform.rotation);
                break;

            case "W":
                txt_spellName.text = "Bubble";
                txt_spellDescription.text = "The first remembered use of magic, a water bubble with density similar to air, charged with anti-mana, causing the bubble to follow its targets.";
                lastSpell = Instantiate(bubble, transform.position, transform.rotation);
                break;

            case "WAWA":
                txt_spellName.text = "Sacred Storm";
                txt_spellDescription.text = "Your lungs are flooded with sacred vapor, and an immense sensation of satisfaction and achieving heaven courses through your spine.";
                lastSpell = Instantiate(waterRing, transform.position, transform.rotation);
                break;

            case "FFAFFA":
                txt_spellName.text = "Fireball Conjurer";
                txt_spellDescription.text = "An archaic ritual capable of repeatedly casting the legendary FireBall, self-sustained with the environmental mana.";
                lastSpell = Instantiate(fireballConjurer, transform.position, transform.rotation);
                break;

            case "AAAE":
                txt_spellName.text = "Fragmented Blade";
                txt_spellDescription.text = "A stone-forged conglomerate of andesite, propelled by compressed air infused with mana. Thus mimicking countless supersonic stone-bullets.";
                lastSpell = Instantiate(windBlade, transform.position, transform.rotation);
                break;
        }
    }

    public void CreateSpell()
    {
        if (!string.IsNullOrEmpty(combo) && allSpells.Contains(combo))
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
        txt_spellDescription.text = "";
        txt_spellName.text = "";
        Destroy(lastSpell);
    }

    public void ActivatePanel()
    {
        InventoryVisualManager.Instance.MenuActivated(true);

    }
}
