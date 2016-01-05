using UnityEngine;
using System.Collections;

public class NPCDetection : MonoBehaviour
{

    public Color detectionColor;
    public Material detectionMaterial;
    private Color defaultColor;

    [SerializeField]
    private LineRenderer ray;
    RaycastHit hit;

    public bool moneyFound;

    ArrayList foundMoney = new ArrayList();

    Vector3 resetRay = Vector3.zero;

    void Awake()
    {
        moneyFound = false;

        defaultColor = detectionMaterial.color;
        detectionColor.a = defaultColor.a;
    }

    void Update()
    {

        foreach (GameObject money in foundMoney)
        {

            Vector3 relativeMoneyPos = money.transform.position - transform.position;

            if (Physics.Raycast(transform.position, relativeMoneyPos, out hit))
            {

                if (hit.collider.tag == "Money")
                {
                    moneyFound = true;
                    Vector3[] pos = new Vector3[] { gameObject.transform.position, hit.collider.gameObject.transform.position };

                    ray.SetPositions(pos);
                    break;
                }
                if (hit.collider.tag != "Money")
                {
                    moneyFound = false;
                }
            }
        }


        if (moneyFound == true)
        {
            detectionMaterial.color = detectionColor;
        }
        else
        {
            detectionMaterial.color = defaultColor;

            ray.SetPosition(0, resetRay);
            ray.SetPosition(1, resetRay);
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Money")
        {
            foundMoney.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Money")
        {
            moneyFound = false;
            foundMoney.Remove(other.gameObject);
        }

    }
}
