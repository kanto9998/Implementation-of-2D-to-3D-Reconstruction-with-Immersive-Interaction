using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Collider : MonoBehaviour
{
    public GameObject obj_GoalEffect;

    // Start is called before the first frame update
    void Start()
    {
        obj_GoalEffect.SetActive( false );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //∞Ò¿Œ √≥∏Æ
        if (other.gameObject.name == "GoalBox")
        {
            StartCoroutine( Goal_Effect() );
        }
    }

    IEnumerator Goal_Effect()
    {
        obj_GoalEffect.SetActive( true );

        yield return new WaitForSeconds( 1f );
        
        obj_GoalEffect.SetActive( false );
    }
}
