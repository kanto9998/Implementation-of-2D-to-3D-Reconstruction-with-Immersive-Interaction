using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game_Mgr
{
    private static Game_Mgr instance;


    public static Game_Mgr Instance
    {
        get
        {
            if( null == instance )
            {
                instance = new Game_Mgr();

                //정보 초기화
                instance.imageName = "test3";
                instance.gameSort = -1;
                instance.game3Sort = 0;
                instance.gamePlay = false;
            }
            return instance;
        }
    }

    //변수 선언
    public string       imageName;
    public int          gameSort;           //0:공게임, 1:고양이게임, 2:가구배치
    public int          game3Sort;          //0:의자, 1:쇼파, 2:책상

    public bool         gamePlay;           //플레이중 [ 공게임에서 공을 던졌을때 ]

    public Vector2      vecMousePos;        //마우스 포지션

    //함수 선언
}
