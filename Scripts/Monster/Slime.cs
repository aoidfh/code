using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영

public class Slime : Monster
{
    //몬스터 콜라이더
    Collider2D colliders;
  
    //플레이어
    PlayerCtrl player;

    //데미지 텍스트 위치
    public Transform damagePos;
    //데미지 텍스트
    public GameObject damageText;
    
    //리지드바디
    Rigidbody2D rigdy;

    //슬라임 점프력
    public float jumpPower;
    //생성시 시작 y위치
    float startPosY;

    //몬스터 체력 슬라이더
    public Slider mushroom_HpSlider;
    //몬스터 체력바 오브젝트
    public GameObject hpBar;

    private void Start()
    {
        //몬스터 이름
        MonName = "슬라임";
        //몬스터 스탯
        Stat = new Stat(100f, 100f, 30f);

        //시작 y위치
        startPosY = transform.position.y;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        MonAnim = transform.GetChild(0).GetComponent<MonsterAnim>();
        colliders = GetComponent<CircleCollider2D>();
        rigdy = GetComponent<Rigidbody2D>();

        StartCoroutine("ChangeMove");

        //최대 체력 설정
        mushroom_HpSlider.maxValue = Stat.MaxHp;

        hpBar.SetActive(false);
    }
  
    private void Update()
    {
        //몬스터 체력바 위치 설정
        mushroom_HpSlider.transform.position = hpBar.transform.position;

        //몬스터 체력바 체력 설정(맞으면 깍임)
        mushroom_HpSlider.value = Stat.Hp;

        //몬스터 애니메이션(무브)
        MonAnim.AnimMove(MoveFlag);
       
        if (HitWall)
        {
            ChangeFlagTime -= Time.deltaTime;
        }

        if (Mstate == Mstate.Death)
        {
            base.Death();

            //체력바 끄기
            Invoke("HpBarSetActive", 1f);
            //콜라이더 끄기
            colliders.enabled = false;
        }
        else if (Mstate == Mstate.Hit)
        {
            if (IsHit)
            {
                //피격 효과 실행
                HitEffect();

                //데미지 텍스트 생성
                damageText.GetComponent<DamageText>().DamageTxt(Stat.Damage);
                Instantiate(damageText, damagePos);

                colliders.enabled = false;
            }

            IsHit = false;
        }
        else if (Mstate != Mstate.Hit)
        {
            colliders.enabled = true;
        }
       
    }

    private void FixedUpdate()
    {
        if (Mstate == Mstate.Move)
        {
            base.Move();
        }
        else if (Mstate == Mstate.Chase)
        {
            base.Tracking(player.PlayerPos, 1f);
        }
    
        Jump();
    }

    //이동 애니메이션이 실행중일때 스프라이트의 이름에 따라 슬라임이 올라갔다가 내려옴(점프처럼 보이기 위함)
    public void Jump()
    {        
        Vector3 monVec = new Vector3(0, jumpPower, 0) * Time.deltaTime;
   
        if (MonAnim.AnimName == "SlimeMove2")
        {
            transform.position += monVec;         
        }
        else if (MonAnim.AnimName == "SlimeMove4")
        {
            transform.position -= monVec;
        }

        //이동 애니메이션 끝나는 시점과 피격 애니메이션 시작하는 시점에 항상 포지션 Y좌표로 초기화 시켜줌
        if (MonAnim.AnimName == "SlimeMove7" || MonAnim.AnimName == "SlimeHit" || MoveFlag == 0)
        {
            monVec = new Vector3(transform.position.x, startPosY, 0);

            transform.position = monVec;
        }
    }
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            //피격 상태로 변경
            Mstate = Mstate.Hit;

            //넉백 실행
            KnockBack(other.transform.position);

            IsHit = true;
        }       
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //몬스터가 벽과 부딪히면 방향을 바꿈(몬스터 상태가 추적중이 아니라면..)
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("MonsterWall"))
        {
            if (Mstate == Mstate.Chase)
            {
                HitWall = true;

                return;
            }

            while (true)
            {
                int moveFlag = Random.Range(-1, 2);

                if (MoveFlag != moveFlag && moveFlag != 0)
                {
                    MoveFlag = moveFlag;                    

                    return;
                }
            }
        }
    }

    //피격당할 시 효과
    void HitEffect()
    {
        hpBar.SetActive(true);

        //이동방향 코루틴 정지
        StopCoroutine("ChangeMove");

        //몬스터 상태 변경
        Mstate = Mstate.Hit;

        //몬스터 체력
        Stat.SetHp(Stat.Dfs, player.Stat.Att);

        //피격 애님 실행
        MonAnim.AnimHit(true);
      
        //추적 함수 실행 
        Invoke("ChangeTracking", 0.5f);

        if (Stat.Hp <= 0)
        {
            //상태 변경
            Mstate = Mstate.Death;

            //소리 실행
            GameManager.instance.AudioSource(gameObject, clip[1]);

            return;
        }

        //소리 실행
        GameManager.instance.AudioSource(gameObject, clip[0]);
    }

    //몬스터가 맞으면 넉백함
    void KnockBack(Vector3 targetPos)
    {
        //flag == 방향, 0보다 작으면 -1 크면 1
        int flag = transform.position.x - targetPos.x < 0 ? -1 : 1;

        //뒤로 밀려나는 힘
        rigdy.AddForce(new Vector2(flag * 2f, 0), ForceMode2D.Impulse);       
    }

    //맞으면 추적상태로 변경, 피격 애니메이션 끔
    void ChangeTracking()
    {
        if (Mstate != Mstate.Death)
        {
            Mstate = Mstate.Chase;
        }

        MonAnim.AnimHit(false);

        //넉백 효과 초기화
        rigdy.velocity = new Vector3(0, 0, 0);
    }

    public void HpBarSetActive()
    {
        hpBar.SetActive(false);
    }
}
