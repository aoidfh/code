using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//박준영

public class DontDestroy : MonoBehaviour
{
    
    //씬을 옮기더라도 삭제하지 않아야 할 오브젝트
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }  
}
