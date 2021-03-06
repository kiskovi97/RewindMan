﻿using UnityEngine;
using UnityEngine.UI;
namespace FixPhysics
{
    public class BackwardsEffect : MonoBehaviour
    {
        public Light lightSource;
        public GlitchEffect effect;
        public Color reverseColor;
        public float glitchIntensity = 0.2f;
        private Color prevColor;
        public Text help;

        public void Start()
        {
            if (lightSource != null)
                prevColor = lightSource.color;
        }

        private void FixedUpdate()
        {
            if (FixWorld.GameOver)
            {
                if (help != null)
                    help.text = "Use Q To Reverse Time";
                SetBackWardEffect();
            }

            if (FixWorld.Forward && !FixWorld.GameOver)
            {
                if (help != null)
                    help.text = "";
                SetForwardEffect();
            }
            else if (FixWorld.Backward)
            {
                if (help != null)
                    help.text = "";
                SetBackWardEffect();
            }
        }

        private void SetForwardEffect()
        {
            if (lightSource != null)
            {
                lightSource.color = prevColor;
            }
            if (effect != null)
            {
                effect.intensity = 0;
                effect.colorIntensity = 0;
            }
        }

        private void SetBackWardEffect()
        {
            if (lightSource != null)
            {
                lightSource.color = reverseColor;
            }
            if (effect != null)
            {
                effect.intensity = glitchIntensity;
                effect.colorIntensity = 1f;
            }
        }
    }
}
