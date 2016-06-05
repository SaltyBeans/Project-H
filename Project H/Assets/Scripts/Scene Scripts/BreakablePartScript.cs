using UnityEngine;
using System.Collections;

public class BreakablePartScript : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRot;
    
	void Start ()
	{
	    initialPos = transform.position;
	    initialRot = transform.rotation;
	}
	
    public bool GetBreakablePartStatus()
    {
        return !GetComponent<Rigidbody>().isKinematic;   
    }

    public void RepairPart()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
