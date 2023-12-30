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

    private int gameScore;


    private void Start( )
    {
        bubbles = new List<GameObject>();
        panelGameResult.SetActive(false);

        SpawnItem(fieldItemCountMax);
    }

    private void Update( )
    {
        
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

}
