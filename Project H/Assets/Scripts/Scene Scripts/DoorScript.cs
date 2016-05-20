using UnityEngine;

public class DoorScript : MonoBehaviour
{
    /// <summary>
    /// It's true if it's open.
    /// </summary>
    private bool DoorState = false;
    private Crosshair crosshair;
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

        crosshair = GameObject.FindGameObjectWithTag("Player").GetComponent<Crosshair>();

    }

    void Update()
    {
        if (crosshair.GetHit().collider != null && crosshair.GetHit().collider.gameObject == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (DoorState)
                    CloseDoor();

                else
                    OpenDoor();
            }

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

    /// <summary>
    /// Returns true if the door is open.
    /// </summary>
    /// <returns>Door's state, open or closed.</returns>
    public bool getState()
    {
        return DoorState;
    }
}
