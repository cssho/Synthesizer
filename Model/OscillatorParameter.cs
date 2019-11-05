using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Synthesizer.Annotations;

namespace Synthesizer.Model
{
    public class OscillatorParameter
    {
        public WaveForm WaveForm { get; set; }

        public int AdjustFrequency { get; set; }

        public bool IsActive { get; set; }

    }

    public class SubOscillatorParameter : OscillatorParameter
    {
        public double Rate { get; set; }
    }
}
