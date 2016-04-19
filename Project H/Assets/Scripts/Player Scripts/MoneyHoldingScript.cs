using UnityEngine;
using System.Collections;

public class MoneyHoldingScript : MonoBehaviour {

    public GameObject[] fingers;
    public GameObject palm;
    private GameObject heldMoney;

    void Start()
    {
        heldMoney = null; 
    }

    public void HoldMoney(GameObject money)
    {
        if (heldMoney == null) //Only a single money stack can be held at once (Can be changed)
        {
            heldMoney = money;
            money.AddComponent<FixedJoint>();
            money.transform.rotation = palm.transform.rotation;
            money.transform.position = palm.transform.position;
            money.GetComponent<Collider>().enabled = false;
            money.GetComponent<Rigidbody>().velocity = Vector3.zero;
            money.GetComponent<FixedJoint>().connectedAnchor = palm.transform.localPosition;
            money.GetComponent<FixedJoint>().connectedBody = palm.GetComponent<Rigidbody>();

            //animate finger closing

        }
        else
        {
            return; //Hands already full? return
        }
    }

    public void DropMoney()
    {
        heldMoney.GetComponent<Collider>().enabled = true;
        Destroy(heldMoney.GetComponent<FixedJoint>());
        heldMoney = null;
        //animate finger releasing
    }

}
