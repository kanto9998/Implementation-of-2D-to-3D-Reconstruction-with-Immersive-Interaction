using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Camera : MonoBehaviour
{
    public GameObject objBall;
    public GameObject objBallBase;

    public GameObject obj_GoalPost;

    public GameObject obj_Goal_Collider;
    public GameObject obj_Goal_RigidBody;

    private float fSpeed = 10.0f;
    public float rotateSpeed = 100;
    public Vector3 ballPower;

    // Start is called before the first frame update
    void Start()
    {
        objBall.GetComponent<Rigidbody>().useGravity = false;

        Game_Mgr.Instance.gamePlay = false;
    }

    private void FixedUpdate()
    {
        //↑
        if (Input.GetKey(KeyCode.W))
        {
            GoalPost_Move_Util(1);
        }

        //↓
        if (Input.GetKey(KeyCode.S))
        {
            GoalPost_Move_Util(0);
        }

        //←
        if (Input.GetKey(KeyCode.A))
        {
            GoalPost_Move_Util(3);
        }

        //→
        if (Input.GetKey(KeyCode.D))
        {
            GoalPost_Move_Util(2);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            GoalPost_Move_Util(4);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            GoalPost_Move_Util(5);
        }

    }


    // Update is called once per frame
    void Update()
    {
        switch( Game_Mgr.Instance.gameSort )
        {
            case 0:     Game_1_Play();      break;
        }

        if(Game_Mgr.Instance.gamePlay == true)
        {
            if (obj_GoalPost.GetComponent<Rigidbody>() != null)
            {
                StartCoroutine( RigidBody_Delete_Delay() );
            }
        }
        else if(Game_Mgr.Instance.gamePlay == false)
        {
            obj_Goal_RigidBody.SetActive( true );
            obj_Goal_Collider.SetActive( false );

            if( obj_GoalPost.GetComponent<Rigidbody>() == null )
            {
                obj_GoalPost.AddComponent<Rigidbody>();
                obj_GoalPost.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                obj_GoalPost.GetComponent<Rigidbody>().mass = 5;
                obj_GoalPost.GetComponent<Rigidbody>().useGravity = false;
                obj_GoalPost.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }

        }
    }

    IEnumerator RigidBody_Delete_Delay()
    {
        Destroy(obj_GoalPost.GetComponent<Rigidbody>());

        yield return new WaitForSeconds( 0.2f );

        obj_Goal_RigidBody.SetActive(false);
        obj_Goal_Collider.SetActive(true);
    }

    private void Game_1_Play()
    {
        if (Game_Mgr.Instance.gamePlay == true)
        {
            return;
        }

        Camera_move();

        //스페이스 [ 공 던지기 ]
        if (Input.GetKey(KeyCode.Space))
        {
            Game_Mgr.Instance.gamePlay = true;

            objBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            objBall.GetComponent<Rigidbody>().useGravity = true;
            objBall.GetComponent<Rigidbody>().AddForce(ballPower, ForceMode.Impulse);
        }
    }

    private void Camera_move()
    {
        objBall.transform.localPosition = new Vector3(0, 0, 0);

        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            50f );

        // 각 위치를 WorldPosition으로 변경
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);

        objBallBase.transform.position = new Vector3(worldMousePosFar.x, worldMousePosFar.y, worldMousePosFar.z);
    }

    private void GoalPost_Move_Util(int nSort)
    {
        if ( Game_Mgr.Instance.gamePlay == true ) {
            return;
        }

        float moveZ = 0f;
        float moveX = 0f;
        float moveY = 0f;
        switch (nSort)
        {
            case 0: moveZ += 1f; break;
            case 1: moveZ -= 1f; break;
            case 2: moveX -= 1f; break;
            case 3: moveX += 1f; break;
            case 4: moveY += 1f; break;
            case 5: moveY -= 1f; break;
        }

        obj_GoalPost.transform.Translate(new Vector3(moveX, moveY, moveZ) * fSpeed * Time.deltaTime);
    }
}
