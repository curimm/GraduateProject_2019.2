using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainableObjectControl : MonoBehaviour
{
    [SerializeField]
    private int enduranceCount;

    [SerializeField]
    private GameObject dropObject;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DropObject()
    {
        Instantiate(dropObject, transform.position + new Vector3(0, 2, 0), transform.rotation);
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if("Player" == collision.gameObject.tag)
        {
            --enduranceCount;
        }

        if(enduranceCount <= 0)
        {
            DropObject();
        }
    }
}
