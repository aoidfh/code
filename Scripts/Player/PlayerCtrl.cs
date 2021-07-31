using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by 박준영

//구동방식
public enum Mode
{
    PC,
    Moblie
}

//캐릭터 상태 표시(이동, 공격, 피격, 대기, 죽음)
public enum Pstate
{
    Move,
    Attack,
    Hit,
    Ladder,
    LadderHit,  
    Death
}

public class PlayerCtrl : MonoBehaviour
{
    //리지드바디
    Rigidbody2D rigdy;
    //플레이어 무기
    public PlayerWeapon playerWeapon;
    //플레이어애니메이터
    public PlayerAnim playerAnim;
    //플레이어 다리
    public PlayerFoot playerFoot;
    //데미지텍스트 오브젝트
    public GameObject damageText;
    //텍스트 위치
    public Transform damagePos;
    //모바일용 터치매니져    
    public UI_TouchManager touchManager;

    Mode mode;
    [SerializeField]
    //플레이어스탯
    Stat stat;
    [SerializeField]
    //플레이어 상태
    Pstate pstate;

    //좌우이동(PC용)
    float horizontal;
    //임시저장
    float tempHorizontal;
    //상하이동(PC용)
    float vertical;
    //좌우이동 스피드
    public float horSpeed;   
    //상하이동 스피드
    public float verSpeed;  
    //점프파워
    public float jumpPower;
    //점프횟수
    int jumpCnt = 1;
    //점프중인지..
    bool isJump;
    
    //피격되었는지..
    bool isHit;
    //피격시 효과시간
    int effectTime;
    //피격 시간 초기화
    readonly int resetEffectTime = 10;

    //플레이어 위치
    Vector3 playerPos;

    //사다리에서 떨어지는 시간
    float ladderTime;
    readonly float resetTime = 1f;
   
    //피격한 몬스터
    GameObject monster;   
    //비석
    Tombstone tombstone;


    public Pstate Pstate { get => pstate; set => pstate = value; }
    public Vector3 PlayerPos { get => playerPos; set => playerPos = value; }
    public int JumpCnt { get => jumpCnt; set => jumpCnt = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public GameObject Monster { get => monster; set => monster = value; }   
    public Stat Stat { get => stat; set => stat = value; }
    public bool IsHit { get => isHit; set => isHit = value; }
    public Mode Mode { get => mode; set => mode = value; }
    public Rigidbody2D Rigdy { get => rigdy; set => rigdy = value; }

    void Start()
    {
        Stat = new Stat(100, 100f, 30f, 10f);
        
        Rigdy = GetComponent<Rigidbody2D>();

        mode = Mode.Moblie;      
    }

    void Update()
    {
        if (GameManager.instance.sceneMgr.curScene == 0)
        {            
            Rigdy.gravityScale = 0;
        }
       
        if (Pstate == Pstate.Move)
        {
            if (Mode == Mode.PC)
            {
                //(PC용)
                Jump();
            }
            else
            {
                if (touchManager.IsJump)
                {
                    Jump();
                }
            }

            if (playerFoot.RigdyTime == playerFoot.RigdyDelayTime)
            {               
                Rigdy.gravityScale = 1;                
            }           
        }
        else if (Pstate == Pstate.Attack)
        {
            if (Mode == Mode.PC)
            {
                //점프 공격중에는 horizontal의 힘만큼 앞으로 이동(PC용)
                if (playerFoot.IsGround != true && IsJump)
                {
                    transform.position += new Vector3(horizontal, 0, 0) * horSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (playerFoot.IsGround != true && IsJump)
                {
                    transform.position += new Vector3(touchManager.Horizontal, 0, 0) * horSpeed * Time.deltaTime;
                }
            }        
        }
        else if (pstate == Pstate.Ladder)
        {
            tempHorizontal = 0;

            //캐릭터를 사다리 위치로 이동
            transform.position = new Vector3(PlayerPos.x, transform.position.y);

            if (Mode == Mode.PC)
            {
                //(PC용)
                playerAnim.PlayerLadderAnim(true, vertical);
            }
            else
            {
                playerAnim.PlayerLadderAnim(true, touchManager.Vertical);
            }            
        }
        else if (Pstate == Pstate.Death)
        {
            Death();
        }
      
        //시간이 지나면 코루틴 끄고, 효과시간 초기화
        if (effectTime <= 0)
        {
            StopCoroutine("HitEffect");

            effectTime = resetEffectTime;
        }
              
        //무적시간
        Invincibility(effectTime);

        //플레이어 위치전달
        PlayerPos = this.transform.position;
    }

    void FixedUpdate()
    {
        if (Pstate == Pstate.Move)
        {
            Move();
        }
        else if (pstate == Pstate.Ladder)
        {
            Ladder();            
        }
        else if (isHit)
        {
            Hit();
          
            //데미지 텍스트 생성
            damageText.GetComponent<DamageText>().DamageTxt(Stat.Damage);
            Instantiate(damageText, damagePos);
        }          

        if (isJump)
        {           
            JumpEfftect();
        }    
    }

    //캐릭터 이동
    //v가 0보다 작으면 왼쪽으로 이동, 크면 오른쪽으로 이동, 이동 애니메이션 실행
    //0이면 대기상태, 이동 애니메이션 끔
    public void Move()
    {      
        if (Mode == Mode.PC)
        {
            //(PC용)
            horizontal = Input.GetAxis("Horizontal");

            if (playerFoot.IsGround)
            {
                transform.position += new Vector3(horizontal, 0, 0) * horSpeed * Time.deltaTime;

                tempHorizontal = horizontal;
            }
            else
            {
                transform.position += new Vector3(tempHorizontal, 0, 0) * horSpeed * Time.deltaTime;
            }
                    
            if (horizontal < 0)
            {
                playerAnim.AnimMove(-1);
            }
            else if (horizontal > 0)
            {
                playerAnim.AnimMove(1);
            }
            else
            {
                playerAnim.AnimMove(0);
            }
        }
        else
        {
            if (playerFoot.IsGround)
            {
                transform.position += new Vector3(touchManager.Horizontal, 0, 0) * horSpeed * Time.deltaTime;

                tempHorizontal = touchManager.Horizontal;
            }
            else
            {
                transform.position += new Vector3(tempHorizontal, 0, 0) * horSpeed * Time.deltaTime;
            }
                       
            if (touchManager.Horizontal < 0)
            {
                playerAnim.AnimMove(-1);
            }
            else if (touchManager.Horizontal > 0)
            {
                playerAnim.AnimMove(1);
            }
            else
            {
                playerAnim.AnimMove(0);
            }
        }
    }

    public void Jump()
    {
        if (Mode == Mode.PC)
        {
            //점프는 점프시간과 땅에 닿아야지만 가능(PC용)
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (playerFoot.JumpTime >= playerFoot.DelayTime && playerFoot.IsGround)
                {                    
                    IsJump = true;

                    //점프소리 재생
                    GameManager.instance.AudioSource(playerFoot.gameObject);
                }
            }
        }
        else
        {
            if (playerFoot.JumpTime >= playerFoot.DelayTime && playerFoot.IsGround)
            {
                IsJump = true;

                //점프소리 재생
                GameManager.instance.AudioSource(playerFoot.gameObject);
            }
        }      
    }
    
    public void JumpEfftect()
    {       
        if (IsJump != true || JumpCnt <= 0)
        {                    
            return;
        }
                               
        //점프 카운트 감소
        JumpCnt--;
             
        //캐릭터 점프파워
        Rigdy.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);        
    }

    //사다리 타기
    void Ladder()
    {
        if (Mode == Mode.PC)
        {
            //(PC용)
            vertical = Input.GetAxis("Vertical");

            transform.position += new Vector3(0, vertical, 0) * verSpeed * Time.deltaTime;

            //사다리를 타면 중력을 없애고, 힘도 초기화시킴
            Rigdy.gravityScale = 0;
            Rigdy.velocity = new Vector2(0, 0);

            //플레이어가 땅에 닿아 캐릭터가 움직이지 않을때 
            //사다리에서 떨어지기 위해 일정한 시간동안 눌려주고 있으면 떨어짐
            if (vertical != 0 && playerFoot.footCollider.isTrigger != true)
            {
                ladderTime -= Time.deltaTime;

                if (ladderTime <= 0)
                {
                    Pstate = Pstate.Move;

                    playerAnim.PlayerLadderAnim(false);

                    ladderTime = resetTime;
                }
            }
            else
            {
                ladderTime = resetTime;
            }
        }
        else
        {

            transform.position += new Vector3(0, touchManager.Vertical, 0) * verSpeed * Time.deltaTime;

            //사다리를 타면 중력을 없애고, 힘도 초기화시킴
            Rigdy.gravityScale = 0;
            Rigdy.velocity = new Vector2(0, 0);

            //플레이어가 땅에 닿아 캐릭터가 움직이지 않을때 
            //사다리에서 떨어지기 위해 일정한 시간동안 눌려주고 있으면 떨어짐
            if (touchManager.Vertical != 0 && playerFoot.footCollider.isTrigger != true)
            {
                ladderTime -= Time.deltaTime;

                if (ladderTime <= 0)
                {
                    Pstate = Pstate.Move;

                    playerAnim.PlayerLadderAnim(false);

                    ladderTime = resetTime;
                }
            }
            else
            {
                ladderTime = resetTime;
            }
        }
    }

    void Hit()
    {        
        //피격 당한 몬스터의 스탯
        Monster monsterStat = monster.GetComponent<Monster>();

        //몬스터에게 맞으면 체력깍임(플레이어 방어력 - 몬스터 공격력)
        Stat.SetHp(Stat.Dfs, monsterStat.Stat.Att);

        //플레이어의 체력이 0보다 작으면 상태를 죽음으로 바꿈
        if (Stat.Hp <= 0)
        {
            Pstate = Pstate.Death;
            
            IsHit = false;            
            return;
        }

        //피격효과
        StartCoroutine("HitEffect");  
        
        //스턴효과
        Invoke("Stun", 0.8f);
       
        IsHit = false;

        //사다리를 타는중일때는 일정시간 멈춤
        if (Pstate == Pstate.LadderHit)
        {
            Rigdy.velocity = new Vector2(0, 0);

            return;
        }

        //넉백효과
        KnockBack(monster.transform.position);        
    }

    void Death()
    {      
        //움직일 수 없음
        horSpeed = 0;
        jumpCnt = 0;
        Rigdy.gravityScale = 0;
              
        //플레이어 콜라이더의 레이어를 바꿔줌
        gameObject.transform.GetChild(0).gameObject.layer = 10;       

        //데스애님 실행
        playerAnim.PlayerDeath();

        tombstone = GameObject.FindGameObjectWithTag("TombstonePos").GetComponent<Tombstone>();

        //캐릭터가 비석 위로 이동
        transform.position = tombstone.transform.position - new Vector3(0, 0.5f, 0);
    }

    //플레이어가 몬스터에게 맞으면 뒤로 밀려남   
    public void KnockBack(Vector3 targetPos)
    {
        Rigdy.velocity = new Vector2(0, 0);

        //flag == 방향, 0보다 작으면 -1 크면 1
        int flag = transform.position.x - targetPos.x < 0 ? -1 : 1;

        //점프중이 아니면..
        if (IsJump != true)
        {
            //뒤로 밀려나는 힘
            Rigdy.AddForce(new Vector2(flag, 3.5f), ForceMode2D.Impulse);
        }

        //점프중이면.. 뒤로 밀려나는 힘    
        Rigdy.AddForce(new Vector2(flag, 0), ForceMode2D.Impulse);                 
    }

    //피격당할시 무적, layer를 바꿔서 몬스터와 충돌 안되게 함
    void Invincibility(int time)
    {      
        if (time != resetEffectTime)
        {
            //플레이어 콜라이더의 레이어를 바꿔줌
            gameObject.transform.GetChild(0).gameObject.layer = 10;

            return;
        }

        if (pstate != Pstate.Death)
        {
            //플레이어 콜라이더의 레이어를 바꿔줌
            gameObject.transform.GetChild(0).gameObject.layer = 8;        
        }
    }

    //플레이어가 피격되면 스프라이트가 반짝이게 보이게 하기 위한 효과
    IEnumerator HitEffect()
    {
        //효과시간 감소
        effectTime--;

        //효과시간이 홀수면 색을 바꿔줌
        if (effectTime % 2 == 0)
        {
            playerAnim.Sprite.color = new Color(255, 255, 255, 255);
        }
        else
        {
            playerAnim.Sprite.color = new Color(255, 0, 255, 255);
        }

        yield return new WaitForSeconds(0.2f);

        StartCoroutine("HitEffect");
    }    

    //맞으면 잠시 멈춘뒤 플레이어 상태 바꿔줌
    public void Stun()
    {       
        if (Pstate == Pstate.Hit)
        {
            Pstate = Pstate.Move;            
        }
        else if (Pstate == Pstate.LadderHit)
        {
            Pstate = Pstate.Ladder;
        }

        Rigdy.velocity = new Vector2(0, 0);
    }      
}
