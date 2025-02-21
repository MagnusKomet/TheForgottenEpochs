using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace
{
    public class ManaMeterController : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;
        public Image fill;

        [SerializeField]
        private float mana = 8;

        private void Start()
        {
            slider.maxValue = mana;
            slider.value = mana;

            fill.color = gradient.Evaluate(1f);

        }

        private void FixedUpdate()
        {
            IncreaseMana(0.01f);
        }

        public bool ReduceMana(float mana = 1)
        {
            float finalMana = this.mana - mana;

            if (finalMana < 0)
            {
                return false;
            }
            else
            {
                UpdateMeter(finalMana);
                return true;
            }
        }

        public bool IncreaseMana(float mana = 1)
        {
            float finalMana = this.mana + mana;

            if (finalMana > slider.maxValue)
            {
                return false;
            }
            else
            {
                UpdateMeter(finalMana);
                return true;
            }
        }

        public void FillMana()
        {
            UpdateMeter(slider.maxValue);
        }

        public void UpdateMeter(float mana)
        {
            this.mana = mana;
            slider.value = this.mana;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

    }
}

