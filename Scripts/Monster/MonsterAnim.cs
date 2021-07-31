using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영
public class MonsterAnim : MonoBehaviour
{   
    //몬스터이미지
    SpriteRenderer sprite;
    //몬스터애니메이터
    Animator anim;

    //스프라이트 이름
    string animName;

    //필드아이템위치
    FieldItemPos fieldItemPos;
    
    public string AnimName { get => animName; }

    void Start()
    {       
        fieldItemPos = GameObject.Find("FieldItemPos").GetComponent<FieldItemPos>();       
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();        
    }

    void Update()
    {
        animName = sprite.sprite.name;
    }

    //num == (MonsterAI.moveFlag)
    public void AnimMove(int moveFlag)
    {       
        //sprite.flip(몬스터 이미지 뒤집기)      
        switch (moveFlag)
        {
            case -1:
                anim.SetBool("isMove", true);
                sprite.flipX = false;
                break;
            case 0:              
                anim.SetBool("isMove", false);
                break;
            case 1:
                anim.SetBool("isMove", true);
                sprite.flipX = true;
                break;
        }
    }           

    //피격 애니메이션 키고끄기
    public void AnimHit(bool isHit)
    {    
        anim.SetBool("isHit", isHit);       
    }  
    
    //데스 애니메이션 킴
    public void AnimDeath(bool isDeadth)
    {
        anim.SetBool("isDeath", isDeadth);
    }

    //몬스터 죽음
    public void MonsterDeath()
    {       
        //그 자리에 필드 아이템 생성
        fieldItemPos.FieldItemPosition(gameObject.transform.position);
        
        //퀘스트 수락이 되어있고, 해당 씬에 맞는 퀘스트의 몬스터를 죽여야 카운트 올라가게함  
        QuestInformation questInfo = GameManager.instance.questInfo;

        if (questInfo.QuestAccept && questInfo.CheckName)
        {         
            questInfo.ProduceKeyCount += 1;
        }
    }
}
