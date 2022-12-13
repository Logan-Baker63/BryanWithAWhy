using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeConfirm_Reset : MonoBehaviour
{
    [SerializeField] DevMode devMode;

    public int potentialPoints;
    [SerializeField] List<AttributeMeter> attributeMeters;
    [SerializeField] AbilityMeter designMeter;
    [SerializeField] Text pointDisplay;

    // Start is called before the first frame update
    void Awake()
    {
        devMode = FindObjectOfType<DevMode>();
        AttributeMeter[] potentialMeters = FindObjectsOfType<AttributeMeter>();
        foreach(AttributeMeter meter in potentialMeters)
        {
            attributeMeters.Add(meter);
            meter.slotsHighlighted = 0;
        }

        foreach (AbilityMeter meter in FindObjectsOfType<AbilityMeter>())
        {
            if (meter.abilityType == AbilityMeter.AbilityType.Design)
            {
                designMeter = meter;
            }
        }

        pointDisplay = GameObject.FindGameObjectWithTag("PointDisplay").GetComponent<Text>();
    }

    public void ResetButton()
    {
        foreach(AttributeMeter meter in attributeMeters)
        {
            meter.ResetPointClaim();
        }
    }

    public void ConfirmButton()
    {
        foreach(AttributeMeter meter in attributeMeters)
        {
            if(meter.slotsHighlighted > 0)
            {
                meter.SpendAttributePoints();
            }
        }
        potentialPoints = designMeter.abilityPoints;
        UpdatePointDisplay();
        designMeter.UpdateUses();
        devMode.ExitDevMode();
    }

    public void UpdatePointDisplay()
    {
        pointDisplay.text = potentialPoints.ToString();
    }

    public void PotentialPointsReset() //Call whenever opening design UI
    {
        potentialPoints = designMeter.abilityPoints;
        UpdatePointDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
