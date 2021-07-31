using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영

public class UI_Information : MonoBehaviour
{
    //플레이어스탯
    public PlayerCtrl playerStat;

    //아이템정보창
    public GameObject ItemInfoPanel;
    //아이템 이미지
    public Image image;

    //아이템이름 텍스트
    public Text itemNameTxt;
    //아이템타입 텍스트
    public Text itemTypeTxt;
    //아이템설명 텍스트
    public Text itemInfo;

    //포탈 이동불가 텍스트
    public GameObject potalTxt;
    //포탈 버튼 텍스트
    public Text potalButtonTxt;
    //포탈 버튼 이미지
    public Image potalButtonImage;

    //플레이어 체력 슬라이더
    public Slider player_HpSlider;
    //플레이어 체력
    public Text hpText;
    //플레이어 공격력
    public Text attText;
    //플레이어 방어력
    public Text dfsText;

    void Start()
    {
        playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        //시작시 아이템 정보창 끄기
        ItemInfoPanel.SetActive(false);
        potalTxt.SetActive(false);
    }

    void Update()
    {
        //플레이어 체력바 최대 체력
        player_HpSlider.maxValue = playerStat.Stat.MaxHp;
        //플레이어 체력바
        player_HpSlider.value = playerStat.Stat.Hp;
        //플레이어 체력, 공격력, 방어력
        hpText.text = playerStat.Stat.MaxHp.ToString() + " / " + playerStat.Stat.Hp.ToString();
        attText.text = "공격력 : " + playerStat.Stat.Att.ToString();
        dfsText.text = "방어력 : " + playerStat.Stat.Dfs.ToString();
    }

    //아이템 정보창 켜기, 아이템 정보 나열(아이템 이름, 유형, 설명, 이미지)
    public void ItemInfo(Slot slot)
    {
        ItemInfoPanel.SetActive(true);

        itemNameTxt.text = slot.Item.itemName;
        itemTypeTxt.text = slot.Item.itemType + "";
        itemInfo.text = slot.Item.itemInfo;

        image.sprite = Resources.Load(slot.Item.itemID.ToString(), typeof(Sprite)) as Sprite;      
    }    
        
    //아이템 정보창 끄기
    //아이템 정보창 눌리면 끄게 만듦
    public void CancelIteminfo()
    {
        ItemInfoPanel.SetActive(false);       
    }             
    
    //포탈 UI 이미지,텍스트
    public void PotalButtonTxt(string sprite, string text)
    {       
        potalButtonImage.sprite = Resources.Load(sprite, typeof(Sprite)) as Sprite;

        potalButtonTxt.text = text;
    }
}
