using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영

public class Inventroy : MonoBehaviour
{
    //slot개수
    public Transform slotRoot;
    //인벤토리 슬롯
    List<Slot> slots;
    //슬롯아이템 삭제버튼
    List<Button> removeSlots;

    //인벤토리안 아이템카운트
    int inItemCount;
    //인벤토리 아이템 초과
    bool invenItemFull;

    //아이템버튼 간격시간
    float clickInterval;
    //아이템버튼 초기화시간
    readonly float resetClickInterval = 0.2f;

    //아이템이 클릭되었는지..   
    bool onClick;    

    //아이템버튼 위치(몇번째의 버튼을 눌렸는가..)
    int btnIndex;
    //아이템버튼 위치 초기화
    readonly int resetBtnIndex = 5;

    //플레이어
    PlayerCtrl player;
    
    public bool InvenItemFull { get => invenItemFull; set => invenItemFull = value; }    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        //클릭간의 간격시간, 아이템버튼위치 초기화
        clickInterval = resetClickInterval;
        btnIndex = resetBtnIndex;

        slots = new List<Slot>();
        removeSlots = new List<Button>();      
        
        for (int i = 0; i < slotRoot.childCount; i++)
        {
            var slot = slotRoot.GetChild(i).GetComponent<Slot>();
            var removeSlot = slotRoot.GetChild(i).GetChild(1).gameObject.GetComponent<Button>();
            
            // 슬롯 생성시 아이템이 없기 때문에 버튼 끔
            slot.GetComponent<Button>().interactable = false;
            removeSlot.GetComponent<Button>().interactable = false;

            slots.Add(slot);
            removeSlots.Add(removeSlot);          
        }      
    }

    private void Update()
    {
        //인벤토리의 아이템이 클릭이 되면..
        if (onClick)
        {
            clickInterval -= Time.deltaTime;

            //더블 클릭 되지 않았다면..
            if (clickInterval <= 0)
            {
                onClick = false;

                //한번만 클릭이 되었다면 아이템정보창 켜기
                GameManager.instance.itemInfo.ItemInfo(slots[btnIndex]);                

                //클릭시간 간격과, 아이템버튼위치 초기화
                clickInterval = resetClickInterval;
                btnIndex = resetBtnIndex;
            }
        }       
    }

    //인벤토리에 아이템 생성
    public void GetFieldItem(int itemID)
    {
        ItemDatabase itemDB = GameManager.instance.itemDatabase;

        //아이템데이터베이스 개수만큼 반복
        for (int i = 0; i < itemDB.ItemList.Count; i++)
        {
            if (itemDB.ItemList[i].itemID == itemID)
            {
                //인벤토리 슬롯개수만큼 반복
                for (int j = 0; j < slots.Count; j++)
                {                   
                    if (slots[j].InItem != true)
                    {                     
                        inItemCount++;

                        //인벤토리의 아이템 개수 초과 체크                        
                        if (inItemCount >= slots.Count)
                        {
                            InvenItemFull = true;
                        }
                        
                        //버튼활성화
                        ItemButtonActive(j, true);

                        //아이템 생성
                        slots[j].SetItem(itemDB.ItemList[i]);

                        return;
                    }
                }
            }
        }
    }
       
    //더블클릭시 아이템 사용
    public void DoubleClickUseItem(int index)
    {
        onClick = true;        
       
        //더블클릭시 처음 클릭한 버튼과 두번째 클릭한 버튼이 같고, 아이템 타입이 사용 아이템이라면..
        if (btnIndex == index && slots[index].Item.itemType == Item.ItemType.사용)
        {
            onClick = false;
       
            GameManager.instance.AudioSource(gameObject);

            //아이템 사용효과            
            player.Stat.SetHp(slots[index].Item.itemEffect);
     
            RemoveItem(index);

            //클릭시간 간격과, 아이템버튼위치 초기화
            clickInterval = resetClickInterval;
            btnIndex = resetBtnIndex;

            return;
        }

        //처음 클릭한 버튼과 두번째 클릭한 버튼이 같은지 비교하기 위해..
        btnIndex = index;
    }
   
    //아이템 제거
    public void RemoveItem(int num)
    {
        //아이템 카운트 감소
        inItemCount--;

        //인벤토리의 아이템 개수 초과 체크                        
        if (inItemCount < slots.Count)
        {
            InvenItemFull = false;
        }

        //장비아이템 스탯증가
        player.Stat.EquipmentItemEffect(slots[num].Item, -slots[num].Item.itemEffect);

        //버튼 비활성화
        ItemButtonActive(num, false);
        //아이템 제거
        slots[num].SetItem(null);
    }

    //아이템 버튼 활성화,비활성화
    void ItemButtonActive(int index, bool active)
    {
        slots[index].GetComponent<Button>().interactable = active;
        removeSlots[index].GetComponent<Button>().interactable = active;
    }    
}
