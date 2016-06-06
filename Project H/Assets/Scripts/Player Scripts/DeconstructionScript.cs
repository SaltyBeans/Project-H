using UnityEngine;
using System.Collections;

public class DeconstructionScript : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "breakable")
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.GetComponent<Rigidbody>().AddExplosionForce(1,other.transform.position,0.1f);
        }
    }
}
