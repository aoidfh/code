using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by박준영

[System.Serializable]
public class Item
{
    //아이템 유형 설정
    public enum ItemType
    {
        Null,
        장비,
        사용,
        기타
    }

    //장비아이템 유형 설정
    public enum EquipmentType
    {
        Null,
        모자,
        상의,
        하의,
        망토,
        신발,
        무기
    }

    //아이템의 정보 : 아이템고유번호, 이름, 유형, 설명, 이미지
    public int itemID;
    public string itemName;  
    public ItemType itemType;
    public EquipmentType equipmentType;
    public string itemInfo;
    public float itemEffect;
    public Sprite itemSprite;
     
    //장비아이템 생성자
    public Item(int itemID, string itemName, ItemType itemType, EquipmentType equipmentType, string itemInfo, float itemEffect)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemType = itemType;
        this.equipmentType = equipmentType;
        this.itemInfo = itemInfo;
        this.itemEffect = itemEffect;
        this.itemSprite = Resources.Load(itemID.ToString(), typeof(Sprite)) as Sprite;
    }

    //소비아이템 생성자
    public Item(int itemID, string itemName, ItemType itemType, string itemInfo, float itemEffect)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemType = itemType;
        this.itemInfo = itemInfo;
        this.itemEffect = itemEffect;
        this.itemSprite = Resources.Load(itemID.ToString(), typeof(Sprite)) as Sprite;
    }

    //기타아이템 생성자
    public Item(int itemID, string itemName, ItemType itemType, string itemInfo)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemType = itemType;
        this.itemInfo = itemInfo;
        this.itemSprite = Resources.Load(itemID.ToString(), typeof(Sprite)) as Sprite;        
    }

}
