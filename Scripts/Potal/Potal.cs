using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class Potal : MonoBehaviour
{   
    //씬을 옮기기위한 인덱스
    public int sceneIndex;
    //플레이어 몸
    PlayerBody playerBody;    
     
    private void Start()
    {       
        playerBody = GameObject.FindGameObjectWithTag("PlayerCollider").GetComponent<PlayerBody>();
    }
 
    private void OnTriggerStay2D(Collider2D other)
    {
        //씬을 옮기고 포탈위치에 플레이어 위치 옮기기
        if (other.gameObject.CompareTag("PlayerCollider") && playerBody.NextPotal)
        {            
            //오른쪽에 위치한 포탈
            if (sceneIndex == 1)
            {
                //플레이어의 인벤토리안에 키가 있는지 확인
                if (GameManager.instance.keyInventory.slot.InItem)
                {
                    if (GameManager.instance.PotalOpen != true)
                    {
                        //포탈 열쇠 사용
                        GameManager.instance.keyInventory.UseKey();

                        //해당 포탈을 열음
                        GameManager.instance.PotalOpen = true;
                    }
                }
                else
                {
                    //열쇠가 없고 포탈이 열려 있지 않다면 포탈 이동 불가능하다고 메시지 띄움
                    if (GameManager.instance.PotalOpen != true)
                    {
                        GameManager.instance.itemInfo.potalTxt.SetActive(true);

                        playerBody.NextPotal = false;

                        Invoke("CancelPotalText", 1f);
                    }
                }
            }

            //해당 포탈이 열려 있다면 씬이동
            if (GameManager.instance.PotalOpen)
            {
                GameManager.instance.sceneMgr.NextGameScene(sceneIndex);
                GameManager.instance.PotalIndex = sceneIndex;

                //소리 실행
                GameManager.instance.AudioSource(gameObject);

                playerBody.NextPotal = false;
            }
                         
        }
    }
  
    //이동불가 포탈 텍스트 끄기
    public void CancelPotalText()
    {
        GameManager.instance.itemInfo.potalTxt.SetActive(false);
    }
}
