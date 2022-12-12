using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMeter : MonoBehaviour
{
    [SerializeField] Color defaultColour;
    [SerializeField] Color aquiredColour;
    int abilityUses = 0;

    [SerializeField] List<Image> abilitySlots;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform slot in transform)
        {
            abilitySlots.Add(slot.GetComponent<Image>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUses();
    }

    void UpdateUses()
    {
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
