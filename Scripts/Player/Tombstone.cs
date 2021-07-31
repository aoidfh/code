using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class Tombstone : MonoBehaviour
{
    //비석
    public GameObject tombstone;
    //플레이어
    PlayerCtrl player;
    //생성 되었는가..
    bool spawn;
    
    private void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();          
    }

    private void Update()
    {
        if (player.Pstate != Pstate.Death)
        {
            //플레이어의 위치를 받음
            transform.position = player.PlayerPos + new Vector3(0, 1, 0);
        }

        //플레이어의 상태가 데스이면 생성
        if (player.Pstate == Pstate.Death)
        {
            //여러개 생성 방지
            if (spawn)
            {
                return;
            }

            //비석 생성
            Instantiate(tombstone, gameObject.transform);

            spawn = true;
        }
    }               
}
