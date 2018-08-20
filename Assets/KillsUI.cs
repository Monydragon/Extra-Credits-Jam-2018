using UnityEngine;

public class KillsUI : MonoBehaviour
{
    TMPro.TextMeshProUGUI txt;
    public static KillsUI UI { get; private set; }
    int kills;

    void Awake()
    {
        UI = this;
    }

    void Start()
    {
        txt = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void AddKill()
    {
        txt.text = (++kills).ToString();
    }
}
