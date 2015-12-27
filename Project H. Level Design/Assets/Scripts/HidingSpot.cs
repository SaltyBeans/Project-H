using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    //This script will be a hiding spot/Furniture's component. 

    public int moneyLimit;      //Limit that this object can hold.
    private int currentMoney; //Money amount this object is holding at this moment.
    public GameObject moneyPrefab;
    public Transform moneySpawn;

    int[] moneyInside = new int[4];     //Money that is "inside" the object.

    //TextMesh text;

    void Start()
    {
        //text = GetComponentInChildren<TextMesh>();
        currentMoney = 0;
    }

    void Update()
    {
       // text.text = "Money: " + currentMoney;
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag != "Money")
            return;

        MoneyScript moneyScript = other.gameObject.GetComponent<MoneyScript>();

        int moneyAmount = moneyScript.getMoneyAmount();

        int total = currentMoney + moneyAmount; //total with to money player's holding.

        if (total > moneyLimit) //If the amount we want  to add + current money is bigger than money limit.
            return;                                                                         //Exit.

        else
        {
            currentMoney = currentMoney + moneyAmount;         //Add up the money
            moneyInside[moneyToIndex(moneyAmount)]++;   //Iterate the moneyInside
            Destroy(other.gameObject);                                      //Destroy the object.
        }
    }

    void OnMouseOver()
    {

        if (Input.GetKeyDown(KeyCode.C))        //Eject/Pop ALL THE STACKS inside.
        {
            for (int i = 3; i >= 0; i--)
            {
                Debug.Log("money inside" + i + " " + moneyInside[i]);
                while (moneyInside[i] > 0)
                    ejectTheStack(i);

            }
        }

        if (Input.GetKeyDown(KeyCode.X))    //Eject/Pop ONLY THE HIGHEST stack inside.
        {
            for (int i = 3; i >= 0; i--)
            {
                if (moneyInside[i] != 0)
                {
                    ejectTheStack(i);
                    break;
                }
            }
        }
    }

    void ejectTheStack(int stackCode)   //Stack code is 0, 1, 2 or 3. Which stack do you want to POP! a 5$ stack? 20$ stack ? 50$ or 100$ stack?
    {
        GameObject mon = Instantiate(moneyPrefab, moneySpawn.position, Quaternion.identity) as GameObject;
        MoneyScript monScript = mon.GetComponent<MoneyScript>();
        monScript.setMoneyAmout(monScript.moneyTypes[stackCode]);
        mon.GetComponent<Renderer>().material = monScript.moneyMaterials[stackCode];
        currentMoney -= monScript.moneyTypes[stackCode];
        moneyInside[stackCode]--;
    }


    int moneyToIndex(int Amount) //To iterate the moneyInside array, change | 5->0 | 20->1 | etc.
    {
        switch (Amount)
        {
            case 5:
                return 0;
            case 20:
                return 1;
            case 50:
                return 2;
            case 100:
                return 3;
            default:
                return -99;
        }
    }
    public int getAmount()
    {
        return currentMoney;
    }
    public int getMaxAmount()
    {
        return moneyLimit;
    }

}
