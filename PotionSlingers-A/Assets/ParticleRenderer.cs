using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRenderer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {     
        //GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerID = spriteRenderer.sortingLayerID;
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = -1;
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Foreground";
        Debug.Log("Sprite renderer???");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
