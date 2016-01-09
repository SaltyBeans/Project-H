using UnityEngine;
using System.Collections;

public class MoneyScript10K : MonoBehaviour
{

    //This may be enum or something else, depending on the prefab.
    private int moneyAmount { get; set; }
    //Right now, the amount is set at 10.000$.

    public readonly int moneyType = 10000;

    void Start()
    {
        moneyAmount = 10000;
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
