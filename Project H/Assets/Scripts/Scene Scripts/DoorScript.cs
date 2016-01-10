using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    private bool DoorState = false;
    void Start()
    {
        if (name == "outsidedoor")
        {
            GetComponent<Transform>().rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            GetComponent<Transform>().rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    public void OpenDoor()
    {
        if (name == "outsidedoor")
        {
            GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);

            DoorState = true;
        }
        else
        {

            GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
            DoorState = true;
        }
    }
    public void CloseDoor()
    {
        if (name == "outsidedoor")
        {
            GetComponent<Transform>().rotation = Quaternion.Euler(0, -90, 0);
            DoorState = false;
        }
        else
        {
            GetComponent<Transform>().rotation = Quaternion.Euler(0, 90, 0);
            DoorState = false;
        }
    }
    public bool getState()
    {
        return DoorState;
    }
}
