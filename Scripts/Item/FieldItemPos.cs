using UnityEngine;

//박준영

public class FieldItemPos : MonoBehaviour
{
    //필드에 표시될 아이템
    public GameObject fieldItem;
            
    //몬스터가 죽으면 그자리에 필드 아이템 만듦
    public void FieldItemPosition(Vector3 monsterPos)
    {       
        //50% 확률로 아이템 생성   
        int rand = Random.Range(1, 10 +1);

        if (rand <= 5)
        {
            return;
        }
                    
        Instantiate(fieldItem, monsterPos, transform.rotation);                                   
    }   

}
