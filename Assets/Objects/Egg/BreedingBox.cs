using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreedingBox : MonoBehaviour, IInteractable
{
    private int eggsHatching = 0;
    [SerializeField] private int maxEggsHatching = 3;
    private List<float> eggHatchingTimer;
    [SerializeField] private int maxNumberOhEggs;
    private bool isInteracting;
    private bool isEmptying;
    private float actualIntercationTime;
    [SerializeField] private float interationTime;
    [SerializeField] private SpriteRenderer[] eggs;

    [SerializeField] private Image circleIsReady;
    [SerializeField] private GameObject circle;
    [SerializeField] private float hatchTime = 5;

    private void Start()
    {
        eggHatchingTimer = new List<float>();
        UpdateEggImages();
        actualIntercationTime = interationTime;
        circle.SetActive(false);
    }

    private void Update()
    {
        if (isInteracting)
            Action();
        else if (isEmptying)
            EmptyGauge();
        if (eggHatchingTimer.Count > 0)
        {
            for (int i = 0; i < eggHatchingTimer.Count; i++)
            {
                eggHatchingTimer[i] -= TimeManager.deltaTime;
                if (eggHatchingTimer[i] <= 0)
                {
                    CatManager.Instance.AddNewCat();
                    eggHatchingTimer.RemoveAt(i);
                    eggsHatching--;
                    UpdateEggImages();
                }
            }
        }
    }

    public void HatchNewEgg()
    {
        if (ResourcesManager.Instance.EggNumber > 0)
        {
            ResourcesManager.Instance.RemoveEgg(1);
            eggsHatching++;
            UpdateEggImages();
            eggHatchingTimer.Add(hatchTime);
        }
    }

    private void UpdateEggImages()
    {
        for (int i = 0; i < 3; i++)
        {
            if(eggsHatching > i)
            {
                eggs[i].enabled = true;
            }
            else
            {
                eggs[i].enabled = false;
            }
        }
    }

    public void Interation()
    {
        if (ResourcesManager.Instance.EggNumber <= 0 || eggsHatching >= maxEggsHatching) return;
        circle.SetActive(true);
        isInteracting = true;
        isEmptying = false;
    }

    public void StopInteration()
    {
        isInteracting = false;
        isEmptying = true;
    }

    void Action()
    {
        actualIntercationTime -= Time.deltaTime;
        circleIsReady.fillAmount = 1 - (actualIntercationTime / interationTime);

        if (actualIntercationTime <= 0)
        {
            HatchNewEgg();

            circleIsReady.fillAmount = 0;
            actualIntercationTime = interationTime;

            if (ResourcesManager.Instance.EggNumber <= 0 || eggsHatching >= maxEggsHatching) StopInteration();

        }
    }

    void EmptyGauge()
    {
        actualIntercationTime += Time.deltaTime * 2;
        actualIntercationTime = Mathf.Clamp(actualIntercationTime, 0, interationTime);
        circleIsReady.fillAmount = 1 - (actualIntercationTime / interationTime);


        if (circleIsReady.fillAmount <= 0)
        {
            isEmptying = false;
            circle.SetActive(false);
        }
    }

}
