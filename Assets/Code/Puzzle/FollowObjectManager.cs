using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectManager : MonoBehaviour
{
 
    public GameObject followObject; // 손가락을 따라갈 빈 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        // 빈 게임 오브젝트를 미리 생성
        followObject.SetActive(false); // 초기에는 비활성화
    }

    // 빈 게임 오브젝트를 외부에서 접근할 수 있게 해주는 함수
    public GameObject GetFollowObject()
    {
        return followObject;
    }

    // 빈 게임 오브젝트를 특정 위치로 이동시키는 함수
    public void SetFollowObjectPosition(Vector2 newPosition)
    {
        followObject.transform.position = newPosition;
    }
}
