using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpriteManager : MonoBehaviour
{
    // 색상을 정의하는 enum
    public enum PuzzleColor
    {
        Red,
        Yellow,
        Green,
        Blue,
        Purple,
        Magenta,
        White
    }

    // 7가지 색상을 미리 정의
    /*public Color[] availableColors = {
        Color.red, Color.yellow, Color.green, Color.blue,
        new Color(0.5f, 0f, 1f), // 보라색
        Color.magenta, Color.white
    };*/

    // enum에 해당하는 Color 값을 딕셔너리로 매핑
    private Dictionary<PuzzleColor, Color> colorMapping = new Dictionary<PuzzleColor, Color>()
    {
        { PuzzleColor.Red,  new Color(1f, 0.45f, 0.45f)},//빨간색 Color.red
        { PuzzleColor.Yellow,  new Color(1f, 0.95f, 0.45f)},//노란색 Color.yellow
        { PuzzleColor.Green,  new Color(0.5f, 1f, 0.5f)},//초록색 Color.green
        { PuzzleColor.Blue,  new Color(0.5f, 0.7f, 1f)},//파란색 Color.blue
        { PuzzleColor.Purple, new Color(0.5f, 0f, 1f) }, // 보라색
        { PuzzleColor.Magenta,  new Color(1f, 0.5f, 1f)},//분홍색 Color.magenta
        { PuzzleColor.White, Color.white }//흰색
    };

    // 색상별 점수 매핑
    private Dictionary<PuzzleColor, int> colorScores = new Dictionary<PuzzleColor, int>()
    {
        { PuzzleColor.Red,  100 },
        { PuzzleColor.Yellow,  200 },
        { PuzzleColor.Green,  300 },
        { PuzzleColor.Blue,  400 },
        { PuzzleColor.Purple,  500 },
        { PuzzleColor.Magenta,  600 },
        { PuzzleColor.White,  700 }
    };

    // 7가지 색상 중에서 5가지 색상을 랜덤으로 선택
    public List<PuzzleColor> SelectRandomColors(int numberOfColors)
    {
        //List<Color> allColors = new List<Color>(availableColors); // 모든 색상 가져오기
        //List<Color> selectedColors = new List<Color>();
        List<PuzzleColor> allColors = new List<PuzzleColor>((PuzzleColor[])System.Enum.GetValues(typeof(PuzzleColor))); // 모든 enum 값 가져오기       
        List<PuzzleColor> selectedColors = new List<PuzzleColor>();

        while (selectedColors.Count < numberOfColors)
        {
            //Color randomColor = allColors[Random.Range(0, allColors.Count)];
            PuzzleColor randomColor = allColors[Random.Range(0, allColors.Count)];
            if (!selectedColors.Contains(randomColor))
            {
                selectedColors.Add(randomColor);
            }
        }
        return selectedColors;
    }

    // enum에 맞는 Color 값을 반환하는 함수
    public Color GetColor(PuzzleColor puzzleColor)
    {
        return colorMapping[puzzleColor];
    }

    // 점수를 계산하고 반환하는 메서드
    public int GetScoreForColor(PuzzleColor puzzleColor)
    {
        return colorScores[puzzleColor];
    }
}
