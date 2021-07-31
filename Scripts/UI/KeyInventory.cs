using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영

public class KeyInventory : MonoBehaviour
{
    //슬롯
    public Slot slot;

    private void Start()
    {
        //시작시 슬롯 버튼 비활성화
        slot.GetComponent<Button>().interactable = false;
    }

    //인벤토리에 아이템 생성
    public void GetFieldItem()
    {        
        //열쇠 아이템 생성
        slot.SetItem(GameManager.instance.itemDatabase.ItemList[3]);

        //슬롯 버튼 활성화
        slot.GetComponent<Button>().interactable = true;
    }

    public void UseKey()
    {     
        //아이템 제거
        slot.SetItem(null);
        //버튼 비활성화
        slot.GetComponent<Button>().interactable = false;

        slot.InItem = false;
    }

    //열쇠 슬롯을 누르면 아이템정보 활성화
    public void OnClick()
    {
        GameManager.instance.itemInfo.ItemInfo(slot);
    }
}
