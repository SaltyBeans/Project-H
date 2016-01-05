using UnityEngine;
using System.Collections;

public class OfficiaDisabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Official")
        {
            other.gameObject.SetActive(false);
        }
    }


}
