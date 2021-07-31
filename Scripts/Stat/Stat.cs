using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

[System.Serializable]
public class Stat
{
    //스탯 정보 : 체력, 최대체력, 공격력, 방어력, 데미지
    float hp;
    float maxHp;
    float att;
    float dfs;    

    float damage;

    public float Hp { get => hp; set => hp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Att { get => att; set => att = value; }
    public float Dfs { get => dfs; set => dfs = value; }
    public float Damage { get => damage; set => damage = value; }

    //스탯 생성자
    public Stat(float maxHp, float hp, float att, float dfs)
    {
        this.MaxHp = maxHp;
        this.Hp = hp;
        this.Att = att;
        this.Dfs = dfs;
    }

    public Stat(float maxHp, float hp, float att)
    {
        this.MaxHp = maxHp;
        this.Hp = hp;
        this.Att = att;       
    }

    //회복 물약 먹을 시 체력 계산
    public void SetHp(float hp)
    {
        this.Hp += hp;

        if (this.Hp >= this.MaxHp)
        {
            this.Hp = this.MaxHp;         
        }       
    }

    //피격 당할 시 체력 계산(공격력보다 방어력이 높더라도 무조건 1은 깍이게 만듦)
    public void SetHp(float hitObjDfs, float attackObjAtt)
    {              
        Damage = attackObjAtt - hitObjDfs;

        if (Damage <= 0)
        {
            Damage = 1f;
        }

        this.Hp -= Damage;
    }

    //무기,방어구 아이템 효과(스탯 증가)
    public void EquipmentItemEffect(Item item, float itemEffect)
    {
        if (item.itemType == Item.ItemType.장비)
        {
            if (item.equipmentType == Item.EquipmentType.무기)
            {
                Att += itemEffect;
            }
            else
            {
                Dfs += itemEffect;
            }
        }
    }
}
