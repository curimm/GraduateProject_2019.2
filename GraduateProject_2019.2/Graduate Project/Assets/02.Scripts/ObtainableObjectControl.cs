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

    // enduracne가 0이 되었을 때 이 객체를 파괴하고, 실제 획득할 수 있는 아이템을 만들어 준다.
    void DropObject()
    {
        Instantiate(dropObject, transform.position + new Vector3(0, 2, 0), transform.rotation);
        Destroy(this.gameObject);
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    print("1");
    //    if("Axe" == collision.gameObject.tag)
    //    {

    //        print(collision.gameObject.tag);
    //        --enduranceCount;

    //        if (enduranceCount <= 0)
    //        {
    //            DropObject();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if ("Axe" == other.gameObject.tag)
        {
            print(other.gameObject.tag);
            --enduranceCount;

            if (enduranceCount <= 0)
            {
                DropObject();
            }
        }
    }
}
