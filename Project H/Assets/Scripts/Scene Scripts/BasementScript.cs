using UnityEngine;
using System.Collections;

public class BasementScript : MonoBehaviour
{
    private bool isLocked;
    private bool isOpen;
    private Vector3 initialPos;
    private Quaternion initialRot;
    
	void Start ()
	{
	    initialPos = transform.localPosition;
	    initialRot = transform.localRotation;
	    isOpen = false;
	    isLocked = true;
	}

    public bool getLockStatus()
    {
        return isLocked;
    }

    public bool getOpenStatus()
    {
        return isOpen;
    }

    public void unlockBasement()
    {
        isLocked = false;
    }

    public void OpenCloseHatch()
    {
        if (isOpen)
        {
            transform.localPosition = initialPos;
            transform.localRotation = initialRot;
            isOpen = false;
        }
        else
        {
            transform.localPosition=new Vector3(0.33f,1.33f,0);
            transform.localRotation=Quaternion.Euler(0,0,-45);
            isOpen = true;
        }
    }
}
