using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//박준영

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    //인벤토리
    public Inventroy inventory;
    //열쇠인벤토리
    public KeyInventory keyInventory;
    //필드아이템위치
    public FieldItemPos fieldItemPos;
    //아이템데이터베이스
    public ItemDatabase itemDatabase;    
    //아이템정보창
    public UI_Information itemInfo;
    //퀘스트정보창
    public QuestInformation questInfo;
    //치트키
    public CheatKey cheatKey;

    //씬매니저
    public SceneMgr sceneMgr;

    //메인UI
    public GameObject titleUI;
    //게임UI
    public GameObject GameUI;

    //플레이어
    public GameObject player;  
    
    //포탈인덱스(위치 바꾸기위해)
    int potalIndex;
    //열쇠를 사용하여 포탈을 열었는가..
    bool potalOpen;
    
    public int PotalIndex { get => potalIndex; set => potalIndex = value; }    
    public bool PotalOpen { get => potalOpen; set => potalOpen = value; }  

    void Awake()
    {
        instance = this;
    }

    //소리 재생(오브젝트에 한가지의 소리가 있을 경우)
    public void AudioSource(GameObject obj)
    {
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        audioSource.Play();
    }

    //소리 재생(오브젝트에 여러개의 소리가 있을 경우)
    public void AudioSource(GameObject obj, AudioClip clip)
    {        
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        audioSource.clip = clip;
           
        audioSource.Play();
    }
 
    //타이틀씬이면 타이틀UI만 킴, 나머지씬이면 타이틀UI만 끔
    public void NextSceneActiveObjs(int sceneBuildIndex)
    {
        if (sceneBuildIndex == 0)
        {
            titleUI.SetActive(true);
            GameUI.SetActive(false);
            player.SetActive(false);            
        }
        else
        {           
            titleUI.SetActive(false);
            GameUI.SetActive(true);
            player.SetActive(true);
        }       
    }    
}

