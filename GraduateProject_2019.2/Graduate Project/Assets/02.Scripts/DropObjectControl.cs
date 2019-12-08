using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectControl : MonoBehaviour
{
    [SerializeField]
    private DropObjectType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 3, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ("Player" == collision.gameObject.tag)
        {
            PlayerControl playerControl = collision.gameObject.GetComponent<PlayerControl>();
            if (null == playerControl)
            {
                return;
            }
            playerControl.ObtainObject(type);
            Destroy(this.gameObject);
        }
    }
}
