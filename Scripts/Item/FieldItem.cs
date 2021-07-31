using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class FieldItem : MonoBehaviour
{           
    //이미지
    SpriteRenderer sprite;
    //콜라이더
    new Collider2D collider;
    //아이템고유번호
    int itemID;
    //아이템 수직 이동
    float moveFlag;

    [SerializeField]
    //생성된 아이템 시간(안먹으면 삭제하기 위함)
    float onFieldItemTime;
       
    void Start()
    {       
        sprite = GetComponent<SpriteRenderer>();        
        collider = GetComponent<Collider2D>();

        CheatKey itemPos = GameManager.instance.cheatKey;

        if (itemPos.Cheat)
        {
            //치트 사용시 무기, 방어구 생성
            switch (itemPos.ItemNum)
            {
                case 0:
                    itemID = 1002;
                    itemPos.ItemNum++;
                    break;

                case 1:
                    itemID = 1003;
                    itemPos.ItemNum = 0;

                    itemPos.Cheat = false;
                    break;
            }
        }
        else
        {
            //소비아이템이 생성될 확률 70%, 장비아이템이 생성될 확률 30%
            int itemRand = Random.Range(1, 10 + 1);

            if (itemRand <= 7)
            {
                itemID = 1001;
            }
            else
            {
                int emtRand = Random.Range(1002, 1003 + 1);

                itemID = emtRand;
            }

        }

        //필드 아이템 이미지 생성
        sprite.sprite = Resources.Load(itemID.ToString(), typeof(Sprite)) as Sprite;
       
        //수직이동 코루틴 시작
        StartCoroutine("ItemVerticalMove");
    }

    void Update()
    {        
        onFieldItemTime -= Time.deltaTime;
        
        //생성되고 시간이 지나고 아이템을 안먹으면 아이템 삭제
        if (onFieldItemTime <= 0)
        {
            FiledItemDestory();
        }
       
        //아이템 수직 이동
        switch (moveFlag)
        {
            case 1:
                transform.position += new Vector3(0, 0.1f, 0) * Time.deltaTime;
                break;
            case 2:
                transform.position -= new Vector3(0, 0.1f, 0) * Time.deltaTime;
                break;
        }
    }

    //1초마다 이동방향을 바꿔줌
    IEnumerator ItemVerticalMove()
    {
        if (moveFlag == 1)
        {
            moveFlag = 2;
        }
        else
        {
            moveFlag = 1;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine("ItemVerticalMove");
    }
   
    //플레이어와 닿으면 필드아이템 제거(아이템이 먹어짐)
    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.gameObject.CompareTag("PlayerCollider"))
        {                                                       
            //필드아이템을 먹기전 인벤토리에 남은 칸이 없다면 필드의 아이템이 사라지지않게 실행
            if (GameManager.instance.inventory.InvenItemFull)
            {
                return;
            }

            //필드아이템 이미지(itemID) 전달
            GameManager.instance.inventory.GetFieldItem(itemID);
            
            //아이템 먹는 소리 실행
            GameManager.instance.AudioSource(gameObject);

            //아이템 콜라이더 끔
            collider.enabled = false;

            //아이템을 먹으면 아이템을 투명하게 만듦(소리 실행후 삭제하기 위함)
            sprite.color = new Color(255, 255, 255, 0);           
            //필드아이템 삭제
            Invoke("FiledItemDestory", 1f);
        }
    }

    //필드아이템 삭제
    void FiledItemDestory()
    {
        Destroy(this.gameObject);
    }
}
