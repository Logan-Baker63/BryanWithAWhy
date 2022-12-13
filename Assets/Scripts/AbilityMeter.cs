using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMeter : MonoBehaviour
{
    [SerializeField] Color defaultColour;
    [SerializeField] Color aquiredColour;
    [SerializeField] Color highlightedColour;

    public int abilityPoints = 3;
    AbilityCounter abilityCounter;

    public List<Image> abilitySlots;

    public int GetAbilityPoints() { return abilityPoints; }

    public enum AbilityType
    {
        Design,
        Programming,
        Art,
    }
    public AbilityType abilityType;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform slot in transform)
        {
            if (slot.GetComponent<Image>())
            {
                abilitySlots.Add(slot.GetComponent<Image>());
            }
        }

        abilityCounter = GetComponentInChildren<AbilityCounter>();

        UpdateUses();
    }

    public void AquireAbilityPoints(int _abilityPoinrs)
    {
        abilityPoints += _abilityPoinrs;

        UpdateUses();
    }

    public void SpendAbilityPoints(int _abilityPoints)
    {
        abilityPoints -= _abilityPoints;

        if (abilityPoints < 0)
        {
            abilityPoints = 0;
        }

        UpdateUses();
    }

    public void SetHighlightPoints(int _abilityPoints)
    {
        int startingSlotID = abilityPoints - _abilityPoints;

        for (int i = startingSlotID; i < abilityPoints; i++)
        {
            abilitySlots[i].color = highlightedColour;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUses()
    {
        if (abilityPoints > abilitySlots.Count)
        {
            abilityPoints = abilitySlots.Count;
        }
        abilityCounter.UpdateCounter(abilityPoints);

        foreach (Image img in abilitySlots)
        {
            img.color = defaultColour;
        }
        
        for (int i = 0; i < abilityPoints; i++)
        {
            abilitySlots[i].color = aquiredColour;
        }
    }
}
