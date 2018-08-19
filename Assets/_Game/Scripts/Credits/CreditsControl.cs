using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsControl : MonoBehaviour
{
    public float speed = 10, stopCreditsYPos = 175;
    RectTransform rect;
    
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + speed * Time.deltaTime);

        if (rect.anchoredPosition.y >= stopCreditsYPos)
            Application.Quit();
    }
}
