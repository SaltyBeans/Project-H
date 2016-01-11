using UnityEngine;
using System.Collections;

public class CoffeeTableScript : MonoBehaviour
{

    public float shelfReach = 0.8f;

    [SerializeField]
    private GameObject[] drawers = new GameObject[4];

    private Vector3[] initPos = new Vector3[4];

    void Start()
    {
        int drawerCount = 0;

        foreach (GameObject item in drawers)
        {
            initPos[drawerCount] = item.GetComponent<Transform>().position;
            drawerCount++;
        }
    }


    void LateUpdate()
    {

        int drawerCount = 0;

        foreach (GameObject item in drawers) //Normally, this would be solved with Animation but I need to Bodge this so this only works in x-axis.
        {
            item.GetComponent<Rigidbody>().position = new Vector3(Mathf.Clamp(item.GetComponent<Rigidbody>().position.x, initPos[drawerCount].x - shelfReach, initPos[drawerCount].x + shelfReach), item.GetComponent<Rigidbody>().position.y, item.GetComponent<Rigidbody>().position.z);

            drawerCount++;
        }


    }
}
