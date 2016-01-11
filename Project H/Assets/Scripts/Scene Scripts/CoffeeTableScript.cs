using UnityEngine;
using System.Collections;

public class CoffeeTableScript : MonoBehaviour
{

    public float shelfReach = 1.1f;

    [SerializeField]
    private GameObject[] drawers = new GameObject[4];

    private Vector3[] initPos = new Vector3[4];
    private Vector3[] initPosLocal = new Vector3[4];
    void Start()
    {
        int drawerCount = 0;

        foreach (GameObject item in drawers)
        {
            initPos[drawerCount] = item.GetComponent<Transform>().position;
            initPosLocal[drawerCount] = item.GetComponent<Transform>().localPosition;
            drawerCount++;
        }
    }


    void Update()
    {

        int drawerCount = 0;

        foreach (GameObject item in drawers) //Normally, this would be solved with Animation but I need to Bodge this so this only works in x-axis.
        {
            if (Vector3.Distance(initPos[drawerCount], item.GetComponent<Transform>().position) > shelfReach)
            {
                if (initPos[drawerCount].x - item.GetComponent<Transform>().position.x > 0)
                {
                    Vector3 pos = initPosLocal[drawerCount];
                    pos.x -= shelfReach;
                    item.GetComponent<Transform>().localPosition = pos;
                }
                else if (initPos[drawerCount].x - item.GetComponent<Transform>().position.x < 0)
                {
                    Vector3 pos = initPosLocal[drawerCount];
                    pos.x += shelfReach;
                    item.GetComponent<Transform>().localPosition = pos;
                }
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;

                //drawers[drawerCount].transform.position = initPos[drawerCount];
            }
            drawerCount++;
        }


    }
}
