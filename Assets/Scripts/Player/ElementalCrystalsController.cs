using System;
using UnityEngine;

namespace PlayerSpace
{
    public class ElementalCrystalsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject weapon;
        [SerializeField]
        private Renderer earthCrystal;
        [SerializeField]
        private Renderer airCrystal;
        [SerializeField]
        private Renderer fireCrystal;
        [SerializeField]
        private Renderer waterCrystal;
        [SerializeField]
        private Renderer lifeCrystal;

        Color earthColor = new Color(0, 191, 0, 1);
        Color airColor = new Color(1, 1, 191, 1);
        Color fireColor = new Color(191, 1, 0, 1);
        Color waterColor = new Color(0, 1, 191, 1);
        Color lifeColor = new Color(191, 1, 191, 1);

        float earthMana = 10;
        float airMana = 10;
        float fireMana = 10;
        float waterMana = 10;
        float lifeMana = 10;

        float reducedMana = 0.5f;
        float augmentedMana = 0.005f;

        private void Start()
        {
            SetEmission(earthMana, earthCrystal, earthColor);
            SetEmission(airMana, airCrystal, airColor);
            SetEmission(fireMana, fireCrystal, fireColor);
            SetEmission(waterMana, waterCrystal, waterColor);
            SetEmission(lifeMana, lifeCrystal, lifeColor);
        }

        void FixedUpdate()
        {
            weapon.transform.Rotate(0, 0, 1);

            AugmentCrystalMana('E');
            AugmentCrystalMana('A');
            AugmentCrystalMana('F');
            AugmentCrystalMana('W');

        }

        public void ReduceCrystalMana(char crystalName)
        {
            switch (crystalName)
            {
                case 'E':
                    if (earthMana > -10)
                    {
                        earthMana -= reducedMana;
                        SetEmission(earthMana, earthCrystal, earthColor);
                    }
                    break;

                case 'A':
                    if (airMana > -10)
                    {
                        airMana -= reducedMana;
                        SetEmission(airMana, airCrystal, airColor);
                    }
                    break;

                case 'F':
                    if (fireMana > -10)
                    {
                        fireMana -= reducedMana;
                        SetEmission(fireMana, fireCrystal, fireColor);
                    }
                    break;

                case 'W':
                    if (waterMana > -10)
                    {
                        waterMana -= reducedMana;
                        SetEmission(waterMana, waterCrystal, waterColor);
                    }
                    break;
            }
        }


        public float GetDamageMultiplier(string crystalNames)
        {
            float totalMultiplier = 0;
            int count = 0;

            foreach (char crystal in crystalNames)
            {
                switch (crystal)
                {
                    case 'E':
                        totalMultiplier += (earthMana + 10) / 10;
                        break;

                    case 'A':
                        totalMultiplier += (airMana + 10) / 10;
                        break;

                    case 'F':
                        totalMultiplier += (fireMana + 10) / 10;
                        break;

                    case 'W':
                        totalMultiplier += (waterMana + 10) / 10;                        
                        break;

                        
                }

                count++;
            }

            return totalMultiplier / count;
        }
        

        public void AugmentCrystalMana(char crystalName)
        {
            switch (crystalName)
            {
                case 'E':
                    if (earthMana < 10)
                    {
                        earthMana += augmentedMana;
                        SetEmission(earthMana, earthCrystal, earthColor);
                    }
                    break;

                case 'A':
                    if (airMana < 10)
                    {
                        airMana += augmentedMana;
                        SetEmission(airMana, airCrystal, airColor);
                    }
                    break;

                case 'F':
                    if (fireMana < 10)
                    {
                        fireMana += augmentedMana;
                        SetEmission(fireMana, fireCrystal, fireColor);
                    }
                    break;

                case 'W':
                    if (waterMana < 10)
                    {
                        waterMana += augmentedMana;
                        SetEmission(waterMana, waterCrystal, waterColor);
                    }
                    break;
            }
        }

        public void SetEmission(float newIntensity, Renderer crystal, Color color)
        {
            // Calcular el nuevo color de emisión con la intensidad
            Color finalColor = color * newIntensity;

            // Aplicar el color de emisión al material del Renderer
            crystal.material.SetColor("_EmissionColor", finalColor);

            // Hacer que la propiedad de emisión se actualice en tiempo real
            DynamicGI.SetEmissive(crystal, finalColor);
        }

    }

}