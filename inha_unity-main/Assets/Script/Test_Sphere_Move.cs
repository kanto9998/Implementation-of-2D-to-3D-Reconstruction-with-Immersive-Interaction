using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Sphere_Move : MonoBehaviour
{
    private float fSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float moveZ = 0f;
        float moveX = 0f;

        //ก่
        if (Input.GetKey(KeyCode.W))
        {
            moveZ += 1f;
        }

        //ก้
        if (Input.GetKey(KeyCode.S))
        {
            moveZ -= 1f;
        }

        //ก็
        if (Input.GetKey(KeyCode.A))
        {
            moveX -= 1f;
        }

        //กๆ
        if (Input.GetKey(KeyCode.D))
        {
            moveX += 1f;
        }

        if( Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 2, ForceMode.Impulse);
        }

        this.transform.Translate( new Vector3( moveX, 0f, moveZ ) * fSpeed * Time.deltaTime );
        //this.transform.Translate(new Vector3(moveX, 0f, moveZ).normalized * 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
