using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMatchChecker : MonoBehaviour
{
    private PuzzleManager puzzleManager;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>(); // PuzzleManager 참조
    }

    // 퍼즐 조각의 x, y 좌표를 받아서 주변의 같은 색 퍼즐들을 탐색
    public void CheckForMatches(int startX, int startY)
    {
        // 탐색 범위 조건: y는 6 이상만 탐색
        if (startY < 6)
            return;

        PuzzlePiece startPiece = puzzleManager.puzzlePieces[startX, startY];
        if (startPiece == null || !startPiece.pieceImage.enabled)
            return;

        List<PuzzlePiece> matchingPieces = new List<PuzzlePiece>();
        matchingPieces.Add(startPiece);

        // 상하좌우 방향 탐색
        CheckAdjacentPieces(startX, startY, matchingPieces, startPiece.pieceImage.color);

        // 총 4개 이상의 퍼즐 조각이 있다면 비활성화 처리
        if (matchingPieces.Count >= 4)
        {
            foreach (var piece in matchingPieces)
            {
                piece.pieceImage.enabled = false;
            }
        }
    }

    // 상하좌우로 같은 색 퍼즐을 재귀적으로 탐색
    private void CheckAdjacentPieces(int x, int y, List<PuzzlePiece> matchingPieces, Color targetColor)
    {
        // 상하좌우 좌표
        int[] xOffset = { 0, 0, -1, 1 };
        int[] yOffset = { -1, 1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = x + xOffset[i];
            int newY = y + yOffset[i];

            // 퍼즐 범위를 벗어나는지 체크 + y 범위를 6 이상으로 제한
            if (newX >= 0 && newX < puzzleManager.gridWidth && newY >= 6 && newY < puzzleManager.gridHeight)
            {
                PuzzlePiece adjacentPiece = puzzleManager.puzzlePieces[newX, newY];

                // 같은 색의 퍼즐 조각인지 체크하고, 아직 탐색되지 않은 퍼즐만 처리
                if (adjacentPiece != null && adjacentPiece.pieceImage.enabled && adjacentPiece.pieceImage.color == targetColor && !matchingPieces.Contains(adjacentPiece))
                {
                    matchingPieces.Add(adjacentPiece);
                    CheckAdjacentPieces(newX, newY, matchingPieces, targetColor); // 재귀 호출
                }
            }
        }
    }
}
