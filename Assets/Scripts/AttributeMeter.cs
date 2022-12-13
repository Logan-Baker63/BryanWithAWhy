using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeMeter : MonoBehaviour
{
    [SerializeField] Color defaultColour;
    [SerializeField] Color aquiredColour;
    [SerializeField] Color highlightedColour;

    public int attributePoints = 0;
    public Text attributeCounter;

    [HideInInspector] public int slotsHighlighted;

    [SerializeField] List<Image> attributeSlots;
    [SerializeField] AttributeConfirm_Reset overlordScript;
    [SerializeField] AbilityMeter designMeter;

    public int GetAttributePoints() { return attributePoints; }

    public enum AttributeType
    {
        MaxHP,
        MoveSpeed,
        ProjectileSpeed,
        RangedDamage,
        ProjectileSize,
        FireRate,
        ProjectileSpread,
        ProjectileAmount,
        MeleeDamage,
        Stamina,
        DodgeSpeed,
        DodgeInvulnerability
    }
    public AttributeType attributeType;

    // Start is called before the first frame update
    void Awake()
    {
        overlordScript = FindObjectOfType<AttributeConfirm_Reset>();

        foreach(AbilityMeter meter in FindObjectsOfType<AbilityMeter>())
        {
            if (meter.abilityType == AbilityMeter.AbilityType.Design)
            {
                designMeter = meter;
            }
        }

        foreach (Transform slot in transform)
        {
            if (slot.GetComponent<Image>())
            {
                attributeSlots.Add(slot.GetComponent<Image>());
            }
        }

        attributeCounter = GetComponentInChildren<Text>();

        UpdateUI();
    }

    public void SpendPotentialPoint()
    {
        if(overlordScript.potentialPoints > 0 && attributePoints + slotsHighlighted < attributeSlots.Count)
        {
            slotsHighlighted++;
            SetHighlightPoints(attributePoints + slotsHighlighted);
            overlordScript.potentialPoints--;
            overlordScript.UpdatePointDisplay();
        }
    }

    public void ResetPointClaim()
    {
        int startingSlotID = attributePoints + slotsHighlighted - 1;
        for (int i = startingSlotID; i >= attributePoints; i--)
        {
            attributeSlots[i].color = defaultColour;
        }
        overlordScript.potentialPoints += slotsHighlighted;
        overlordScript.UpdatePointDisplay();
        slotsHighlighted = 0;
        UpdateUI();
    }

    public void AcquireAttributePoints(int pointsToGain)
    {
        attributePoints += pointsToGain;

        UpdateUI();
    }

    public void SpendAttributePoints()
    {
        AcquireAttributePoints(slotsHighlighted);
        designMeter.abilityPoints -= slotsHighlighted;
        ResetPointClaim();

        if (attributePoints < 0)
        {
            attributePoints = 0;
        }

        switch (attributeType)
        {
            case AttributeType.MaxHP:
                GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().maxHealth += 40;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().currentHealth += 40;
                return;
            case AttributeType.MoveSpeed:
                return;
            case AttributeType.ProjectileSpeed:
                return;
            case AttributeType.RangedDamage:
                return;
            case AttributeType.ProjectileSize:
                return;
            case AttributeType.FireRate:
                return;
            case AttributeType.ProjectileSpread:
                return;
            case AttributeType.ProjectileAmount:
                return;
            case AttributeType.MeleeDamage:
                return;
            case AttributeType.Stamina:
                return;
            case AttributeType.DodgeSpeed:
                return;
            case AttributeType.DodgeInvulnerability:
                return;
        }

        UpdateUI();
    }

    public void SetHighlightPoints(int pointInvestment)
    {
        int startingSlotID = attributePoints;

        for (int i = startingSlotID; i < pointInvestment; i++)
        {
            attributeSlots[i].color = highlightedColour;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        if (attributePoints > attributeSlots.Count)
        {
            attributePoints = attributeSlots.Count;
        }
        attributeCounter.text = (attributePoints + slotsHighlighted).ToString();

        foreach (Image img in attributeSlots)
        {
            img.color = defaultColour;
        }
        
        for (int i = 0; i < attributePoints; i++)
        {
            attributeSlots[i].color = aquiredColour;
        }
    }
}
