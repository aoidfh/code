using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class ItemDatabase : MonoBehaviour
{
    [SerializeField]
    List<Item> itemList = new List<Item>();

    public List<Item> ItemList { get => itemList; set => itemList = value; }

    void Start()
    {
        //아이템베이스에 아이템 생성
        ItemList.Add(new Item(1001, "체력회복물약", Item.ItemType.사용,                         "아이템을 사용시 체력이 50 회복이됩니다.(더블 클릭시 아이템 사용가능)", 50f));
        ItemList.Add(new Item(1002, "강철검",      Item.ItemType.장비, Item.EquipmentType.무기, "아이템을 들고 있을시 공격력이 10 상승합니다.",                       10f));
        ItemList.Add(new Item(1003, "가죽옷",      Item.ItemType.장비, Item.EquipmentType.상의, "아이템을 들고 있을시 방어력이 10 상승합니다.",                       10f));
        ItemList.Add(new Item(1004, "보석",        Item.ItemType.기타,                          "다음 스테이지로 갈 수 있게 해주는 열쇠입니다."));
    }   
}
