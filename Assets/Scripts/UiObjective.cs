using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiObjective : MonoBehaviour
{

    [SerializeField] private LifeManager ObjectiveHP;
    [SerializeField] private EnemyTracker WaveProgress;

    [SerializeField] private Image DisplayHP;
    [SerializeField] private Image DisplayProgress;

    [SerializeField] private TMPro.TextMeshProUGUI TextDisplay;
    [SerializeField] private TMPro.TextMeshProUGUI BigSign;


    private int ObjMaxHP;

    // Start is called before the first frame update
    void Start()
    {
        ObjMaxHP = ObjectiveHP.Life();
        DisplayProgress.fillAmount = (WaveProgress.GetWaveLength() - WaveProgress.GetWaveStep()) * 1f / WaveProgress.GetWaveLength();
        DisplayHP.fillAmount = (ObjectiveHP.Life()) * 1f / ObjMaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(WaveProgress.GetState() == 0){
            BigSign.text = "";
            DisplayProgress.fillAmount = (WaveProgress.GetWaveLength() - WaveProgress.GetWaveStep() - 1) * 1f / WaveProgress.GetWaveLength();
            DisplayHP.fillAmount = (ObjectiveHP.Life()) * 1f / ObjMaxHP;
            TextDisplay.text = "Oleada " + (WaveProgress.GetWave());
        }else{
            TextDisplay.text = "Completada";
            BigSign.text = "Oleada " + WaveProgress.GetWave() + " en " + WaveProgress.GetState();
            ObjectiveHP.Set(ObjMaxHP);
            DisplayProgress.fillAmount = 1f;
            DisplayHP.fillAmount = (ObjectiveHP.Life()) * 1f / ObjMaxHP;
        }
    }
}
