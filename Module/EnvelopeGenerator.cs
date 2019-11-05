using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synthesizer.Model;

namespace Synthesizer.Module
{
    public class EnvelopeGenerator
    {
        private readonly AmplifierParameter param;

        public EnvelopeGenerator(AmplifierParameter param)
        {
            this.param = param;
        }

        public bool IsKeyUp { get; set; }

        private double lastKeyDownTime;
        private float lastLevel;
        private bool inAttack, inDecay, inSustain, inRelease;

        public float CalculateLevel(double time)
        {
            if (!IsKeyUp) lastKeyDownTime = time;
            if (param.AttackTime.TotalSeconds >= time && !IsKeyUp)
            {
                if (!inAttack) { inAttack = true; Debug.WriteLine($"{nameof(inAttack)}:{time}"); }
                return lastLevel = (float) (1 / param.AttackTime.TotalSeconds * time);
            }

            if (param.DecayTime.TotalSeconds + param.AttackTime.TotalSeconds >= time && !IsKeyUp)
            {
                if (!inDecay) { inDecay = true; Debug.WriteLine($"{nameof(inDecay)}:{time}"); }
                return lastLevel = (float) ((param.SustainLevel - 1) * (time - param.AttackTime.TotalSeconds)
                                            / param.DecayTime.TotalSeconds + 1);
            }

            if (param.DecayTime.TotalSeconds + param.AttackTime.TotalSeconds < time && !IsKeyUp)
            {

                if (!inSustain) { inSustain = true; Debug.WriteLine($"{nameof(inSustain)}:{time}:{lastLevel}"); }
                return lastLevel = (float) param.SustainLevel;
            }

            if (!inRelease) { inRelease = true; Debug.WriteLine(nameof(inRelease)); }

            var releaseLevel = -lastLevel / param.ReleaseTime.TotalSeconds * (time - lastKeyDownTime)
                                 + lastLevel;
            return (float) (releaseLevel >= 0 ? releaseLevel : 0);
        }
    }
}
