using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GameSettings
    [SerializeField] List<GameObject> prefabBubbles;
    [SerializeField] private float gameTimer;
    [SerializeField] private int fieldItemCountMax;
    [SerializeField] private int deleteCount;

    //UI
    [SerializeField] TextMeshProUGUI textGameScore;
    [SerializeField] TextMeshProUGUI textGameTimer;
    [SerializeField] GameObject panelGameResult;
    
    //Audio
    [SerializeField] AudioClip seBubble;
    [SerializeField] AudioClip seSpecial;
    AudioSource audioSource;

    List<GameObject> bubbles;

    private int gameScore; //ゲームスコアの保存変数


    private void Start( )
    {
        bubbles = new List<GameObject>();
        panelGameResult.SetActive(false);

        SpawnItem(fieldItemCountMax);
    }

    private void Update( )
    {
        PlayerInput();
    }


    /// <summary>
    /// Playerの入力(タッチ)処理
    /// </summary>
    private void PlayerInput()
    {
        if(Input.GetMouseButtonUp(0))
        {
            //screen座標からワールド座標に変換
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint , Vector2.zero);

            //削除されたアイテムを消す
            bubbles.RemoveAll(item => item == null);

            if(hit2D)
            {
                GameObject obj = hit2D.collider.gameObject;
                DeleteItems(obj);
            }
        }
    }


    /// <summary>
    /// アイテムの色をランダムに指定して、指定したアイテムをランダムな位置に生成する
    /// </summary>
    /// <param name="count"></param>
    private void SpawnItem(int count)
    {
        for(int item = 0; item < count; item++)
        {
            int rnd = Random.Range(0 , prefabBubbles.Count);

            float x = Random.Range(-2.0f, 2.0f);
            float y = Random.Range(-2.0f, 2.0f);

            GameObject bubble = 
                Instantiate
                (
                    prefabBubbles[rnd],
                    new Vector3(x , 7 + y , 0),
                    Quaternion.identity
                 );

            //内部データの追加
            bubbles.Add(bubble);
        }
    }

    /// <summary>
    /// 引数と同じ色のアイテムを削除する
    /// </summary>
    /// <param name="target"></param>
    private void DeleteItems(GameObject target)
    {
        List<GameObject> checkItems = new List<GameObject>(); 

        checkItems.Add(target);

        //TODO : 全体のアイテムから同じ色を走査する


        if (checkItems.Count < deleteCount) return;
        
        List<GameObject> destroyItems = new List<GameObject>();
        
        foreach(GameObject item in checkItems)
        {
            //かぶりなしの削除したアイテムをカウント
            if (!destroyItems.Contains(item))
            {
                destroyItems.Add(item);
            }

            Destroy(item);
        }

        //削除した分の加算処理
        SpawnItem(destroyItems.Count);
        gameScore += destroyItems.Count * 100;

        //スコア表示
        textGameScore.text = "" + gameScore;

    }

}
