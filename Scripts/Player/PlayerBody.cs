using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class PlayerBody : MonoBehaviour
{
    //플레이어
    public PlayerCtrl player;
    //플레이어애니메이션
    public PlayerAnim playerAnim;
    //플레이어 몸
    public Collider2D bodyCollider;
    //플레이어 발
    public Collider2D footCollider;
    //모바일용 터치매니져
    public UI_TouchManager touchManager;
    public UI_Information ui;
   
    //바닥 
    GameObject plan;

    //다음씬으로 넘어갈것인가..
    bool nextPotal;    
    //사다리를 타는가..
    bool isLadder;

    
    bool delayTime;
    float potalTime;
    readonly float resetTime = 1f;    
   
    public GameObject Plan { get => plan; set => plan = value; }
    public bool NextPotal { get => nextPotal; set => nextPotal = value; }
    public bool IsLadder { get => isLadder; set => isLadder = value; }

    private void Start()
    {
        potalTime = resetTime;      
    }

    void Update()
    {        
        //플레이어가 이동시 그림체를 뒤집어주기 때문에 플레이어의 콜라이더와 위치가 안맞음(항상 위치 조정)
        if (playerAnim.Sprite.flipX)
        {
            bodyCollider.offset = new Vector2(-0.1f, 0);
        }
        else
        {
            bodyCollider.offset = new Vector2(0.09f, 0);            
        }

        if (delayTime)
        {
            potalTime -= Time.deltaTime;

            if (potalTime <= 0)
            {
                potalTime = resetTime;
                delayTime = false;                
            }
        }

        if (player.Mode == Mode.PC)
        {
            //(PC용)
            //포탈 이동시 딜레이 시간 주고 연속적으로 누를 수 없게 만듦
            if (Input.GetKeyDown(KeyCode.Z) && potalTime >= resetTime)
            {
                nextPotal = true;
                delayTime = true;
            }
        }
        else
        {    
            //포탈 이동시 딜레이 시간 주고 연속적으로 누를 수 없게 만듦
            if (touchManager.IsPotal)
            {
                nextPotal = true;
            }
            else
            {
                nextPotal = false;
            }
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {                
        //상태를 피격으로 바꿔주고, 플레이어 넉백을 위해 몬스터 위치 받아옴
        if (other.gameObject.CompareTag("Monster"))
        {          
            if (player.Pstate == Pstate.Ladder)
            {
                player.Pstate = Pstate.LadderHit;
            }
            else
            {
                player.Pstate = Pstate.Hit;
            }

            player.IsHit = true;

            player.Monster = other.gameObject;        
        }      
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Potal"))
        {
            ui.PotalButtonTxt("Sprite/UI/Key/Potal", "포탈타기");
        }

        //플레이어가 사다리와 부딪히면..
        if (other.gameObject.CompareTag("Ladder"))
        {
            if (player.Pstate == Pstate.Move)
            {               
                IsLadder = true;    
            }
            else
            {             
                IsLadder = false;
            }

            Plan = other.gameObject;

            if (player.Pstate == Pstate.Move)
            {
                if (player.Mode == Mode.PC)
                {
                    //플레이어가 사다리보다 높게 있으면 위로는 위의 키로는 사다리를 탈 수 없음(PC용)
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        if (other.transform.position.y <= transform.position.y)
                        {
                            return;
                        }

                        player.Pstate = Pstate.Ladder;

                        //사다리 위치 전달
                        player.PlayerPos = other.gameObject.transform.position;

                        //사다리 타는중이면 트리거 켜기
                        footCollider.isTrigger = true;
                    }

                    //플레이어가 사다리보다 낮게 있으면 아래키로는 사다리를 탈 수 없음(PC용)
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        if (other.transform.position.y >= transform.position.y)
                        {
                            return;
                        }

                        player.Pstate = Pstate.Ladder;

                        //사다리 위치 전달
                        player.PlayerPos = other.gameObject.transform.position;

                        //사다리 타는중이면 트리거 켜기
                        footCollider.isTrigger = true;
                    }
                }
                else
                {
                    if (touchManager.IsLadder)
                    {
                        player.Pstate = Pstate.Ladder;

                        //사다리 위치 전달
                        player.PlayerPos = other.gameObject.transform.position;

                        //사다리 타는중이면 트리거 켜기
                        footCollider.isTrigger = true;
                    }
                }               
            }
        }      
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Potal"))
        {
            ui.PotalButtonTxt("Sprite/UI/Key/Ladder", "사다리타기");
        }

        //사다리와 부딪히지 않으면..
        if (other.gameObject.CompareTag("Ladder"))
        {
            if (player.Pstate == Pstate.Ladder)
            {
                player.Pstate = Pstate.Move;

                player.playerAnim.PlayerLadderAnim(false);

                //사다리에서 벗어나면 트리거 끄기
                footCollider.isTrigger = false;

                touchManager.IsLadder = false;
                IsLadder = false;               
            }
        }
    }            
}
