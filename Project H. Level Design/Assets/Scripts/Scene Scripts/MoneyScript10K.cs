using UnityEngine;
using System.Collections;

public class MoneyScript10K : MonoBehaviour
{

    //This may be enum or something else, depending on the prefab.
    private int moneyAmount { get; set; }
    //Right now, amount is randomized everytime.



    public readonly int moneyType = 10000;

    void Start()
    {
        moneyAmount = 100000;
    }

    public int getMoneyAmount()
    {
        return moneyAmount;
    }

    public void setMoneyAmout(int amount)
    {
        moneyAmount = amount;
    }
}
