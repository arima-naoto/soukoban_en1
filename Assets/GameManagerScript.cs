using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    int[] map;

   

    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0 };
      
    }


    // Update is called once per frame
    void Update()
    {
        int playerIndex = -1;

        //右キーを押している場合
        if (Input.GetKeyDown(KeyCode.RightArrow)){
        
            //要素数はmap.Lenghtで取得
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }

            //playerIndex + 1のインデックスのものと交換するので、
            //playerIndex - 1よりさらに小さいインデックスの時
            //のみの交換処理を行う
            if (playerIndex < map.Length - 1)
            {
                map[playerIndex + 1] = 1;
                map[playerIndex] = 0;
            }

            string debugText = "";

            for (int i = 0; i < map.Length; i++)
            {
                debugText += map[i].ToString() + ",";
            }
            Debug.Log(debugText);

        }

        //左キーを押している場合
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //要素数はmap.Lenghtで取得
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1)
                {
                    playerIndex = i;
                    break;
                }
            }

            //playerIndex + 1のインデックスのものと交換するので、
            //playerIndex - 1よりさらに小さいインデックスの時
            //のみの交換処理を行う
            if (playerIndex > 0)
            {
                map[playerIndex - 1] = 1;
                map[playerIndex] = 0;
            }

            string debugText2 = "";

            for (int i = 0; i < map.Length; i++)
            {
                debugText2 += map[i].ToString() + ",";
            }
            Debug.Log(debugText2);
        }
    }
    
}
