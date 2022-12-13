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
        if(startingSlotID > attributeSlots.Count - 1)
        {
            startingSlotID = attributeSlots.Count - 1;
        }
        for (int i = startingSlotID; i >= attributePoints; i--)
        {
            attributeSlots[i].color = defaultColour;
        }
        overlordScript.potentialPoints = designMeter.abilityPoints;
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
        int toAdd = slotsHighlighted;
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
                for(int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().maxHealth += 40;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().currentHealth += 40;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().healthBar.UpdateHealthBar();
                }
                return;
            case AttributeType.MoveSpeed:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().movementSpeed += 28;
                }
                return;
            case AttributeType.ProjectileSpeed:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().bulletSpeed += 80;
                }
                return;
            case AttributeType.RangedDamage:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().bulletDamage += 3;
                }
                return;
            case AttributeType.ProjectileSize:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().bulletScaleMultiplier += 0.18f;
                }
                return;
            case AttributeType.FireRate:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().shootCooldown -= 0.04f;
                }
                return;
            case AttributeType.ProjectileSpread:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().spreadAngle -= 3.5f;
                }
                return;
            case AttributeType.ProjectileAmount:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().bulletAmount += 1;
                }
                return;
            case AttributeType.MeleeDamage:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().meleeDamage += 4.5f;
                }
                return;
            case AttributeType.Stamina:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().maxStamina += 2;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().currentStamina += 2;
                }
                return;
            case AttributeType.DodgeSpeed:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().rollSpeed -= 0.019f;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().rollCooldown -= 0.027f;
                }
                return;
            case AttributeType.DodgeInvulnerability:
                for (int i = 0; i < toAdd; i++)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().rollInv += 0.04f;
                }
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
