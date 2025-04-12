using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PuzzleManager puzzleManager;
    private UIManager uiManager;
    //private AdjustRectTransform adjustRectTransform; // AdjustRectTransform 컴포넌트
    public AdjustRectTransform adjustRectTransform;

    void Start()
    {
        //adjustRectTransform = new AdjustRectTransform();
        // AdjustRectTransform을 먼저 실행하여 퍼즐 부모 RectTransform의 크기 조정
        adjustRectTransform.AdjustRectSize();

        // PuzzleManager와 UIManager 초기화
        puzzleManager = new PuzzleManager();
        uiManager = new UIManager();

        // 퍼즐 초기화
        //puzzleManager.InitializePuzzle();
    }

    void Update()
    {
        // 게임 상태 업데이트
        //puzzleManager.CheckPuzzleStatus();
    }
}
