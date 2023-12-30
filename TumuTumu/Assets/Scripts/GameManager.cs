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

    private int gameScore; //�Q�[���X�R�A�̕ۑ��ϐ�


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

        //�Q�[���I��
        if(0 > gameTimer)
        {
            panelGameResult.SetActive(true);
            enabled = false;
            return;
        }

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
                CheckItems(obj);
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
    private void DeleteItems(List<GameObject> checkItems )
    {
        if (checkItems.Count < deleteCount) return;

        //�{�[�i�X�Ƃ��āA�I�[�o�[�����J�E���g*5����
        int overCountBonus = checkItems.Count - deleteCount;
        overCountBonus *= 5;

        //�����_���ȃA�C�e���̍폜
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

        audioSource.PlayOneShot(seBubble);
    }


    /// <summary>
    /// �����F�̃A�C�e����Ԃ�
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    List<GameObject> GetSameItems(GameObject target)
    {
        List<GameObject> ret = new List<GameObject>();

        foreach (GameObject item in bubbles)
        {
            /*�A�C�e���������A�����A�C�e���A�Ⴄ�F�A�����������ꍇ�X�L�b�v*/

            Sprite SR_item   = item  .GetComponent<SpriteRenderer>().sprite;
            Sprite SR_target = target.GetComponent<SpriteRenderer>().sprite;

            //���̃A�C�e���܂ł̋������i�[
            float distance = Vector2.Distance(target.transform.position, item.transform.position);

            if (!item || target == item) continue;
            if (SR_item != SR_target)    continue;
            if (distance > 1.55f)        continue;

            ret.Add(item);
        }

        return ret;
    }


    /// <summary>
    /// �����Ɠ����A�C�e����T��
    /// </summary>
    /// <param name="target"></param>
    private void CheckItems(GameObject target)
    {
        List <GameObject> checkItems = new List<GameObject>();
        checkItems.Add(target);

        int checkIndex = 0; //�`�F�b�N�ς݂̃C���f�N�X��

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
