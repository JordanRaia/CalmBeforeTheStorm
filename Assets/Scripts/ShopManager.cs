using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI; // Reference to ShopPanel
    public TextMeshProUGUI coinText; // Reference to CoinText
    public TextMeshProUGUI heartText;
    public TextMeshProUGUI meleeText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI rangedText;
    public TextMeshProUGUI healText;
    public TextMeshProUGUI manaRegenText;

    public PointsManager pointsManager; // Reference to PointsManager script
    public PlayerMana playerMana;
    public PlayerHealth playerHealth; // Reference to PlayerHealth script
    public Shooting shootingScript; // Reference to Shooting script

    // Prices for items
    public int heartPrice = 5;
    public int manaPrice = 5;
    public int healPrice = 1;
    public int manaRegenPrice = 5;
    public int meleeDamageUpgradePrice = 5;
    public int rangedDamageUpgradePrice = 5;

    private bool shopIsOpen = false;

    public PopupManager popupManager;

    void Start()
    {
        shopUI.SetActive(false); // Ensure the shop UI is hidden at start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && shopIsOpen)
        {
            CloseShop();
        }
    }

    public void ToggleShop()
    {
        shopIsOpen = !shopIsOpen;
        shopUI.SetActive(shopIsOpen);

        if (shopIsOpen)
        {
            // Bring Shop UI to front
            shopUI.transform.SetAsLastSibling();

            UpdateCoinDisplay();
            //Time.timeScale = 0f; // Pause the game
        }
        else
        {
            //Time.timeScale = 1f; // Resume the game
        }
    }

    public void CloseShop()
    {
        shopIsOpen = false;
        shopUI.SetActive(false);
        //Time.timeScale = 1f;
    }

    public void UpdateCoinDisplay()
    {
        coinText.text = pointsManager.GetPoints().ToString();
        heartText.text = heartPrice.ToString();
        meleeText.text = meleeDamageUpgradePrice.ToString();
        manaText.text = manaPrice.ToString();
        rangedText.text = rangedDamageUpgradePrice.ToString();
        healText.text = healPrice.ToString();
        manaRegenText.text = manaRegenPrice.ToString();
    }

    // Purchasing Functions
    public void BuyHeart()
    {
        if (pointsManager.GetPoints() >= heartPrice)
        {
            pointsManager.SpendPoints(heartPrice);
            playerHealth.IncreaseMaxHealth(1); // Increase hearts by 1
            heartPrice += 5;
            UpdateCoinDisplay();
        }
        else
        {
            // Display the popup message
            popupManager.ShowPopup("Insufficient Funds");
        }
    }

    public void BuyMana()
    {
        if (pointsManager.GetPoints() >= manaPrice)
        {
            pointsManager.SpendPoints(manaPrice);
            playerMana.IncreaseMaxMana(1); // Increase hearts by 1
            manaPrice += 5;
            UpdateCoinDisplay();
        }
        else
        {
            // Display the popup message
            popupManager.ShowPopup("Insufficient Funds");
        }
    }

    public void BuyHeal()
    {
        if (pointsManager.GetPoints() >= healPrice)
        {
            pointsManager.SpendPoints(healPrice);
            playerHealth.Heal(12);
        }
        else
        {
            // Display the popup message
            popupManager.ShowPopup("Insufficient Funds");
        }
    }

    public void BuyManaRate()
    {
        if (pointsManager.GetPoints() >= manaRegenPrice)
        {
            pointsManager.SpendPoints(manaRegenPrice);
            playerMana.IncreaseManaRegen();
            manaRegenPrice += 5;
            UpdateCoinDisplay();
        }
        else
        {
            // Display the popup message
            popupManager.ShowPopup("Insufficient Funds");
        }
    }

    public void UpgradeMeleeDamage()
    {
        if (pointsManager.GetPoints() >= meleeDamageUpgradePrice)
        {
            pointsManager.SpendPoints(meleeDamageUpgradePrice);
            shootingScript.IncreaseMeleeDamage(5); // Increase melee damage by 5
            meleeDamageUpgradePrice += 5;
            UpdateCoinDisplay();
        }
        else
        {
            // Display the popup message
            popupManager.ShowPopup("Insufficient Funds");
        }
    }

    public void UpgradeRangedDamage()
    {
        if (pointsManager.GetPoints() >= rangedDamageUpgradePrice)
        {
            pointsManager.SpendPoints(rangedDamageUpgradePrice);
            shootingScript.IncreaseRangedDamage(5); // Increase ranged damage by 5
            UpdateCoinDisplay();
        }
        else
        {
            popupManager.ShowPopup("Insufficient Funds");
        }
    }
}
