using UnityEngine;
using System.Collections;

public class MoneyTransactionScript : MonoBehaviour
{
    private GameObject[] sceneMoney;

    void Start()
    {
        sceneMoney = GameObject.FindGameObjectsWithTag("Money");
    }

    public void Buy(int price)
    {
        bool priceIsPaid = false;
        int index = 0;
        while (!priceIsPaid && index <= sceneMoney.Length)
        {
            if (price >= 10000)
            {
                price -= sceneMoney[index].GetComponent<MoneyScript10K>().getMoneyAmount();
                Destroy(sceneMoney[index]);
                index++;
            }
            else
            {
                if (sceneMoney[index].GetComponent<MoneyScript10K>().getMoneyAmount() >= price && sceneMoney[index].GetComponent<MoneyScript10K>().getMoneyAmount() != 10000)
                {
                    sceneMoney[index].GetComponent<MoneyScript10K>().setMoneyAmout(sceneMoney[index].GetComponent<MoneyScript10K>().getMoneyAmount() - price);
                    price = 0;
                }
                else
                {
                    price -= sceneMoney[index].GetComponent<MoneyScript10K>().getMoneyAmount();
                    Destroy(sceneMoney[index]);
                    index++;
                }
            }
            if (price == 0)
                priceIsPaid = true;            
        }
    }
}