using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        audioSource = GetComponent<AudioSource>();
        bubbles = new List<GameObject>();
        panelGameResult.SetActive(false);

        SpawnItem(fieldItemCountMax);
    }


    private void Update( )
    {
        gameTimer -= Time.deltaTime;
        textGameTimer.text = "" + (int)gameTimer;

        //ゲーム終了
        if(0 > gameTimer)
        {
            panelGameResult.SetActive(true);
            enabled = false;
            return;
        }

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
                CheckItems(obj);
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
    private void DeleteItems(List<GameObject> checkItems )
    {
        if (checkItems.Count < deleteCount) return;

        //ボーナスとして、オーバーしたカウント*5個消す
        int overCountBonus = checkItems.Count - deleteCount;
        overCountBonus *= 5;

        //ランダムなアイテムの削除
        while (overCountBonus > 0)
        {
            int rnd = Random.Range(0 , bubbles.Count);
            checkItems.Add(bubbles[rnd]);
            overCountBonus--;


            if (overCountBonus == 0)
            {
                audioSource.PlayOneShot(seSpecial);
            }
        }


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

        audioSource.PlayOneShot(seBubble);
    }


    /// <summary>
    /// 同じ色のアイテムを返す
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    List<GameObject> GetSameItems(GameObject target)
    {
        List<GameObject> ret = new List<GameObject>();

        foreach (GameObject item in bubbles)
        {
            /*アイテムが無い、同じアイテム、違う色、距離が遠い場合スキップ*/

            Sprite SR_item   = item  .GetComponent<SpriteRenderer>().sprite;
            Sprite SR_target = target.GetComponent<SpriteRenderer>().sprite;

            //次のアイテムまでの距離を格納
            float distance = Vector2.Distance(target.transform.position, item.transform.position);

            if (!item || target == item) continue;
            if (SR_item != SR_target)    continue;
            if (distance > 1.55f)        continue;

            ret.Add(item);
        }

        return ret;
    }


    /// <summary>
    /// 引数と同じアイテムを探す
    /// </summary>
    /// <param name="target"></param>
    private void CheckItems(GameObject target)
    {
        List <GameObject> checkItems = new List<GameObject>();
        checkItems.Add(target);

        int checkIndex = 0; //チェック済みのインデクス数

        while(checkIndex < checkItems.Count)
        {
            List<GameObject> sameItems = GetSameItems(checkItems[checkIndex]);

            checkIndex++;

            foreach(GameObject item in sameItems)
            {
                if (checkItems.Contains(item)) continue;
                checkItems.Add(item);
            }
        }
        DeleteItems(checkItems);
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene("TumuTumuScene");

    }

}
