using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public void GiveFullHEALTH(float health)
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
    }
    public void SetHealth(float health)
    {
       
        healthBarSlider.value = health;
    }

}
