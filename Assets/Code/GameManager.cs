using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PuzzleManager puzzleManager;
    private UIManager uiManager;
    //private AdjustRectTransform adjustRectTransform; // AdjustRectTransform ������Ʈ
    public AdjustRectTransform adjustRectTransform;

    void Start()
    {
        //adjustRectTransform = new AdjustRectTransform();
        // AdjustRectTransform�� ���� �����Ͽ� ���� �θ� RectTransform�� ũ�� ����
        adjustRectTransform.AdjustRectSize();

        // PuzzleManager�� UIManager �ʱ�ȭ
        puzzleManager = new PuzzleManager();
        uiManager = new UIManager();

        // ���� �ʱ�ȭ
        //puzzleManager.InitializePuzzle();
    }

    void Update()
    {
        // ���� ���� ������Ʈ
        //puzzleManager.CheckPuzzleStatus();
    }
}
