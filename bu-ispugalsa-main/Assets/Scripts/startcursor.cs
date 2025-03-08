using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startcursor : MonoBehaviour
{
    void Start()
{
    Cursor.visible = true; // Делаем курсор видимым
    Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
}
}
