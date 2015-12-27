using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    public Texture2D crosshair;
    public Rect pos;
    static bool OriginalOn = true;
    public bool CursorLock= false;
    public TextMesh infoText;
    RaycastHit hit;
    public Camera cam;
	void Start () {
        pos = new Rect((Screen.width - crosshair.width) / 2, (Screen.height - crosshair.height) / 2, crosshair.width, crosshair.height);
        infoText.text = null;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (CursorLock == true)
            {
                CursorLock = false;
            }
            else if(CursorLock == false)
            {
                CursorLock = true;
            }
        }
        if (hit.collider.tag == "door")
        {
            if (hit.collider.GetComponent<DoorScript>().getState() == true)
            {
                infoText.text = "Press [E] to Close";

               
            }
            else
            {
                infoText.text = "Press [E] to Open";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.GetComponent<DoorScript>().OpenDoor();
                }
            }
        }

        if (hit.collider.tag == "drawer")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.gameObject.GetComponent<Animator>().Play("drawerAnim");
            }
            
        }

    }
    void FixedUpdate()
    {
        if(Physics.Raycast ( cam.transform.position , cam.transform.forward , out hit ,5.0f) )
        {
            if (hit.collider.tag == "hideSpot")
            {
                    infoText.text = hit.collider.name + ": " + hit.collider.GetComponent<HidingSpot>().getAmount() +" / " +hit.collider.GetComponent<HidingSpot>().getMaxAmount()+"$";               
            }
            else if (hit.collider.tag == "Money")
            {
                infoText.text = hit.collider.GetComponent<MoneyScript>().getMoneyAmount() + "$";
            }
            else
            {
                infoText.text = null;
            }
        }
        else
        {
            infoText.text = null;
        }

    }

	void OnGUI () {
        if (CursorLock == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (CursorLock == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (OriginalOn == true)
        {
            GUI.DrawTexture(pos, crosshair);
        }
	}
}
