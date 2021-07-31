using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//박준영

//버튼을 누르면 오브젝트가 작았다가 커짐(눌린것처럼 보이기 위함)
public class UI_ClickMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{    
    public void OnPointerDown(PointerEventData eventData)
    {       
        transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
    }
   
    public void OnPointerUp(PointerEventData eventData)
    {       
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }
}
