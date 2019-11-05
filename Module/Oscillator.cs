using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Synthesizer.Module
{
    public class Oscillator : ISampleProvider
    {
        private readonly int sampleRate;
        private long lastCount;

        public Oscillator(int sampleRate)
        {
            this.sampleRate = sampleRate;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (var n = 0; n < count; n++)
            {
                if (UseSub)
                    buffer[offset + n] =
                        (float)(GeneratorFunction((lastCount + n) / (double)sampleRate * Frequency) * (1 - Rate)
                                 + SubGeneratorFunction((lastCount + n) / (double)sampleRate * Frequency) * Rate);
                else
                    buffer[offset + n] = GeneratorFunction((lastCount + n) / (double)sampleRate * Frequency);
            }

            lastCount += count;
            return count;


        }

        public WaveFormat WaveFormat { get; }
        public double Frequency { get; set; }
        public Func<double, float> GeneratorFunction;
        public Func<double, float> SubGeneratorFunction;
        public double Rate { get; set; }
        public bool UseSub { get; set; }

    }
}
