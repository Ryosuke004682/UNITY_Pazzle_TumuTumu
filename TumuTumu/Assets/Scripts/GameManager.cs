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

    private int gameScore; //�Q�[���X�R�A�̕ۑ��ϐ�


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
    /// Player�̓���(�^�b�`)����
    /// </summary>
    private void PlayerInput()
    {
        if(Input.GetMouseButtonUp(0))
        {
            //screen���W���烏�[���h���W�ɕϊ�
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(worldPoint , Vector2.zero);

            //�폜���ꂽ�A�C�e��������
            bubbles.RemoveAll(item => item == null);

            if(hit2D)
            {
                GameObject obj = hit2D.collider.gameObject;
                DeleteItems(obj);
            }
        }
    }


    /// <summary>
    /// �A�C�e���̐F�������_���Ɏw�肵�āA�w�肵���A�C�e���������_���Ȉʒu�ɐ�������
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

            //�����f�[�^�̒ǉ�
            bubbles.Add(bubble);
        }
    }

    /// <summary>
    /// �����Ɠ����F�̃A�C�e�����폜����
    /// </summary>
    /// <param name="target"></param>
    private void DeleteItems(GameObject target)
    {
        List<GameObject> checkItems = new List<GameObject>(); 

        checkItems.Add(target);

        //TODO : �S�̂̃A�C�e�����瓯���F�𑖍�����


        if (checkItems.Count < deleteCount) return;
        
        List<GameObject> destroyItems = new List<GameObject>();
        
        foreach(GameObject item in checkItems)
        {
            //���Ԃ�Ȃ��̍폜�����A�C�e�����J�E���g
            if (!destroyItems.Contains(item))
            {
                destroyItems.Add(item);
            }

            Destroy(item);
        }

        //�폜�������̉��Z����
        SpawnItem(destroyItems.Count);
        gameScore += destroyItems.Count * 100;

        //�X�R�A�\��
        textGameScore.text = "" + gameScore;

    }

}
