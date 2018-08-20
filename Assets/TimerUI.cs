using UnityEngine;

public class TimerUI : MonoBehaviour
{
    System.TimeSpan t;
    TMPro.TextMeshProUGUI txt;
    public static TimerUI UI { get; private set; }
    bool stop;

    void Awake()
    {
        UI = this;
        txt = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();   
    }

    void Update()
    {
        if (stop)
            return;

        t = new System.TimeSpan(0,0, (int)Time.timeSinceLevelLoad);
        txt.text = $"Time: {t.Minutes}:{t.Seconds}";
    }

    public void Stop()
    {
        stop = true;
    }
}
