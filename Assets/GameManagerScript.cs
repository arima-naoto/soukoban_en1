using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;

    //レベルデザイン用の配列
    int[,] map; 

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]
        {
            { 0,0,0,0,0 },
            { 0,0,1,0,0 },
            { 0,0,0,0,0 },
        };

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[x,y] == 1)
                {
                    GameObject instance = Instantiate(
                        playerPrefab, 
                        new Vector3(x, map.GetLength(0) - y,0), 
                        Quaternion.identity
                        );
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
