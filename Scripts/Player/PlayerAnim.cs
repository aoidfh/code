using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class PlayerAnim : MonoBehaviour
{
    //플레이어
    public PlayerCtrl player;
    //플레이어 무기
    public PlayerWeapon playerWepon;

    //플레이어 이미지
    SpriteRenderer sprite;

    //플레이어 애니메이터
    Animator anim;

    private GameObject playerAnimObj;    

    public SpriteRenderer Sprite { get => sprite; set => sprite = value; }
    public GameObject PlayerAnimObj { get => playerAnimObj; set => playerAnimObj = value; }
    public Animator Anim { get => anim; set => anim = value; }

    void Start()
    {       
        Sprite = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();    
    }
   
    //플레이어 이동 애님 키고 끄기
    public void AnimMove(int num)
    {       
        switch (num)
        {
            case -1:
                Anim.SetBool("isMove", true);
                Sprite.flipX = false;
                break;
            case 0:
                Anim.SetBool("isMove", false);
                break;
            case 1:
                Anim.SetBool("isMove", true);
                Sprite.flipX = true;
                break;
        }
    }
    
    //공격 애님 실행
    public void AnimAttack(bool isAttack)
    {
        Anim.SetBool("isAttack", isAttack);
    }      
    
    //공격 애님이 끝나는 지점에서 실행
    public void PlayerCancelAttack()
    {
        playerWepon.CancelAttack();        
    }

    //사다리 애님 실행(움직이지 않으면 애니메이션 정지)
    public void PlayerLadderAnim(bool isLadder, float vertical)
    {
        if (vertical != 0)
        {
            Anim.speed = 1;            
        }
        else
        {
            Anim.speed = 0;          
        }

        Anim.SetBool("isLadder", isLadder);         
    }

    public void PlayerLadderAnim(bool isLadder)
    {       
        Anim.SetBool("isLadder", isLadder);
    }

    //죽음 애님 실행
    public void PlayerDeath()
    {
        Anim.SetBool("isDeath", true);
    }
}
