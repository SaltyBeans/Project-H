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

    public void setMoneyAmout(int amount)
    {
        moneyAmount = amount;
    }

    //Vector3 dist;
    //float posX;
    //float posY;

    //void OnMouseDown()      
    //{
    //    dist = Camera.main.WorldToScreenPoint(transform.position); //Camera distance to object
    //    posX = Input.mousePosition.x - dist.x;      //Mouse positions - Camera Distance
    //    posY = Input.mousePosition.y - dist.y;

    //}

    //void OnMouseDrag()
    //{
    //    Vector3 curPos =
    //              new Vector3(Input.mousePosition.x - posX,     //Calculate the current position -Vector3- with mouse coor X and Y
    //              Input.mousePosition.y - posY, dist.z);

    //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos); 
    //    transform.position = worldPos;

    //    Debug.Log("Dragged money value: " + moneyAmount);

    //}

}
