using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[] map;

    void PrintArray() 
    {
        string debugText = "";
        for(int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ", ";
        }
        Debug.Log(debugText);
    }

    int GetPlayerIndex() 
    {
        for(int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }

    bool MoveNumber(int number,int moveForm,int moveTo)
    {
        //移動先が範囲外なら移動不可
        if(moveTo< 0 || moveTo>= map.Length)
        {
            return false;
        }
        //移動先に2(箱)がいたら
        if (map[moveTo] == 2)
        {
            //どの方向へ移動するかを算出
            int velocity = moveTo - moveForm;

            //プレイヤーの移動先から、更に先へ2(箱)を移動させる。
            //箱の移動処理。MoveNumberめぞっど内でMoveNumberメゾットを
            //呼び、処理が再帰している。移動可不可をboolで記録
            bool succese = MoveNumber(2, moveTo, moveTo + velocity);

            //もし箱が移動失敗したら、プレイヤーの移動も失敗
            if (!succese)
            {
                return false;
            }
        }

        //プレイヤー・箱関わらずの移動処理
        map[moveTo] = number;
        map[moveForm] = 0;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] {2, 0, 0, 1, 0, 0, 0, 2, 2 };
        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = GetPlayerIndex();

            //移動処理を関数化
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex2 = GetPlayerIndex();

            MoveNumber(1, playerIndex2, playerIndex2 - 1);
            PrintArray();
        }
    }
}
