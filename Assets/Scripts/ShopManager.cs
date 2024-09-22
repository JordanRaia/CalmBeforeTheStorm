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

    public Button buyHeartButton;
    public Button buyManaButton;

    private int heartPurchaseCount = 0;
    private int manaPurchaseCount = 0;
    private int maxPurchases = 7;

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

    // Font size settings
    [Header("Font Sizes")]
    public float defaultFontSize = 15f;       // default size
    public float soldOutFontSize = 12f;       // sold-out size

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
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void CloseShop()
    {
        shopIsOpen = false;
        shopUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateCoinDisplay()
    {
        coinText.text = pointsManager.GetPoints().ToString();

        // Update Heart Text
        if (heartPurchaseCount >= maxPurchases)
        {
            heartText.text = "Sold Out";
            heartText.fontSize = soldOutFontSize;
        }
        else
        {
            heartText.text = heartPrice.ToString();
            heartText.fontSize = defaultFontSize;
        }

        // Update Mana Text
        if (manaPurchaseCount >= maxPurchases)
        {
            manaText.text = "Sold Out";
            manaText.fontSize = soldOutFontSize;
        }
        else
        {
            manaText.text = manaPrice.ToString();
            manaText.fontSize = defaultFontSize;
        }

        // Update other texts as usual
        meleeText.text = meleeDamageUpgradePrice.ToString();
        rangedText.text = rangedDamageUpgradePrice.ToString();
        healText.text = healPrice.ToString();
        manaRegenText.text = manaRegenPrice.ToString();
    }



    // Purchasing Functions
    public void BuyHeart()
    {
        if (heartPurchaseCount >= maxPurchases)
        {
            popupManager.ShowPopup("Maximum Hearts Purchased");
            return;
        }

        if (pointsManager.GetPoints() >= heartPrice)
        {
            pointsManager.SpendPoints(heartPrice);
            playerHealth.IncreaseMaxHealth(1);
            heartPrice += 5;
            heartPurchaseCount++;

            if (heartPurchaseCount >= maxPurchases)
            {
                DisableButton(buyHeartButton, heartText);
            }

            UpdateCoinDisplay();
        }
        else
        {
            popupManager.ShowPopup("Insufficient Funds");
        }
    }


    public void BuyMana()
    {
        if (manaPurchaseCount >= maxPurchases)
        {
            popupManager.ShowPopup("Maximum Mana Purchased");
            return;
        }

        if (pointsManager.GetPoints() >= manaPrice)
        {
            pointsManager.SpendPoints(manaPrice);
            playerMana.IncreaseMaxMana(1);
            manaPrice += 5;
            manaPurchaseCount++;

            if (manaPurchaseCount >= maxPurchases)
            {
                DisableButton(buyManaButton, manaText);
            }

            UpdateCoinDisplay();
        }
        else
        {
            popupManager.ShowPopup("Insufficient Funds");
        }
    }

    private void DisableButton(Button button, TextMeshProUGUI priceText)
    {
        button.interactable = false;
        priceText.text = "Sold Out";

        // Change the button's colors to look disabled
        ColorBlock colors = button.colors;
        colors.normalColor = Color.gray;
        colors.highlightedColor = Color.gray;
        colors.pressedColor = Color.gray;
        colors.selectedColor = Color.gray;
        button.colors = colors;
    }

    public void BuyHeal()
    {
        if (pointsManager.GetPoints() >= healPrice)
        {
            pointsManager.SpendPoints(healPrice);
            playerHealth.Heal(12);
            healPrice += 1;
            UpdateCoinDisplay();
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
            rangedDamageUpgradePrice += 5;
            UpdateCoinDisplay();
        }
        else
        {
            popupManager.ShowPopup("Insufficient Funds");
        }
    }
}
