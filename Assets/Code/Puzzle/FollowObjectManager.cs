using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectManager : MonoBehaviour
{
 
    public GameObject followObject; // �հ����� ���� �� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // �� ���� ������Ʈ�� �̸� ����
        followObject.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
    }

    // �� ���� ������Ʈ�� �ܺο��� ������ �� �ְ� ���ִ� �Լ�
    public GameObject GetFollowObject()
    {
        return followObject;
    }

    // �� ���� ������Ʈ�� Ư�� ��ġ�� �̵���Ű�� �Լ�
    public void SetFollowObjectPosition(Vector2 newPosition)
    {
        followObject.transform.position = newPosition;
    }
}
