using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//박준영

public class Slot : MonoBehaviour
{   
    [SerializeField]
    //아이템
    Item item;
    [SerializeField]
    //슬롯에 아이템이 있는지..
    bool inItem;
    //아이템이미지
    Image image;

    PlayerCtrl player;

    public Item Item { get => item; set => item = value; }
    public bool InItem { get => inItem; set => inItem = value; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

        image = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void SetItem(Item item)
    {
        this.Item = item;

        //slot안에 아이템이 없다면..
        if (this.Item == null)
        {
            image.enabled = false;

            gameObject.name = "Empty";
            //슬롯안에 아이템이 있는지..
            InItem = false;

            return;
        }

        //슬롯안에 아이템이 있는지..
        InItem = true;

        image.enabled = true;

        gameObject.name = item.itemName;
        image.sprite = item.itemSprite;
        
        //장비아이템 스탯증가
        player.Stat.EquipmentItemEffect(item, item.itemEffect);
        //이미지 색상 변경(알파값 0 -> 255)
        image.color = new Color(255, 255, 255, 255);
       
    }
}
