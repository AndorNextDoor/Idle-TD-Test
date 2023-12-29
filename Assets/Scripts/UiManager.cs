using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveTimerText;


    private void Awake()
    {
        instance = this;
    }

    public void SetLivesText(int amount)
    {
        livesText.text = amount.ToString();
    }

    public void SetWaveTimer(int time)
    {
        waveTimerText.gameObject.SetActive(true);
        waveTimerText.text = time.ToString();
    
        if(time <= 0)
        {
            waveTimerText.gameObject.SetActive(false);
        }
    }

}
