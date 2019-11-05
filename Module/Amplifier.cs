using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Synthesizer.Module
{
    public class Amplifier : ISampleProvider
    {
        public Amplifier(ISampleProvider source, int sampleRate)
        {
            this.source = source;
            this.sampleRate = sampleRate;
        }
        private readonly ISampleProvider source;
        private readonly int sampleRate;
        private long lastCount;

        public EnvelopeGenerator Eg { get; set; }

        public float Gain { get; set; }

        public int Read(float[] buffer, int offset, int count)
        {
            var samplesRead = source.Read(buffer, offset, count);
            if (Eg == null) return samplesRead;
            for (var n = 0; n < count; n++)
            {
                var calculateLevel = Eg.CalculateLevel((lastCount + n) / (double)sampleRate);
                buffer[offset + n] = Gain * buffer[offset + n]
                                          * calculateLevel;
            }
            lastCount += count;
            return samplesRead;
        }

        public WaveFormat WaveFormat => source.WaveFormat;
    }
}
