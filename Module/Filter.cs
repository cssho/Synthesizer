using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Dsp;
using NAudio.Wave;
using Synthesizer.Model;

namespace Synthesizer.Module
{
    public class Filter : ISampleProvider
    {
        private readonly ISampleProvider source;

        public BiQuadFilter Setting { get; set; }

        public Filter(ISampleProvider source)
        {
            this.source = source;
        }
        public int Read(float[] buffer, int offset, int count)
        {
            var samplesRead = source.Read(buffer, offset, count);
            if (Setting == null) return samplesRead;
            for (var n = 0; n < count; n++)
            {
                buffer[offset + n] = Setting.Transform(buffer[offset + n]);
            }
            return samplesRead;
        }

        public WaveFormat WaveFormat => source.WaveFormat;

    }
}
