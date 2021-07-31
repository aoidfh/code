using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영
//몬스터 상태(이동, 추적, 피격, 처치)   
public enum Mstate
{
    Move,
    Chase,
    Hit,
    Death
}

[System.Serializable]
public class Monster : MonoBehaviour
{
    //몬스터 이름
    string monName;
    //몬스터 방향
    int moveFlag;
    //몬스터 상태
    Mstate mstate;
    //몬스터 스탯
    Stat stat;     

    //몬스터 오디오
    public AudioClip[] clip;

    //몬스터 애님
    MonsterAnim monAnim;

    //피격 당하였는가..
    bool isHit;

    //방향 지연시간
    float changeFlagTime;
    //방향 딜레이 시간
    readonly float flagDelayTime = 0.5f;
    //벽과 부딪혔는가..
    bool hitWall;
    //임시저장
    int tempMoveFlag;
    
    public Stat Stat { get => stat; set => stat = value; }
    public int MoveFlag { get => moveFlag; set => moveFlag = value; }
    public Mstate Mstate { get => mstate; set => mstate = value; }         
    public string MonName { get => monName; set => monName = value; }
    public MonsterAnim MonAnim { get => monAnim; set => monAnim = value; }    
    public bool IsHit { get => isHit; set => isHit = value; }
    public float ChangeFlagTime { get => changeFlagTime; set => changeFlagTime = value; }
    public bool HitWall { get => hitWall; set => hitWall = value; }

    public float FlagDelayTime => flagDelayTime;

    public void Move()
    {
        //MonsterAI.moveFlag(방향)
        //0 == 대기, -1 == 왼쪽, 1 == 오른쪽으로 움직임  
        Vector3 monVec = new Vector3(MoveFlag, 0, 0) * Time.deltaTime;

        transform.position += monVec;
    }

    //몬스터가 mstate가 Chase(추적)상태면 실행
    //몬스터가 플레어이를 추적해서 따라감
    public void Tracking(Vector3 targetPos, float monSpeed)
    {
        //MonsterAI.moveFlag(방향)
        //-1 == 왼쪽, 1 == 오른쪽으로 움직임 
        Vector3 monVec = new Vector3(MoveFlag * monSpeed, 0, 0) * Time.deltaTime;

        //몬스터 위치
        float monPos = this.transform.position.x;
      
        //몬스터가 벽과 부딪히면 일정 시간동안 방향을 바꿔줌
        if (HitWall)
        {
            while (true)
            {
                int rand = Random.Range(-1, 2);

                if (rand != 0 && rand != tempMoveFlag)
                {
                    MoveFlag = rand;

                    break;
                }
            }                                  

            if (ChangeFlagTime <= 0)
            {               
                HitWall = false;
                ChangeFlagTime = FlagDelayTime;
            }
        }
        else
        {
            if (monPos > targetPos.x + 1)
            {
                MoveFlag = -1;
            }
            else if (monPos < targetPos.x - 1)
            {
                MoveFlag = 1;
            }

            tempMoveFlag = MoveFlag;
        }
        
        transform.position += monVec;     
    }
   
    //3초마다 랜덤으로 이동방향 변경
    IEnumerator ChangeMove()
    {             
        MoveFlag = Random.Range(-1, 2);
       
        yield return new WaitForSeconds(3);

        StartCoroutine("ChangeMove");
    }
   
    //데스애님 실행, 2초후 오브젝트삭제 함수 실행
    public void Death()
    {     
        //몬스터 죽음 애님 실행
        MonAnim.AnimDeath(true);
        
        //2초후 오브젝트 삭제
        Invoke("ObjectDestory", 2f);      
    }

    //오브젝트 삭제
    public void ObjectDestory()
    {              
        Destroy(gameObject);
    }   
}