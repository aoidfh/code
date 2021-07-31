using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class CheatKey : MonoBehaviour
{
    public PlayerCtrl player;
    public QuestInformation questInformation;
    public FieldItemPos fieldItemPos;

    //치트키
    bool cheat;

    //생성될 아이템개수
    int itemNum;

    public int ItemNum { get => itemNum; set => itemNum = value; }

    public bool Cheat { get => cheat; set => cheat = value; }

    void Update()
    {             
        if (Input.GetKeyDown(KeyCode.Q))
        {
            questInformation.ProduceKeyCount = questInformation.KillCount;
        }

        //치트키를 사용하면 플레이어의 앞에 장비 아이템 2개 생성
        if (Input.GetKeyDown(KeyCode.W))
        {
            cheat = true;

            PlayerCtrl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

            Vector3 spawnItemPos;

            for (int i = 0; i < 2; i++)
            {
                spawnItemPos = player.PlayerPos + new Vector3(i + 1, -0.2f, 0);

                Instantiate(fieldItemPos.fieldItem, spawnItemPos, transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            player.Stat.Hp = 10;
        }
    }
}
