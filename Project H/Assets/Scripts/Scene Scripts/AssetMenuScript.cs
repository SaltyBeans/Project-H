using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MoneyTransactionScript))]
[RequireComponent(typeof(PauseMenuScript))]

public class AssetMenuScript : MonoBehaviour
{
    public GameObject assetMenu;
    public GameObject player;
    private MoneyTransactionScript transaction;
    private PauseMenuScript pauseMenu;
    public Text totalCashText;
    public Text warningText;

    private bool sledgeBought;
    private bool basementKeyBought;
    private bool itemYBought; //placeholder
    private bool itemZBought; //placeholder
    
    void Start ()
    {
        sledgeBought = false;
        basementKeyBought = false;
        itemYBought = false;
        itemZBought = false;
	    assetMenu.SetActive(false);
        transaction = GetComponent<MoneyTransactionScript>();
        pauseMenu = GetComponent<PauseMenuScript>();
        totalCashText.text = "Total Cash: " + transaction.getTotalCash() + "$";
    }
	
	void Update ()
	{
	    totalCashText.text ="Total Cash: " + transaction.getTotalCash() + "$";

        if (Input.GetKey(KeyCode.Tab) && !pauseMenu.getPauseMenuStatus())
	    {
	        assetMenu.SetActive(true);
            player.GetComponent<Crosshair>().OriginalOn = false;
            player.GetComponent<Crosshair>().CursorLock = false;
            Time.timeScale = 0;
	    }
	    else if(Input.GetKeyUp(KeyCode.Tab) && !pauseMenu.getPauseMenuStatus())
	    {
	        assetMenu.SetActive(false);
            player.GetComponent<Crosshair>().OriginalOn = true;
            player.GetComponent<Crosshair>().CursorLock = true;
	        warningText.text = null;
            Time.timeScale = 1;
	    }
	    
	}
    
    public void PurchaseItemWithID(int id)//When the button with id is pressed, perform the purchase transaction
    {
        if (id == 1 && !sledgeBought)
        {
            if (transaction.getTotalCash() >= 1000)
            {
                warningText.text = "Bought The Sledgehammer!";
                sledgeBought = true;
                transaction.Buy(1000);
            }
            else
            {
                warningText.text = "Warning: Not enough money!";
            }
        }
        else if (id == 2 && !basementKeyBought)
        {
            if (transaction.getTotalCash() >= 5000)
            {
                warningText.text = "Bought The Basement Key!";
                basementKeyBought = true;
                transaction.Buy(5000);
            }
            else
            {
                warningText.text = "Warning: Not enough money!";
            }
        }
        else if (id == 3 && !itemYBought)
        {
            if (transaction.getTotalCash() >= 0)
            {
                itemYBought = true;
                transaction.Buy(0);
            }
            else
            {
                warningText.text = "Warning: Not enough money!";
            }
        }
        else if (id == 4 && !itemZBought)
        {
            if (transaction.getTotalCash() >= 0)
            {
                itemZBought = true;
                transaction.Buy(0);
            }
            else
            {
                warningText.text = "Warning: Not enough money!";
            }
        }
        else
        {
            warningText.text = "Warning: Item already bought!";
        }
    }

    public void placeBoughtItems() //Place the bought items to their places | Will be called at the end of the wave
    {
        if (sledgeBought)
        {
            
        }
        if (basementKeyBought)
        {
            
        }
        if (itemYBought)
        {
            
        }
        if (itemZBought)
        {
            
        }
    }
}
