using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalSlider : MonoBehaviour
{

    public Slider slider;
    public int targetScore = 4;
    public int damage;
    public bool hit;
    public float hitAmount = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hit)
        {
            slider.value += hitAmount;
            if(hitAmount > 0)
                hitAmount -= 0.005f;

            if(hitAmount <= 0)
            {
                hit = false;
                hitAmount = 0f;
            }

            if (slider.value == slider.maxValue)
                hit = false;
        }

        if (!hit)
        {
            slider.value -= 0.02f;
        }

    }

    public void setTargetScore(int targetScore)
    {
        this.targetScore = targetScore;
        // slider.maxValue = targetScore;
    }

    public void setDamage(int damage)
    {
        slider.value = 0;
        hitAmount = 0f;
        if (damage >= targetScore)
        {
            hitAmount = 0.099f;
            Debug.Log("Success");             
        }
        else if (damage < (targetScore/2))
        {
            hitAmount = 0.06f;
            Debug.Log("Big Failure");
        }
        else
        {
            hitAmount = 0.08f;
            Debug.Log("Failure");
        }
        // hitAmount = ((float)damage / (float)targetScore) / 7.5f;
        // Debug.Log(hitAmount);
        this.damage = damage;
        hit = true;
    }
}
