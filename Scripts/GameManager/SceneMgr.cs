using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//박준영

public class SceneMgr : MonoBehaviour
{    
    //게임메뉴
    public GameObject Menu;
    //현재씬
    public int curScene;
    //퀘스트
    public QuestInformation questInfo;

    //다음씬 이동 효과 이미지
    public Image loadingScene;

    //컬러 변경 시간
    float blackTime;
    //초기화 시간
    readonly float resetTime = 100;

    //플레이어
    public GameObject player;
    //로딩판넬
    public GameObject loadingPanel;

    private void Start()
    {
        loadingPanel.SetActive(false);

        blackTime = resetTime;
    }

    private void Update()
    {       
        //초기화, 
        if (blackTime <= 0)
        {            
            StopCoroutine("ChangeColorBlack");
           
            blackTime = resetTime;
            NextScene();
            
            loadingPanel.SetActive(false);
            player.SetActive(true);
        }      
    }

    //메뉴를 활성화 시키면 타임을 멈춰줌, 비활성화시 타임 진행    
    public void OnClickMenu(bool menuActive)
    {
        Menu.SetActive(menuActive);
       
        if (menuActive != true)
        {
            Time.timeScale = 1;

            return;
        }

        Time.timeScale = 0;
    }
  
    //메뉴
    public void OnClickMenu()
    {
        Menu.SetActive(true);
    }

    //메인화면으로 돌아감
    public void TitleScene()
    {        
        SceneManager.LoadScene("TitleGameScene");

        OnClickMenu(false);

        GameObject[] allObjs = FindObjectsOfType<GameObject>();

        for (int i = 0; i < allObjs.Length; i++)
        {
            Destroy(allObjs[i]);
        }              
    }

    //다음씬으로 이동(처음 스타트버튼)
    public void StartGameNextScene(int index)
    {
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.buildIndex + index;

        SceneManager.LoadScene(curScene);
        GameManager.instance.NextSceneActiveObjs(curScene);

        questInfo.Quest(curScene);
    }

    //다음씬으로 이동(포탈로 이동시)
    public void NextGameScene(int index)
    {       
        Scene scene = SceneManager.GetActiveScene();
        curScene = scene.buildIndex + index;
              
        StartCoroutine("ChangeColorBlack");       
    }

    //다음씬으로 이동(포탈로 이동시)
    void NextScene()
    {       
        SceneManager.LoadScene(curScene);        
        questInfo.Quest(curScene);

        loadingScene.color = new Color(0, 0, 0, 0);
    }
  
    //이어하기
    public void Resume()
    {
        OnClickMenu(false);        
    }
    
    //게임 끝내기
    public void GameQuit()
    {        
        Application.Quit();
    }

    //부드럽게 화면 색상을 검정으로 전환
    IEnumerator ChangeColorBlack()
    {                
        loadingPanel.SetActive(true);
        player.SetActive(false);

        //효과시간 감소
        blackTime--;

        loadingScene.color += new Color(0, 0, 0, 0.1f);
               
        yield return new WaitForSeconds(0.02f);

        StartCoroutine("ChangeColorBlack");
    }    
}
