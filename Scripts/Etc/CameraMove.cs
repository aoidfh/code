using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class CameraMove : MonoBehaviour
{    
    //플레이어
    GameObject player;

    //카메라 X위치 최소값
    public float minPosX;
    //카메라 X위치 최고값
    public float maxPosX;
    //카메라 Y위치 최소값
    public float minPosY;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    //카메라는 플레이어를 따라가게 만듬
    //카메라는 X,Y의 최소값과 최대값 범위 안에서만 움직임
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);

        if (transform.position.x <= minPosX)
        {
            transform.position = new Vector3(minPosX, transform.position.y , -10f);
        }
        else if (transform.position.x >= maxPosX)
        {
            transform.position = new Vector3(maxPosX, transform.position.y, -10f);
        }

        if (transform.position.y <= minPosY)
        {     
            transform.position = new Vector3(transform.position.x, minPosY, -10f);
        }       
    }

}

