using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class MoneyHoldingScript : MonoBehaviour
{
    [Range(0, 15)]
    public int holdNumber; //How many stacks can the player hold.
    public GameObject[] fingers;
    public GameObject palm;
    private GameObject heldMoney;
    private Stack<GameObject> moneyStack;
    private DragRigidbody dragScript;
    private Crosshair crosshair;
    void Start()
    {
        heldMoney = null;
        dragScript = GetComponentInParent<DragRigidbody>();
        moneyStack = new Stack<GameObject>();
        crosshair = GetComponentInParent<Crosshair>();

        if (crosshair == null)
        {
            Debug.LogError("Need Crosshair script on the character!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DropMoney();
        }

        else if (crosshair.GetHit().collider != null && crosshair.GetHit().collider.tag == "Money")
        {
            if (crosshair.GetHit().collider.GetComponent<MoneyScript>() == null) // Money has collider script attached.
            {
                if (Input.GetMouseButtonDown(0))
                {
                    HoldMoney(crosshair.GetHit().collider.gameObject);
                }
            }
        }
    }

    public void HoldMoney(GameObject money)
    {
        if (moneyStack.Count < holdNumber && !moneyStack.Contains(money)) //If there is room on the hand && if already picked that money up
        {
            moneyStack.Push(money);
            money.transform.rotation = palm.transform.rotation;
            money.transform.position = palm.transform.position;
            money.GetComponent<Collider>().enabled = false;
            money.GetComponent<Rigidbody>().velocity = Vector3.zero;
            money.transform.parent = palm.transform.parent;    //Money should move with the hand.
            money.transform.localPosition += new Vector3(0, moneyStack.Count * 0.35f, 0); //Offset the money so they can get stacked on the hand.
            money.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void DropMoney()
    {
        if (moneyStack.Count > 0) //If there is at least one stack on the hand.
        {
            GameObject nextMoney = moneyStack.Peek();
            moneyStack.Pop();
            nextMoney.GetComponent<Collider>().enabled = true;
            nextMoney.GetComponent<Rigidbody>().isKinematic = false;
            nextMoney.transform.parent = null;
            //dragScript.enabled = true;

            //animate finger releasing            
        }

    }

}
