using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour
{
    public static int Difficulty { get; private set; } = 0; // 0 - Easy, 1 - Average, 2 - Hard

    // �������� ��� ������ ����� ������
    public void SetEasy()
    {
        
        Difficulty = 0;
        SceneManager.LoadScene("textScene");
        Debug.Log("������� ���������� Easy");
    }

    public void SetAverage()
    {
        
        Difficulty = 1;
        SceneManager.LoadScene("textScene");
        Debug.Log("������� ���������� Average");
    }

    public void SetHard()
    {
        
        Difficulty = 2;
        SceneManager.LoadScene("textScene");
        Debug.Log("������� ���������� Hard");
    }
}
