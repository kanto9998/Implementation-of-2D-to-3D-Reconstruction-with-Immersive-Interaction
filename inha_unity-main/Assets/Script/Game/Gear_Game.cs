using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_Game : MonoBehaviour
{
    public GameObject   objGearCloneBase;

    public GameObject   objGear_Chair;
    public GameObject   objGear_Sofa;
    public GameObject   objGear_Table;

    private float   fSpeed = 10.0f;
    private float   fRotateSpeed = 80.0f;
    private bool    bCreate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        //ก่
        if (Input.GetKey(KeyCode.W))
        {
            Gear_Move_Util( 0 );
        }

        //ก้
        if (Input.GetKey(KeyCode.S))
        {
            Gear_Move_Util( 1 );
        }

        //ก็
        if (Input.GetKey(KeyCode.A))
        {
            Gear_Move_Util( 2 );
        }

        //กๆ
        if (Input.GetKey(KeyCode.D))
        {
            Gear_Move_Util( 3 );
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Gear_Move_Util( 4 );
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Gear_Move_Util( 5 );
        }

        if( Input.GetKey(KeyCode.Q) )
        {
            Gear_Rotate_Util( 3 );
        }
        
        if( Input.GetKey(KeyCode.E) )
        {
            Gear_Rotate_Util( 2 );
        }
        
        if( Input.GetKey(KeyCode.R) )
        {
            Gear_Rotate_Util( 1 );
        }
        
        if( Input.GetKey(KeyCode.F) )
        {
            Gear_Rotate_Util( 0 );
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if( bCreate == true ) {
                return;
            }

            StartCoroutine( Create_Delay() );

            switch( Game_Mgr.Instance.game3Sort )
            {
                case 0:     Gear_Clone_Create( objGear_Chair );     break;
                case 1:     Gear_Clone_Create( objGear_Sofa );      break;
                case 2:     Gear_Clone_Create( objGear_Table );     break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Create_Delay()
    {
        bCreate = true;

        yield return new WaitForSeconds( 0.2f );

        bCreate = false;
    }

    private void Gear_Clone_Create(GameObject originObj)
    {
        GameObject      obj = Instantiate( originObj, objGearCloneBase.transform ).transform.GetChild( 0 ).gameObject;

        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        obj.GetComponent<Rigidbody>().useGravity = true;
    }

    private void Gear_Move_Util(int nSort)
    {
        float moveZ = 0f;
        float moveX = 0f;
        float moveY = 0f;
        switch (nSort)
        {
            case 0:     moveZ += 1f;        break;
            case 1:     moveZ -= 1f;        break;
            case 2:     moveX -= 1f;        break;
            case 3:     moveX += 1f;        break;
            case 4:     moveY += 1f;        break;
            case 5:     moveY -= 1f;        break;
        }

        switch( Game_Mgr.Instance.game3Sort )
        {
            case 0:     objGear_Chair.transform.Translate(new Vector3(moveX, moveY, moveZ) * fSpeed * Time.deltaTime);       break;
            case 1:     objGear_Sofa.transform.Translate(new Vector3(moveX, moveY, moveZ) * fSpeed * Time.deltaTime);           break;
            case 2:     objGear_Table.transform.Translate(new Vector3(moveX, moveY, moveZ) * fSpeed * Time.deltaTime);       break;
        }
    }

    private void Gear_Rotate_Util(int nSort)
    {
        float RotateX = 0f;
        float RotateY = 0f;
        float RotateZ = 0f;
        switch (nSort)
        {
            case 0: RotateX -= 1f; break;
            case 1: RotateX += 1f; break;
            case 2: RotateY += 1f; break;
            case 3: RotateY -= 1f; break;
            case 4: RotateZ += 1f; break;
            case 5: RotateZ -= 1f; break;
        }
        
        switch( Game_Mgr.Instance.game3Sort )
        {
            case 0:     objGear_Chair.transform.Rotate(new Vector3(RotateX, RotateY, RotateZ) * fRotateSpeed * Time.deltaTime);           break;
            case 1:     objGear_Sofa.transform.Rotate(new Vector3(RotateX, RotateY, RotateZ) * fRotateSpeed * Time.deltaTime);            break;
            case 2:     objGear_Table.transform.Rotate(new Vector3(RotateX, RotateY, RotateZ) * fRotateSpeed * Time.deltaTime);           break;
        }
    }
}
