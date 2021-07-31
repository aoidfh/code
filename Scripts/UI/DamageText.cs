using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//박준영

public class DamageText : MonoBehaviour
{
    //데미지 텍스트 스피드
    readonly float speed = 0.5f;
    //텍스트 삭제 시간
    readonly float destoryTime = 0.8f;
    //데미지 텍스트
    TextMesh textMesh;

    private void Start()
    {
        //시간이 지나면 삭제시킴
        Invoke("DestoryObject", destoryTime);
    }

    void Update()
    {
        //텍스트가 생성되면 위로 올라가게 만듦
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);        
    }     

    //생성되면 데미지만큼 텍스트에 띄움
    public void DamageTxt(float damage)
    {
        textMesh = gameObject.GetComponent<TextMesh>();

        textMesh.text = damage.ToString();       
    }

    //오브젝트 삭제
    void DestoryObject()
    {
        Destroy(gameObject);
    }    
}
