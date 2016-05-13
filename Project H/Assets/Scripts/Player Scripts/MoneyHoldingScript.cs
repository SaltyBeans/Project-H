using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MoneyHoldingScript : MonoBehaviour
{
    public enum handState { CLOSE, OPEN }
    [Range(0, 15)]
    public int holdNumber; //How many stacks can the player hold.
    public GameObject[] closedFingers;
    public GameObject[] openFingers;
    public GameObject palm;
    private Stack<GameObject> moneyStack;
    private Crosshair crosshair;
    void Start()
    {
        moneyStack = new Stack<GameObject>();
        crosshair = GetComponentInParent<Crosshair>();
        AnimateHand(handState.OPEN);
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
            if (crosshair.GetHit().collider.GetComponent<MoneyScript10K>() != null) // Money has collider script attached.
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
        if (moneyStack.Count < holdNumber && !moneyStack.Contains(money)) //If there is room on the hand AND if already picked that money up
        {
            moneyStack.Push(money);
            money.transform.rotation = palm.transform.rotation;
            money.transform.position = palm.transform.position;
            money.GetComponent<Collider>().enabled = false;
            money.GetComponent<Rigidbody>().velocity = Vector3.zero;
            money.transform.parent = palm.transform.parent;    //Money should move with the hand.
            money.transform.localPosition += new Vector3(0, moneyStack.Count * 0.35f, 0); //Offset the money so they can get stacked on the hand.
            money.GetComponent<Rigidbody>().isKinematic = true;
            AnimateHand(handState.CLOSE);
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
            nextMoney.GetComponent<Rigidbody>().AddForce(GetComponentInParent<Camera>().transform.forward * 50f);
            if (moneyStack.Count == 0)
            {
                AnimateHand(handState.OPEN);
            }
            else
            {
                StartCoroutine("openCloseHand");
            }     
        }
       

    }
    public void AnimateHand(handState hs)
    {
        if (hs == handState.CLOSE)
        {
            foreach(GameObject obj in openFingers)
            {
                obj.SetActive(false);
            }
            foreach(GameObject obj in closedFingers)
            {
                obj.SetActive(true);
            }
        }
        else if (hs == handState.OPEN)
        {
            foreach (GameObject obj in closedFingers)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in openFingers)
            {
                obj.SetActive(true);
            }
        }
    }
    IEnumerator openCloseHand()
    {
        AnimateHand(handState.OPEN);
        yield return new WaitForSeconds(0.1f);
        AnimateHand(handState.CLOSE);
    }
}
