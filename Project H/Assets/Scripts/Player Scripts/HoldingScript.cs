using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingScript : MonoBehaviour
{
    public enum handState { CLOSE, OPEN, SLEDGE }
    [Range(0, 15)]
    public int holdNumber; //How many stacks can the player hold.
    public GameObject[] closedFingers;
    public GameObject[] openFingers;
    public GameObject[] closedSledgeFingers;
    public GameObject palm;
    public GameObject sledgePalm;
    private bool sledgeIsHeld;
    private GameObject sledgeHammer;
    private Stack<GameObject> moneyStack;
    private Crosshair crosshair;
    private Transform camTransform;
    void Start()
    {
        camTransform = GetComponentInParent<Camera>().transform;
        moneyStack = new Stack<GameObject>();
        crosshair = GetComponentInParent<Crosshair>();
        sledgeIsHeld = false;
        AnimateHand(handState.OPEN);
        if (crosshair == null)
        {
            Debug.LogError("Need Crosshair script on the character!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            DropItem();


        else if (crosshair.GetHit().collider != null && crosshair.GetHit().collider.tag == "Money")
        {
            if (crosshair.GetHit().collider.GetComponent<MoneyScript10K>() != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    HoldMoney(crosshair.GetHit().collider.gameObject);
                }
            }
        }
        else if (crosshair.GetHit().collider!= null && crosshair.GetHit().collider.tag == "sHammer")
        {
            if (moneyStack.Count == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    HoldSledge(crosshair.GetHit().collider.gameObject);
                }
            }
        }

        if (sledgeIsHeld)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine("swingHammer");
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
    public void HoldSledge(GameObject sledge)
    {
        sledgeHammer = sledge;
        sledgeHammer.transform.parent = palm.transform.parent;
        sledgeHammer.transform.localPosition = new Vector3(0.25f,-8.89f,0.04f);
        sledgeHammer.transform.localRotation = Quaternion.Euler(1.482f,289.18f,11.18f);
        sledgeHammer.GetComponent<Collider>().enabled = false;
        sledgeHammer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        sledgeHammer.GetComponent<Rigidbody>().isKinematic = true;
        sledgeIsHeld = true;
        AnimateHand(handState.SLEDGE);
    }
    public void DropItem()
    {
        if (moneyStack.Count > 0) //If there is at least one stack on the hand.
        {
            GameObject nextMoney = moneyStack.Peek();
            moneyStack.Pop();
            nextMoney.GetComponent<Collider>().enabled = true;
            nextMoney.GetComponent<Rigidbody>().isKinematic = false;
            nextMoney.transform.parent = null;
            nextMoney.GetComponent<Rigidbody>().AddForce(camTransform.forward * 50f + (-camTransform.right * 15f));
            if (moneyStack.Count == 0)
            {
                AnimateHand(handState.OPEN);
            }
            else
            {
                StartCoroutine("openCloseHand");
            }
        }
        else if (sledgeIsHeld)
        {
            sledgeHammer.GetComponent<Collider>().enabled = true;
            sledgeHammer.GetComponent<Rigidbody>().isKinematic = false;
            sledgeHammer.transform.parent = null;
            sledgeHammer.GetComponent<Rigidbody>().AddForce(camTransform.forward * 50f + (-camTransform.right * 15f));
            sledgeHammer = null;
            sledgeIsHeld = false;
            AnimateHand(handState.OPEN);
        }
    }
    public void AnimateHand(handState hs)
    {
        if (hs == handState.CLOSE)
        {
            palm.SetActive(true);
            sledgePalm.SetActive(false);
            foreach (GameObject obj in openFingers)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in closedFingers)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in closedSledgeFingers)
            {
                obj.SetActive(false);
            }
        }
        else if (hs == handState.OPEN)
        {
            palm.SetActive(true);
            sledgePalm.SetActive(false);
            foreach (GameObject obj in closedFingers)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in openFingers)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in closedSledgeFingers)
            {
                obj.SetActive(false);
            }
        }
        else if (hs == handState.SLEDGE)
        {
            palm.SetActive(false);
            sledgePalm.SetActive(true);
            foreach (GameObject obj in closedFingers)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in openFingers)
            {
                obj.SetActive(false);
            }
            foreach(GameObject obj in closedSledgeFingers)
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
    IEnumerator swingHammer()
    {
        transform.localRotation = Quaternion.Euler(52.17f, 326.66f, 334.18f);
        yield return new WaitForSeconds(0.1f);
        transform.localRotation = Quaternion.Euler(35.35f, 335.02f, 346.11f);
    }
}
