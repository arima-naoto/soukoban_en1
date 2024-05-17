using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;

    public GameObject ClearText;
    int[,] map;
    GameObject[,] field;
    GameObject instance;

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
            return false;

        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
            return false;

        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        //field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, -1 * moveTo.y, 0);
        Vector3 moveToPosition = new Vector3(moveTo.x, -1 * moveTo.y, 0);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                GameObject obj = field[y, x];

                if (obj != null && obj.tag == "Player")
                {
                    return new Vector2Int(x, y);
                } 
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool IsClear()
    {
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int  y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y,x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int  i = 0; i < goals.Count; i++)
        {
            GameObject result = field[goals[i].y, goals[i].x];
            if(result == null || result.tag != "Box")
            {
                return false;
            }
        }

        return true;
    }

   
    void Start()
    {
        Screen.SetResolution(1280, 720, false);

        map = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 3, 0, 3, 0, 0, 0, 0, 0 },
            { 0, 0, 2, 0, 0, 1, 0, 0, 0 },
            { 0, 2, 3, 2, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      
        };

        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)
        ];

        ClearText.SetActive(false);
  

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    instance =
                        Instantiate(playerPrefab, new Vector3(x, -1 * y, 0), Quaternion.identity);
                    field[y, x] = instance;
                    break;
                }
                else if (map[y,x] == 2)
                {
                    instance =
                        Instantiate(boxPrefab, new Vector3(x, -1 * y, 0), Quaternion.identity);
                    field[y, x] = instance;
                }else if (map[y,x] == 3)
                {
                    instance = Instantiate(goalPrefab, new Vector3(x,-1 * y, 0), Quaternion.identity);
                }
            }
        }
    }

    

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var playerIndex = GetPlayerIndex();
            MoveNumber(playerIndex, playerIndex + Vector2Int.right);

            if (IsClear())
            {
                ClearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var playerIndex = GetPlayerIndex();
            MoveNumber(playerIndex, playerIndex + Vector2Int.left);

            if (IsClear())
            {
                ClearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var playerIndex = GetPlayerIndex();
            MoveNumber(playerIndex, playerIndex - Vector2Int.up);

            if (IsClear())
            {
                ClearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var playerIndex = GetPlayerIndex();
            MoveNumber(playerIndex, playerIndex - Vector2Int.down);

            if (IsClear())
            {
                ClearText.SetActive(true);
            }
        }
    }

}
