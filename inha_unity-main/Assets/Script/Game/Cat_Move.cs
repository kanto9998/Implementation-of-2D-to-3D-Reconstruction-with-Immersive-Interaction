using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Move : MonoBehaviour
{
    public GameObject       objCat;
    public GameObject       objCat_Base;

    private int             nMoveState;
    private float           fSpeed = 10.0f;

    private bool            bCoroutine_Play = false;
    private bool            bJump_Delay = false;

    private Coroutine       moveCoroutine;

    void Start()
    {
        bJump_Delay = false;
    }

    private void FixedUpdate()
    {
        bool bMove = false;

        //ก่
        if (Input.GetKey(KeyCode.W))
        {
            Coroutine_Stop();
            Cat_Move_Util( 0 );

            bMove = true;
        }

        //ก้
        if (Input.GetKey(KeyCode.S))
        {
            Coroutine_Stop();
            Cat_Move_Util( 1 );

            bMove = true;
        }

        //ก็
        if (Input.GetKey(KeyCode.A))
        {
            Coroutine_Stop();
            Cat_Move_Util( 2 );

            bMove = true;
        }

        //กๆ
        if (Input.GetKey(KeyCode.D))
        {
            Coroutine_Stop();
            Cat_Move_Util( 3 );

            bMove = true;
        }

        if ( Input.GetKey(KeyCode.Space) && bJump_Delay == false )
        {
            objCat.GetComponent<Rigidbody>().AddForce(Vector3.up * 15, ForceMode.Impulse);

            StartCoroutine( Run_Jump_Delay() );
        }

        if( bMove == false && bCoroutine_Play == false )
        {
            moveCoroutine = StartCoroutine( Run_Random_Move() );
        }
    }

    IEnumerator Run_Jump_Delay()
    {
        bJump_Delay = true;

        yield return new WaitForSeconds( 3f );

        bJump_Delay = false;
    }

    void Update()
    {
        
    }

    private void Coroutine_Stop()
    {
        bCoroutine_Play = false;
        //StopAllCoroutines();
        StopCoroutine( moveCoroutine );
    }

    private void Cat_Move_Util(int nSort)
    {
        float moveZ = 0f;
        float moveX = 0f;
        switch ( nSort )
        {
            case 0:
                moveZ += 1f;
                objCat.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                moveZ -= 1f;
                objCat.transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case 2:
                moveX -= 1f;
                objCat.transform.localRotation = Quaternion.Euler(0, 270, 0);
                break;
            case 3:
                moveX += 1f;
                objCat.transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
        }


        objCat_Base.transform.Translate(new Vector3(moveX, 0f, moveZ) * fSpeed * Time.deltaTime);
    }

    IEnumerator Run_Random_Move()
    {
        bCoroutine_Play = true;

        nMoveState = Random.Range( 0, 4 );

        for(int i = 0; i < 300; i++)
        {
            Cat_Move_Util( nMoveState );

            yield return new WaitForSeconds( 0.008f );
        }

        bCoroutine_Play = false;

        if ( Game_Mgr.Instance.gamePlay == true )
        {
            moveCoroutine = StartCoroutine( Run_Random_Move() );
        }
    }
}
