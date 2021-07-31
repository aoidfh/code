using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class PotalPos : MonoBehaviour
{
    //플레이어
    GameObject player;

    //포탈위치
    public Transform[] potalRoot;
   
    private void Start()
    {     
        player = GameObject.FindGameObjectWithTag("Player");

        //씬이 바뀌면 플레이어는 해당 포탈 위치에 옮겨짐,
        //왼쪽에 있는 포탈을 탈시 바뀐 씬에서 오른쪽 포탈 위치로 옮겨지기 위해 potalRoot의 위치를 바꿈
        if (GameManager.instance.PotalIndex == -1)
        {
            Transform temp;

            temp = potalRoot[1];
            potalRoot[1] = potalRoot[0];
            potalRoot[0] = temp;
        }

        //플레이어는 포탈 위치보다 위에 생성
        player.transform.position = potalRoot[0].transform.position + new Vector3(0, 1, 0);
    }

}
