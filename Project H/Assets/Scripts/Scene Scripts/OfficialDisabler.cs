using UnityEngine;
using System.Collections;

public class OfficialDisabler : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Official")
        {
            other.gameObject.SetActive(false);
        }
    }
}
