using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class UI_TouchManager : MonoBehaviour
{
    public PlayerCtrl player;
    public PlayerFoot playerFoot;
    public PlayerWeapon playerWeapon;

    //좌우이동
    float horizontal;
    //상하이동
    float vertical;

    //공격하였는가..
    bool isAttack;
    //점프하였는가..
    bool isJump;
    //포탈 이동을 하였는가..
    bool isPotal;

    bool isLadder;
  
    public bool IsPotal { get => isPotal; set => isPotal = value; }
    public bool IsAttack { get => isAttack; set => isAttack = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public float Horizontal { get => horizontal; set => horizontal = value; }
    public float Vertical { get => vertical; set => vertical = value; }
    public bool IsLadder { get => isLadder; set => isLadder = value; }

    private void Start()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
    }

    //UI 버튼 클릭 실행 함수

    public void LeftArrowTouchDown()
    {
        if (player.Pstate == Pstate.Move)
        {
            Horizontal = -1f;
        }
    }

    public void LeftArrowTouchUp()
    {        
        Horizontal = 0;              
    }

    public void RightArrowTouchDown()
    {
        if (player.Pstate == Pstate.Move)
        {
            Horizontal = 1f;
        }      
    }

    public void RightArrowTouchUp()
    {       
        Horizontal = 0;             
    }

    public void UpArrowTouchDown()
    {
        if (player.Pstate == Pstate.Ladder)
        {
            vertical = 1f;
        }        
    }

    public void UpArrowTouchUp()
    {       
        Vertical = 0;             
    }

    public void DownArrowTouchDown()
    {
        if (player.Pstate == Pstate.Ladder)
        {
            Vertical = -1f;
        }       
    }

    public void DownArrowTouchUp()
    {       
        Vertical = 0;           
    }

    public void AttackTouch()
    {
        if (player.Pstate == Pstate.Move)
        {
            IsAttack = true;
        }
       
    }

    public void JumpTouch()
    {
        if (player.Pstate == Pstate.Move)
        {
            IsJump = true;
        }      
    }

    public void PotalTouchDown()
    {        
        isPotal = true;       
    }

    public void PotalTouchUp()
    {
        isPotal = false;       
    }

    public void LadderTouchDown()
    {        
        IsLadder = true;

        vertical = 0;        
    }

    public void LadderTouchUp()
    {
        IsLadder = false;
    }
}
