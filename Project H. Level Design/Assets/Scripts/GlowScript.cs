using UnityEngine;
using System.Collections;



public class GlowScript : MonoBehaviour
{
    public Camera cam;

    [SerializeField]
    private GameObject bufferShader;
    
    public string[] TagsToGlow;

    [SerializeField]
    private float maxGlowDistance;
    [SerializeField]
    private Color glowColor;

    Color normalGlowColor;
    void Start()
    {
        //cam = GetComponentInChildren<Camera>();
        normalGlowColor = new Color(0, 0, 0, 0);
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        
        if (Physics.Raycast(ray, out hit, maxGlowDistance))
        {
            if (tagExists(hit.collider.tag))    //Collided with money or hidingspot
            {
                if (hit.collider.gameObject != bufferShader)
                {   
                    bufferShader.GetComponent<Renderer>().material.SetColor("_RimColor", normalGlowColor);
                }
                
                hit.collider.GetComponent<Renderer>().material.SetColor("_RimColor", glowColor);
                bufferShader = hit.collider.gameObject;
            }
            else
            {
                bufferShader.GetComponent<Renderer>().material.SetColor("_RimColor", normalGlowColor);
            }
        }
        else
        {
            bufferShader.GetComponent<Renderer>().material.SetColor("_RimColor", normalGlowColor);
        }

    }

    bool tagExists(string objectTag)
    {
        foreach (string tag in TagsToGlow)
            if (tag == objectTag)
                return true;

        return false;
    }


}
