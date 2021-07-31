using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestInformation : MonoBehaviour
{    
    //퀘스트 정보창
    public GameObject questInfo;
    //퀘스트 진행 목록 정보창
    public GameObject questListInfo;
    //퀘스트 수락 버튼
    public GameObject acceptBtn;
    //퀘스트 완료 버튼
    public GameObject completeBtn;
    //퀘스트 진행텍스트
    public GameObject ProceedingInfo;

    //퀘스트 정보창 텍스트
    public Text questTxt;
    //퀘스트 진행 목록 텍스트
    public Text questListTxt;
    //퀘스트 진행중,완료 텍스트
    public Text proceedingTxt;

    //퀘스트정보창의 몬스터 이름과 퀘스트 진행 목록의 몬스터 이름이 다를 수 없음(체크하기 위함)
    public bool CheckName;
    //퀘스트 수락했는지..
    bool questAccept;
    //퀘스트 완료했는지..
    bool[] questComplete = new bool[2];    

    //몬스터이름텍스트
    [SerializeField]
    string[] monName = new string[2];
    
    //열쇠 생성 카운트
    int produceKeyCount;
    //열쇠 생성 목표 카운트
    readonly int killCount = 10;

    public int ProduceKeyCount { get => produceKeyCount; set => produceKeyCount = value; }
    public bool QuestAccept { get => questAccept; set => questAccept = value; }
    public bool[] QuestComplete { get => questComplete; set => questComplete = value; }    
    public string[] MonName { get => monName; set => monName = value; }
    public int KillCount => killCount;

    private void Start()
    {       
        //시작시 끔(퀘스트정보, 진행 목록, 진행텍스트, 완료버튼)
        questInfo.SetActive(false);
        questListInfo.SetActive(false);
        ProceedingInfo.SetActive(false);
        completeBtn.SetActive(false);
    }

    private void Update()
    {        
        //현재 카운트가 목표 카운트와 같다면..
        if (produceKeyCount == KillCount)
        {
            //퀘스트 완료
            completeBtn.SetActive(true);
        }

    

        //퀘스트 수락시 왼쪽에 중앙부분에 퀘스트 내용 띄워줌
        if (CheckName)
        {
            for (int i = 0; i < questComplete.Length; i++)
            {
                if (questComplete[i] != true)
                {
                    questListTxt.text = MonName[i] + " 처치하기 (" +
                                 ProduceKeyCount + "/" + KillCount + ")";

                    return;
                }
            }
        }

    }

    //게임 씬에 따라 퀘스트가 변경
    public void Quest(int sceneBuildIndex)
    {
        //해당 씬의 퀘스트를 완료 하지 않았다면..      
        if (sceneBuildIndex == GameManager.instance.sceneMgr.curScene && QuestComplete[sceneBuildIndex - 1] != true)
        {
            questTxt.text     = MonName[sceneBuildIndex - 1] + " " + 
                                KillCount + "마리 처치하기 보상: 열쇠";

            CheckName = true;
        }
        else
        {
            CheckName = false;         
        }
             
        NextSceneQuest(sceneBuildIndex);
    }

    //다음 씬으로 이동시 완료되지 않은 퀘스트 초기화
    public void NextSceneQuest(int sceneBuildIndex)
    {
        //해당 씬의 퀘스트를 완료하지 않았다면..
        if (QuestComplete[sceneBuildIndex - 1] != true)
        {
            //퀘스트를 수락했다면..
            if (QuestAccept)
            {
                return;
            }

            //퀘스트 수락버튼 켜고, 퀘스트 진행중 텍스트 끄기
            acceptBtn.SetActive(true);
            ProceedingInfo.SetActive(false);         
        }
    }

    //퀘스트 정보창 켜고끄기
    public void SetQuestInfo()
    {        
        if (questInfo.activeSelf)
        {
            questInfo.SetActive(false);
        }
        else
        {
            questInfo.SetActive(true);
        }
    }

    //퀘스트 수락
    public void AcceptQuest(bool active)
    {              
        QuestAccept = active;               
        
        //퀘스트 정보창, 퀘스트 수락 버튼 끄기
        questInfo.SetActive(false);         
        acceptBtn.SetActive(false);

        //퀘스트 리스트, 퀘스트 진행중 텍스트 켜기
        ProceedingInfo.SetActive(true);
        questListInfo.SetActive(true);
        
        //퀘스트 진행텍스트
        proceedingTxt.text = "진행중";
    }
    
    //퀘스트 완료
    public void CompleteQuest()
    {        
        //현재 열쇠 생성 카운트, 퀘스트 수락 초기화
        produceKeyCount = 0;
        QuestAccept = false;

        //해당씬은 퀘스트가 완료 했다는것을 알려줌
        QuestComplete[GameManager.instance.sceneMgr.curScene - 1] = true;

        //퀘스트 진행텍스트
        proceedingTxt.text = "완료";
        
        //완료버튼, 퀘스트 진행도 텍스트 꺼줌
        completeBtn.SetActive(false);
        questListInfo.SetActive(false);

        //열쇠생성
        KeyInventory keyInven = GameManager.instance.keyInventory;
        keyInven.GetFieldItem();
    }    
}
