using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Dsp;
using NAudio.Wave;
using Reactive.Bindings;
using Synthesizer.Model;

namespace Synthesizer.Module
{
    public class MainPipeline : IObserver<KeyInput>
    {
        private readonly MainViewModel param;
        private Oscillator vco;
        private Oscillator voc2;
        private Filter filter;
        private Amplifier amp;
        private IWavePlayer player;
        private static readonly int SampleRate = 44100;

        public MainPipeline(MainViewModel param)
        {
            this.param = param;
            Build();
        }

        private void Build()
        {
            vco = new Oscillator(SampleRate);
            filter = new Filter(vco);
            amp = new Amplifier(filter, SampleRate);
            BuildVco(param.Vco1, param.Vco2);
            BuildVcf(param.Vcf);
            BuildVca(param.Vca);

        }

        private void BuildVca(AmplifierParameter paramVca)
        {
            amp.Gain = paramVca.Gain;
            amp.Eg = new EnvelopeGenerator(paramVca);
        }

        private void BuildVcf(FilterParameter vofParam)
        {
            switch (vofParam.Type)
            {
                case FilterType.LowPass:
                    filter.Setting = BiQuadFilter.LowPassFilter(SampleRate, vofParam.Frequency, vofParam.Q);
                    break;
                case FilterType.BandPass:
                    filter.Setting = BiQuadFilter.BandPassFilterConstantSkirtGain(SampleRate, vofParam.Frequency, vofParam.Q);
                    break;
                case FilterType.HighPass:
                    filter.Setting = BiQuadFilter.HighPassFilter(SampleRate, vofParam.Frequency, vofParam.Q);
                    break;
                case FilterType.Through:
                default:
                    filter.Setting = null;
                    break;
            }
        }

        private void BuildVco(OscillatorParameter vocParam, SubOscillatorParameter subVcoParam)
        {
            vco.UseSub = subVcoParam.IsActive;
            vco.Rate = subVcoParam.Rate;

            switch (vocParam.WaveForm)
            {
                case WaveForm.Sine:
                    vco.GeneratorFunction = x => (float)Math.Sin(2f * Math.PI * x); break;
                case WaveForm.Square:
                    vco.GeneratorFunction = x => Math.Sign(Math.Sin(2f * Math.PI * x)); break;
                case WaveForm.Triangle:
                    vco.GeneratorFunction = x => 1f - 4f * (float)Math.Abs(Math.Round(x - 0.25f) - (x - 0.25f)); break;
                case WaveForm.Sawtooth:
                    vco.GeneratorFunction = x => 2f * ((float)x - (float)Math.Floor(x + 0.5f)); break;
                case WaveForm.Random:
                default:
                    vco.GeneratorFunction = x => 0;
                    break;
            }

            switch (subVcoParam.WaveForm)
            {
                case WaveForm.Sine:
                    vco.SubGeneratorFunction = x => (float)Math.Sin(2f * Math.PI * x); break;
                case WaveForm.Square:
                    vco.SubGeneratorFunction = x => Math.Sign(Math.Sin(2f * Math.PI * x)); break;
                case WaveForm.Triangle:
                    vco.SubGeneratorFunction = x => 1f - 4f * (float)Math.Abs(Math.Round(x - 0.25f) - (x - 0.25f)); break;
                case WaveForm.Sawtooth:
                    vco.SubGeneratorFunction = x => 2f * ((float)x - (float)Math.Floor(x + 0.5f)); break;
                case WaveForm.Random:
                default:
                    vco.SubGeneratorFunction = x => 0;
                    break;
            }
        }

        private KeyInput firstInput;

        public void OnNext(KeyInput value)
        {
            //Debug.WriteLine(value);
            if (player == null)
            {
                player = new WaveOutEvent();
                player.PlaybackStopped += Player_PlaybackStopped;
                firstInput = value;
                Build();
                vco.Frequency = value.Frequency;


                player.Init(amp);
                player.Play();
            }
            else if (
                player.PlaybackState == PlaybackState.Playing
                && firstInput.TargetKey == value.TargetKey
                && !value.IsDown)
            {
                amp.Eg.IsKeyUp = true;
                Thread.Sleep(param.Vca.ReleaseTime);
                player.Stop();
                firstInput = null;
            }

        }

        private void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            player?.Dispose();
            player = null;
        }

        public void OnError(Exception e)
        {
            Debug.WriteLine(e);
            player.Stop();
        }

        public void OnCompleted()
        {
            player.Stop();
        }
    }
}
