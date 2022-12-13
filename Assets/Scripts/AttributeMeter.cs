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

    [HideInInspector] int slotsHighlighted;
    [HideInInspector] int potentialPoints;

    [SerializeField] List<Image> attributeSlots;

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
    void Start()
    {
        foreach (Transform slot in transform)
        {
            if (slot.GetComponent<Image>())
            {
                attributeSlots.Add(slot.GetComponent<Image>());
            }
        }

        attributeCounter = GetComponentInChildren<Text>();

        UpdateUses();
    }

    public void SpendPotentialPoint()
    {
        slotsHighlighted++;
        SetHighlightPoints(slotsHighlighted);
        potentialPoints--;
    }

    public void ResetPointClaim(int pointInvestment)
    {
        slotsHighlighted = 0;
        int startingSlotID = attributePoints - pointInvestment;
        for (int i = startingSlotID; i < attributePoints; i++)
        {
            attributeSlots[i].color = defaultColour;
        }
    }

    public void AquireAttributePoints(int _abilityPoinrs)
    {
        attributePoints += _abilityPoinrs;

        UpdateUses();
    }

    public void SpendAttributePoints(int _abilityPoints)
    {
        attributePoints -= _abilityPoints;

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

        potentialPoints = attributePoints;
        UpdateUses();
    }

    public void SetHighlightPoints(int pointInvestment)
    {
        int startingSlotID = attributePoints - pointInvestment;

        for (int i = startingSlotID; i < attributePoints; i++)
        {
            attributeSlots[i].color = highlightedColour;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUses()
    {
        if (attributePoints > attributeSlots.Count)
        {
            attributePoints = attributeSlots.Count;
        }
        attributeCounter.text = potentialPoints.ToString();

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
