using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Move : MonoBehaviour
{
    public bool bKeyboard;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (bKeyboard == true)
        {
            if(Input.GetKey( KeyCode.Escape ))
            {
                SceneManager.LoadScene( "Select_Scene" );
            }
        }
    }


    public void SceneMove(string str)
    {
        SceneManager.LoadScene( str );
    }
}
