using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public GameObject playerPrefab; //プレイヤープレハブ

    public GameObject boxPrefab;//ボックスプレハブ

    public GameObject goalPrefab;//ゴールプレハブ

    public GameObject particlePrefab; //パーティクルプレハブ

    public GameObject notMoveBoxPrefab;//動かない壁のプレハブ

    public GameObject ClearText; //クリアテキスト
    public GameObject BottomText;

    int[,] map; //レベルデザイン用配列

    GameObject[,] field;//ゲーム管理用配列

    GameObject instance;//初期化用変数

    public string nextSceneName;//シーン切り替え処理

    void GenerateParticles(Vector3 position) //パーティクルを複数個生成するメゾット
    {
        for (int i = 0; i < 10; i++)
        {
            //パーティクルの生成
            instance = Instantiate(particlePrefab, position, Quaternion.identity);
        }
    }

    bool IsClear()//クリア判定を行うメゾット
    {
        //Vector2Int型可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    //格納芭蕉のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        // 格納場所に箱があるか調べる
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject goal = field[goals[i].y, goals[i].x];   // ゴールの座標に何があるかとってくる

            if (goal == null || goal.tag != "Box")
            {
                return false;
            }
        }

        return true;
    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)//再帰処理を行うメゾット
    {
        //二次元配列に対応させる
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        //NotMoveBoxタグを持っているならば
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "NotMoveBox")
        {
            //プレイヤーは動かない壁から進めない、また進むことが出来ないのでパーティクルも出ない
            return false;
        }

        //パーティクルを生成する位置を設定する
        Vector3 particlePosition = new Vector3(moveTo.x, -1 * moveTo.y, 0);

        //パーティクルを生成する
        GenerateParticles(particlePosition);

        //Boxタグを持っているならば再帰処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;// 移動方向を計算する
            bool success = MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        Move move = field[moveTo.y, moveTo.x].GetComponent<Move>();
        move.MoveTo(new Vector3(moveTo.x, -1 * moveTo.y, 0));

        return true;
    }

    Vector2Int GetPlayerIndex()//Playerを判断するメゾット
    {
        //二重for分を使用し、fieldの要素数で処理を回していく
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //プレイヤーtagを持っていたら
                if (field[y, x] != null && field[y, x].tag == "Player")
                {
                    // プレイヤーを見つけた
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);  // 見つからなかった
    }

    void Start()
    {

        //フィールドマップの宣言
        map = new int[,]
        {
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
            {4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 0, 4, 4, 4, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 4, 0, 0, 2, 0, 3, 0, 4, 0, 4, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 3, 4, 0, 4, 4, 4, 0, 4, 0, 4, 0, 3, 0, 0, 2, 0, 4},
            {4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 3, 4, 0, 0, 0, 0, 4, 0, 4},
            {4, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 4},
            {4, 0, 2, 0, 0, 0, 3, 4, 4, 0, 0, 4, 0, 0, 0, 2, 0, 4, 0, 4},
            {4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 3, 4},
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
        };

        //ゲームクリア時にテキストを描画したいので、今はfalseにしておく
        ClearText.SetActive(false);
        BottomText.SetActive(false);

        //ゲームオブジェクトの管理を行う
        field = new GameObject
        [
            map.GetLength(0),
            map.GetLength(1)
        ];

        //二重forを使用し、mapの要素数で処理を回していく
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //マップ番号が [1] である場合
                if (map[y, x] == 1)
                {
                    //マップ番号 [1] の所にプレイヤーを生成する
                    instance = Instantiate(playerPrefab, new Vector3(x, -1 * y, 0), Quaternion.identity);

                    //プレイヤーをfieldへ格納する
                    field[y, x] = instance;
                }
                //マップ番号が [2] である場合
                else if (map[y, x] == 2)
                {
                    //マップ番号 [2] の所にボックスを生成する
                    instance = Instantiate(boxPrefab, new Vector3(x, -1 * y, 0), Quaternion.identity);

                    //ボックスをfieldへ格納する
                    field[y, x] = instance;
                }
                //マップ番号が [3] である場合
                else if (map[y, x] == 3)
                {
                    //マップ番号 [3] の所にボックスの格納場所を生成する(fieldには格納しない)
                    instance = Instantiate(goalPrefab, new Vector3(x, -1 * y, 0.01f), Quaternion.identity);
                }
                //マップ番号が [4] である場合
                else if (map[y, x] == 4)
                {
                    //マップ番号 [4] の所に動かない壁を生成する
                    instance = Instantiate(notMoveBoxPrefab, new Vector3(x, -1 * y, 0), Quaternion.identity);

                    //動かない壁をfieldへ格納する
                    field[y, x] = instance;
                }
            }
        }
    }

    void Update()
    {
        //右矢印キーが入力された瞬間
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //playerIndexにPlayerを判断するメゾットを格納する
            Vector2Int playerIndex = GetPlayerIndex();

            //プレイヤーを右方向に移動させる
            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));

            //Boxを全て格納場所におけたら
            if (IsClear())
            {
                //ゲームクリア用のテキストを出現させる
                ClearText.SetActive(true);
                BottomText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //playerIndexにPlayerを判断するメゾットを格納する
            Vector2Int playerIndex = GetPlayerIndex();

            //プレイヤーを左方向に移動させる
            MoveNumber(playerIndex, playerIndex + new Vector2Int(-1, 0));

            //Boxを全て格納場所におけたら
            if (IsClear())
            {
                //ゲームクリア用のテキストを出現させる
                ClearText.SetActive(true);
                BottomText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //playerIndexにPlayerを判断するメゾットを格納する
            Vector2Int playerIndex = GetPlayerIndex();

            //プレイヤーを上方向に移動させる
            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, -1));

            //Boxを全て格納場所におけたら
            if (IsClear())
            {
                //ゲームクリア用のテキストを出現させる
                ClearText.SetActive(true);
                BottomText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //playerIndexにPlayerを判断するメゾットを格納する
            Vector2Int playerIndex = GetPlayerIndex();

            //プレイヤーを下方向に移動させる
            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));

            //Boxを全て格納場所におけたら
            if (IsClear())
            {
                //ゲームクリア用のテキストを出現させる
                ClearText.SetActive(true);
                BottomText.SetActive(true);
            }
        }

        //Rキーを押したら
        if (Input.GetKeyDown(KeyCode.R))
        {
            //ゲームをリセットする
            SceneManager.LoadScene(1);
        }

        //クリア時
        if (IsClear())
        {
            //スペースキーが押されたら
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //シーンをタイトルに変更する
                SceneManager.LoadScene(nextSceneName);
            }

        }
    }
}
