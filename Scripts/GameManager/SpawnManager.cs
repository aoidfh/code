using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class SpawnManager : MonoBehaviour
{
    //생성될 위치
    public Transform[] spawnPoints;
    //몬스터 종류
    public GameObject monster;
  
    //위치에 몬스터가 생성되었는지..
    bool[] isSpawn;
  
    //생성 시간
    float[] spawnTime;
    //생성 딜레이 시간
    readonly float delayTime =3f;

    void Start()
    {
        isSpawn = new bool[spawnPoints.Length];
        spawnTime = new float[spawnPoints.Length];

        //시작시 몬스터 생성
        //스폰타임 3초로 맞춤
        for (int i = 0; i < spawnPoints.Length; i++)
        {                    
            SpawnEnemy(i);
            spawnTime[i] = delayTime;
        }      
    }

    void Update()
    {
        //생성위치에 몬스터가 없으면..
        //생성 딜레이 시간이 지나면 몬스터 생성후 생성 딜레이 시간 초기화
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (isSpawn[i] != true)
            {
                spawnTime[i] -= Time.deltaTime;

                if (spawnTime[i] <= 0)
                {
                    SpawnEnemy(i);

                    spawnTime[i] = 3;
                }
            }

            //생성위치에 몬스터가 없으면 isSpawn[i]번째를 false로 바꿈
            if (transform.GetChild(i).childCount == 0)
            {
                isSpawn[i] = false;
            }            
        }        
    }

    //몬스터 생성 함수
    //몬스터를 생성하면 isSpawn을 true로 바꿈
    void SpawnEnemy(int pos)
    {              
        Instantiate(monster, spawnPoints[pos]);
      
        isSpawn[pos] = true;       
    }   
}
