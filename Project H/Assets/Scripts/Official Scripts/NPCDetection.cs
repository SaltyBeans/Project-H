using System.Collections;
using UnityEngine;

public class NPCDetection : MonoBehaviour
{

    public Color detectionColor;
    public Material detectionMaterial;
    private Color defaultColor;

    [SerializeField]
    private LineRenderer ray;
    RaycastHit hit;

    public bool moneyFound;

    public bool endWave;

    ArrayList foundMoney = new ArrayList();

    Vector3 resetRay = Vector3.zero;

    public GameObject moneyLook;

    void Awake()
    {
        moneyFound = false;

        defaultColor = detectionMaterial.color;
        detectionColor.a = defaultColor.a;
    }

    void Update()
    {
        if (!moneyFound)
        {
            detectionMaterial.color = detectionColor;


            foreach (GameObject money in foundMoney)
            {
                Vector3 relativeMoneyPos = money.transform.position - transform.position;
                if (Physics.Raycast(transform.position, relativeMoneyPos, out hit))
                {
                    if (hit.collider.tag == "Money") //At least one of the money in visual detection volume is visible.
                    {
                        moneyLook = money; //Set moneyLook to official to look at.
                        moneyFound = true;
                        Vector3[] pos = new Vector3[] { gameObject.transform.position, hit.point };

                        ray.SetPositions(pos);
                        break;
                    }
                    if (hit.collider.tag != "Money")
                        moneyFound = false;

                }
            }

        }

        else
        {
            detectionMaterial.color = defaultColor;
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
