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

                //���� �ʱ�ȭ
                instance.imageName = "test3";
                instance.gameSort = -1;
                instance.game3Sort = 0;
                instance.gamePlay = false;
            }
            return instance;
        }
    }

    //���� ����
    public string       imageName;
    public int          gameSort;           //0:������, 1:����̰���, 2:������ġ
    public int          game3Sort;          //0:����, 1:����, 2:å��

    public bool         gamePlay;           //�÷����� [ �����ӿ��� ���� �������� ]

    public Vector2      vecMousePos;        //���콺 ������

    //�Լ� ����
}
