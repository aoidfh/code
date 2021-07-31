using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class PlayerFoot : MonoBehaviour
{
    //플레이어
    public PlayerCtrl player;
    
    public PlayerBody playerBody;
    
    //플레이어 발
    public Collider2D footCollider;
    //플레이어애니메이션
    public PlayerAnim playerAnim;

    //점프시간
    float jumpTime;
    //점프 지연시간
    readonly float delayTime = 0.7f;

    //땅에 닿았는지
    bool isGround;
        
    //땅에 떨어질때 중력
    float rigdyTime;
    readonly float rigdyDelayTime = 0.5f;

    //점프 최고점 시간..
    float GroundTime;
    readonly float GroundResetTime = 0.3f;

    public bool IsGround { get => isGround; set => isGround = value; }
    public float JumpTime { get => jumpTime; set => jumpTime = value; }
    public float DelayTime => delayTime;

    public float RigdyTime { get => rigdyTime; set => rigdyTime = value; }
    public float RigdyDelayTime { get => rigdyDelayTime;}

    private void Start()
    {
        jumpTime = DelayTime;
        RigdyTime = RigdyDelayTime;
        GroundTime = GroundResetTime;
    }

    private void Update()
    {       
        //플레이어가 이동시 그림체를 뒤집어주기 때문에 플레이어의 콜라이더와 위치가 안맞음(항상 위치 조정)
        if (playerAnim.Sprite.flipX)
        {
            footCollider.offset = new Vector2(-0.1f, -0.3f);
        }
        else
        {
            footCollider.offset = new Vector2(0.09f, -0.3f);
        }
        
        if (player.IsJump)
        {           
            JumpTime -= Time.deltaTime;
            GroundTime -= Time.deltaTime;
        }

        //점프 초기화, 만약 플레이어가 점프중 공격을 하면 초기화 시키지 않음
        if (JumpTime <= 0)
        {
            if (player.Pstate == Pstate.Attack)
            {
                return;
            }

            player.IsJump = false;

            player.touchManager.IsJump = false;

            player.JumpCnt = 1;

            JumpTime = DelayTime;        
        }

        //점프하면 떨어질때 빨리 떨어짐
        if (IsGround != true)
        {
            if (player.Pstate != Pstate.LadderHit)
            {              
                RigdyTime -= Time.deltaTime;

                if (RigdyTime < rigdyDelayTime - 0.3f)
                {
                    player.Rigdy.gravityScale = 1.3f;                           
                }
            }
        }
        else
        {
            RigdyTime = RigdyDelayTime;
        }
    }

    //사다리를 타는중에는 플레이어의 다리는 땅에 닿을 수 없는데 사다리와 땅이 거의 붙어있을때 땅이 사다리보다 낮으면
    //플레이어는 사다리를 타는중에 땅에 닿을 수 있음
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Plan") && other.transform.position.y <= playerBody.Plan.transform.position.y)
        {
            footCollider.isTrigger = false;           
        }       
    }
       
    //플레이어가 떨어질때는 땅에 닿으면 닿았다고 알려줌..
    //점프 중일때는 최고점전에 땅에 닿으면 false고 최고점 후에 닿아야 true가 됨
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plan"))
        {
            if (player.IsJump)
            {               
                if (GroundTime <= 0)
                {                    
                    IsGround = true;
                    GroundTime = GroundResetTime;
                }

                return;
            }

            IsGround = true;           
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {             
        if (other.gameObject.CompareTag("Plan"))
        {
            IsGround = false;
         
            //사다리를 타는중에 땅과 멀어지면 트리거 켜줌
            if (player.Pstate == Pstate.Ladder)
            {
                footCollider.isTrigger = true;
            }
        }
    }

    //점프 공격중이면 실행
    public void JumpAttack()
    {
        if (player.IsJump)
        {
            player.IsJump = false;
            player.touchManager.IsJump = false;

            player.JumpCnt = 1;

            JumpTime = DelayTime;
        }
    }  
}
