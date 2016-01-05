using UnityEngine;
using System.Collections;

public class OfficialTrigger : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Official")
        {
            other.GetComponent<AIControl>().inspectionComplete = true;
        }
    }
}
