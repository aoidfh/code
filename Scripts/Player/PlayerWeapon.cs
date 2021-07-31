using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //플레이어
    public PlayerCtrl player;
    //몬스터 애니메이터
    public PlayerAnim playerAnim;   
    //무기 충돌체크
    public Collider2D weapon;
    //플레이어 다리
    public PlayerFoot playerFoot;
    //모바일용 터치매니져    
    public UI_TouchManager touchManager;

    //공격 지연시간
    float attackTime;
    //공격 리셋시간
    readonly float resetTime = 1f;

    //공격이 되었는지..(PC용)
    bool isAttack;

    void Start()
    {       
        //시작시 액티브 꺼짐
        weapon.enabled = false;

        attackTime = resetTime;
    }

    private void Update()
    {
        //플레이어가 이동시 그림체를 뒤집어주기 때문에 플레이어의 무기콜라이더와 위치가 안맞음(항상 위치 조정)
        if (playerAnim.Sprite.flipX)
        {
            weapon.offset = new Vector2(0.25f, -0.095f);
        }
        else
        {
             weapon.offset = new Vector2(-0.25f, -0.095f);
        }

        if (player.Mode == Mode.PC)
        {
            //플레이어 상태 공격으로 바꿈(PC용)
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (player.Pstate != Pstate.Move)
                {
                    return;
                }

                if (attackTime >= resetTime)
                {
                    Attack();

                    player.Pstate = Pstate.Attack;
                }

            }

            //1초에 한번 공격 가능하게 만듦(PC용)   
            if (isAttack)
            {
                attackTime -= Time.deltaTime;

                if (attackTime <= 0)
                {
                    attackTime = resetTime;

                    isAttack = false;
                }
            }
        }
        else
        {
            //플레이어 상태 공격으로 바꿈
            if (touchManager.IsAttack && player.Pstate != Pstate.Death)
            {
                if (player.Pstate == Pstate.Ladder || player.Pstate == Pstate.LadderHit)
                {
                    return;
                }

                if (attackTime >= resetTime)
                {
                    Attack();

                    player.Pstate = Pstate.Attack;
                }
            }

            //1초에 한번 공격 가능하게 만듦        
            if (touchManager.IsAttack)
            {
                attackTime -= Time.deltaTime;

                if (attackTime <= 0)
                {
                    attackTime = resetTime;

                    touchManager.IsAttack = false;
                }
            }
        }                
    }
     
    public void Attack()
    {       
        if (player.Pstate == Pstate.Hit)
        {
            return;
        }

        //공격 되었음..(PC용)
        isAttack = true;

        //콜라이더 켜기
        weapon.enabled = true;            
        //공격 애니메이션 실행
        playerAnim.AnimAttack(true);
        //공격 소리 켜기
        GameManager.instance.AudioSource(gameObject);
    }

    //PlayerAnim에서 공격 애니메이션이 끝나는 지점에서 실행
    //플레이어 상태 이동으로 바꿈, 공격 애니메이션 끔, 무기 콜라이더 끔
    public void CancelAttack()
    {      
        playerAnim.AnimAttack(false);
        weapon.enabled = false;       
              
        //점프 공격하면 실행
        playerFoot.JumpAttack();

        //공격중에 몬스터에게 맞으면 플레이어 상태는 바꿔주지 않음
        if (player.Pstate == Pstate.Hit)
        {
            return;
        }

        player.Pstate = Pstate.Move;
    }   
}
