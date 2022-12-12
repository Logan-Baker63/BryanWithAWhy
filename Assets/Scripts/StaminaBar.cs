using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    PlayerAttack playerAttack;
    Slider staminaBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        staminaBarSlider = GetComponent<Slider>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        staminaBarSlider.value = playerAttack.currentStamina / playerAttack.maxStamina;
    }
}
