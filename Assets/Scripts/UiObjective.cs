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

    [SerializeField] private GameObject GameOverCanva;


    private float Animate;
    private float AnimateTimer;
    private int ObjMaxHP;

    // Start is called before the first frame update
    void Start()
    {
        GameOverCanva.SetActive(false);
        ObjMaxHP = ObjectiveHP.Life();
        DisplayProgress.fillAmount = (WaveProgress.GetWaveLength() - WaveProgress.GetWaveStep()) * 1f / WaveProgress.GetWaveLength();
        DisplayHP.fillAmount = (ObjectiveHP.Life()) * 1f / ObjMaxHP;
        AnimateTimer = 0.0f;
        Animate = 2.0f;
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
            if(ObjectiveHP.Life() > 0){
                TextDisplay.text = "Completada";
                BigSign.text = "Oleada " + WaveProgress.GetWave() + " en " + WaveProgress.GetState();
                ObjectiveHP.Set(ObjMaxHP);
                DisplayProgress.fillAmount = 1f;
                DisplayHP.fillAmount = (ObjectiveHP.Life()) * 1f / ObjMaxHP;
            }
        }
        if(ObjectiveHP.Life() < 1){
            AnimateTimer += Time.deltaTime;
            if(AnimateTimer > Animate){
                if(!GameOverCanva.activeSelf){
                    GameObject.Find("Player").gameObject.GetComponent<Character_Controller>().KillPermanent();
                    GameOverCanva.SetActive(true);
                }
            }
        }
    }
}
