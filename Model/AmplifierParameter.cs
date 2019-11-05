using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.Model
{
    public class AmplifierParameter
    {
        public TimeSpan AttackTime { get; set; }
        public TimeSpan DecayTime { get; set; }
        public double SustainLevel { get; set; }
        public TimeSpan ReleaseTime { get; set; }

        public float Gain { get; set; }
    }
}
