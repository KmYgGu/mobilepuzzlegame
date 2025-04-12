using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpriteManager : MonoBehaviour
{
    // ������ �����ϴ� enum
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

    // 7���� ������ �̸� ����
    /*public Color[] availableColors = {
        Color.red, Color.yellow, Color.green, Color.blue,
        new Color(0.5f, 0f, 1f), // �����
        Color.magenta, Color.white
    };*/

    // enum�� �ش��ϴ� Color ���� ��ųʸ��� ����
    private Dictionary<PuzzleColor, Color> colorMapping = new Dictionary<PuzzleColor, Color>()
    {
        { PuzzleColor.Red,  new Color(1f, 0.45f, 0.45f)},//������ Color.red
        { PuzzleColor.Yellow,  new Color(1f, 0.95f, 0.45f)},//����� Color.yellow
        { PuzzleColor.Green,  new Color(0.5f, 1f, 0.5f)},//�ʷϻ� Color.green
        { PuzzleColor.Blue,  new Color(0.5f, 0.7f, 1f)},//�Ķ��� Color.blue
        { PuzzleColor.Purple, new Color(0.5f, 0f, 1f) }, // �����
        { PuzzleColor.Magenta,  new Color(1f, 0.5f, 1f)},//��ȫ�� Color.magenta
        { PuzzleColor.White, Color.white }//���
    };

    // ���� ���� ����
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

    // 7���� ���� �߿��� 5���� ������ �������� ����
    public List<PuzzleColor> SelectRandomColors(int numberOfColors)
    {
        //List<Color> allColors = new List<Color>(availableColors); // ��� ���� ��������
        //List<Color> selectedColors = new List<Color>();
        List<PuzzleColor> allColors = new List<PuzzleColor>((PuzzleColor[])System.Enum.GetValues(typeof(PuzzleColor))); // ��� enum �� ��������       
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

    // enum�� �´� Color ���� ��ȯ�ϴ� �Լ�
    public Color GetColor(PuzzleColor puzzleColor)
    {
        return colorMapping[puzzleColor];
    }

    // ������ ����ϰ� ��ȯ�ϴ� �޼���
    public int GetScoreForColor(PuzzleColor puzzleColor)
    {
        return colorScores[puzzleColor];
    }
}
