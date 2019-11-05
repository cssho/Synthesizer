using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Reactive.Bindings;
using Synthesizer.Annotations;
using Synthesizer.Module;

namespace Synthesizer.Model
{
    public class MainViewModel
    {
        public OscillatorParameter Vco1 { get; set; }
            = new OscillatorParameter { WaveForm = WaveForm.Sine, IsActive = true };

        public FilterParameter Vcf { get; set; }
            = new FilterParameter { Frequency = 500f, Q = 1f, Type = FilterType.Through };

        public AmplifierParameter Vca { get; set; }
            = new AmplifierParameter
            {
                SustainLevel = 0.3,
                AttackTime = TimeSpan.FromMilliseconds(200),
                DecayTime = TimeSpan.FromMilliseconds(300),
                ReleaseTime = TimeSpan.FromMilliseconds(500),
                Gain = 1.0f
            };

        public SubOscillatorParameter Vco2 { get; set; }
            = new SubOscillatorParameter { WaveForm = WaveForm.Triangle, IsActive = false, Rate = 0.5 };


        public MainPipeline Pipeline { get; set; }

        public MainViewModel()
        {
            Pipeline = new MainPipeline(this);
        }
    }
}
