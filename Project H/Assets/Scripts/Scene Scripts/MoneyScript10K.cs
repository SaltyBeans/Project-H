using UnityEngine;

public class MoneyScript10K : MonoBehaviour
{
    private int moneyAmount;
    //Right now, the amount is set at 10.000$.



    float oldDrag;
    float oldAngularDrag;
    public readonly int moneyType = 10000;

    void Awake()
    {
        moneyAmount = 10000;
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

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, (amount / 10000f) * 4.5f, gameObject.transform.localScale.z);
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
