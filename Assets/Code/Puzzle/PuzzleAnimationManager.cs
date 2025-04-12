using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleAnimationManager : MonoBehaviour
{
    // 퍼즐 조각을 이동시키는 메서드
    public IEnumerator MovePiece(PuzzlePiece puzzlePiece, int targetX, int targetY, float fillTime, GridLayoutGroup gridLayoutGroup)
    {
        RectTransform pieceRectTransform = puzzlePiece.GetComponent<RectTransform>();
        Vector2 startPosition = pieceRectTransform.anchoredPosition;

        // 목표 위치를 계산할 때 GridLayoutGroup을 전달
        Vector2 targetPosition = GetPosition(targetX, targetY, gridLayoutGroup);

        float elapsedTime = 0f;

        while (elapsedTime < fillTime)
        {
            pieceRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / fillTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 위치를 정확히 설정
        pieceRectTransform.anchoredPosition = targetPosition;
    }

    // 퍼즐의 그리드 좌표를 화면의 앵커 좌표로 변환
    public Vector2 GetPosition(int x, int y, GridLayoutGroup gridLayoutGroup)
    {
        return new Vector2(
            (x - 2.5f) * gridLayoutGroup.cellSize.x, // X축 위치
            -(y + 1 - 3.5f) * gridLayoutGroup.cellSize.y  // Y축 위치
        );
    }

    // 퍼즐이 위에서 떨어져 생성되는 효과 (PuzzleManager로부터 호출)
    /*public IEnumerator FillPuzzleWithFallEffect(PuzzleManager puzzleManager, GridLayoutGroup gridLayoutGroup, List<PuzzleSpriteManager.PuzzleColor> selectedColors)
    {
        int gridSize = puzzleManager.gridSize;
        PuzzlePiece[,] puzzlePieces = puzzleManager.puzzlePieces;

        for (int y = gridSize - 1; y >= 0; y--)
        {
            for (int x = gridSize - 1; x >= 0; x--)
            {
                GameObject piece = Instantiate(puzzleManager.puzzlePiecePrefab, gridLayoutGroup.transform);
                PuzzlePiece puzzlePiece = piece.GetComponent<PuzzlePiece>();

                // 퍼즐 조각에 랜덤 색상 설정
                PuzzleSpriteManager.PuzzleColor randomColor = selectedColors[Random.Range(0, selectedColors.Count)];
                //puzzlePiece.SetPiece(x, y, puzzleManager.puzzleSpriteManager.GetColor(randomColor));
                //puzzlePiece.Initialize(puzzleManager, x, y); // 초기화 (x, y 좌표 할당)

                RectTransform pieceRectTransform = piece.GetComponent<RectTransform>();
                pieceRectTransform.sizeDelta = new Vector2(120, 120);

                Vector2 startPosition = new Vector2(
                    gridLayoutGroup.GetComponent<RectTransform>().rect.width - (gridSize - x + 2.5f) * gridLayoutGroup.cellSize.x,
                    gridLayoutGroup.GetComponent<RectTransform>().rect.height + (gridSize - y) * gridLayoutGroup.cellSize.y
                );
                pieceRectTransform.anchoredPosition = startPosition;

                puzzlePieces[x, y] = puzzlePiece;

                // MovePiece 호출 시 GridLayoutGroup 전달
                StartCoroutine(MovePiece(puzzlePiece, x, y, 1f, gridLayoutGroup));

                yield return new WaitForSeconds(0.05f);
            }
        }
    }*/

}
