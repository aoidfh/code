using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영

public class Mushroom : Monster
{
    //몬스터 콜라이더
    Collider2D colliders;
    //플레이어
    PlayerCtrl player;
  
    //데미지 텍스트 위치
    public Transform damagePos;
    //데미지 텍스트
    public GameObject damageText;

    //몬스터 체력 슬라이더
    public Slider mushroom_HpSlider;
    //몬스터 체력바 오브젝트
    public GameObject hpBar;
      
    void Start()
    {      
        //몬스터 이름
        MonName = "머쉬룸";
        //몬스터 스탯
        Stat = new Stat(100f, 100f, 20f);       

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();  
        MonAnim = transform.GetChild(0).GetComponent<MonsterAnim>(); 
        colliders = GetComponent<CircleCollider2D>();

        StartCoroutine("ChangeMove");

        ChangeFlagTime = FlagDelayTime;

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

        if (HitWall)
        {
            ChangeFlagTime -= Time.deltaTime;
        }

        //몬스터 애니메이션(무브)
        MonAnim.AnimMove(MoveFlag);

        if (Mstate == Mstate.Move)
        {
            base.Move();
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
            }

            IsHit = false;
        }
        else if (Mstate == Mstate.Chase)
        {
            base.Tracking(player.PlayerPos, 1f);
        }
        else if (Mstate == Mstate.Death)
        {            
            base.Death();

            //체력바 끄기
            Invoke("HpBarSetActive", 1f);

            //몬스터 콜라이더 끄기
            colliders.enabled = false;            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //플레이어에게 맞으면..
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            //피격 상태로 변경
            Mstate = Mstate.Hit;

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

        //몬스터 체력
        Stat.SetHp(Stat.Dfs, player.Stat.Att);

        //피격 애님 실행
        MonAnim.AnimHit(true);
                           
        //추적 함수 실행 
        Invoke("ChangeTracking", 1f);

        if (Stat.Hp <= 0)
        {
            //상태 변경
            Mstate = Mstate.Death;

            //피격 및 죽는소리 실행
            GameManager.instance.AudioSource(gameObject, clip[1]);

            return;
        }

        //피격 소리 실행
        GameManager.instance.AudioSource(gameObject, clip[0]);
    }

    //맞으면 추적상태로 변경, 피격 애니메이션 끔
    void ChangeTracking()
    {
        if (Mstate != Mstate.Death)
        {
            Mstate = Mstate.Chase;
        }

        MonAnim.AnimHit(false);       
    }

    public void HpBarSetActive()
    {
        hpBar.SetActive(false);
    }       
}
