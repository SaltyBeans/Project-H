using UnityEngine;
using System.Collections;

public class MoneyScript10K : MonoBehaviour
{
    private int moneyAmount { get; set; }
    //Right now, the amount is set at 10.000$.



    float oldDrag;
    float oldAngularDrag;
    public readonly int moneyType = 100000;

    void Start()
    {
        moneyAmount = 100000;
        oldDrag = GetComponent<Rigidbody>().drag;
        oldAngularDrag = GetComponent<Rigidbody>().angularDrag;
    }

    public int getMoneyAmount()
    {
        return moneyAmount;
    }

    public void setMoneyAmout(int amount)
    {
        moneyAmount = amount;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "MoneyBlockVolume")
        {
            gameObject.transform.position = GameObject.Find("SpawnPositions/MoneySpawnPosition").GetComponent<Transform>().position;

            GameObject.Find("Rigidbody dragger").GetComponent<SpringJoint>().connectedBody.drag = oldDrag;
            GameObject.Find("Rigidbody dragger").GetComponent<SpringJoint>().connectedBody.angularDrag = oldAngularDrag;
            GameObject.Find("Rigidbody dragger").GetComponent<SpringJoint>().connectedBody = null;

        }
    }
}
