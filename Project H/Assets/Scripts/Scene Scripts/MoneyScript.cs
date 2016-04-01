using UnityEngine;
using System.Collections;

public class MoneyScript : MonoBehaviour
{

    //This may be enum or something else, depending on the prefab.
    private int moneyAmount { get; set; }
    //Right now, amount is randomized everytime.



    public readonly int[] moneyTypes = new int[4] { 5, 20, 50, 100 };

    
    public Material[] moneyMaterials;
    void Awake()
    {
        int rnd = Random.Range(0, 4);
        //Debug.Log("Money rnd: " + rnd);
        moneyAmount = moneyTypes[rnd];
        gameObject.GetComponent<Renderer>().material = moneyMaterials[rnd];  //Change the money material according to value.
    }

    public int getMoneyAmount()
    {
        return moneyAmount;
    }

    public void setMoneyAmount(int amount)
    {
        moneyAmount = amount;
    }
}
