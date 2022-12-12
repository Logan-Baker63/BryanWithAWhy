using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMeter : MonoBehaviour
{
    [SerializeField] Color defaultColour;
    [SerializeField] Color aquiredColour;
    int abilityUses = 0;
    AbilityCounter abilityCounter;

    [SerializeField] List<Image> abilitySlots;

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
    }

    public void AquireAbilityUses(int _abilityUses)
    {
        abilityUses += _abilityUses;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUses();
    }

    void UpdateUses()
    {
        abilityCounter.UpdateCounter(abilityUses);

        if (abilityUses > abilitySlots.Count)
        {
            abilityUses = abilitySlots.Count;
        }
        
        foreach (Image img in abilitySlots)
        {
            img.color = defaultColour;
        }
        
        for (int i = 0; i < abilityUses; i++)
        {
            abilitySlots[i].color = aquiredColour;
        }
    }
}
