using System.Collections.Generic;
using System.Windows.Input;

namespace Synthesizer.Model
{
    public class KeyInput
    {
        private static readonly Dictionary<Key, double> keyFrequencyDictionary
            = new Dictionary<Key, double>
            {
                {Key.A,261.63 },
                {Key.S,293.66 },
                {Key.D,329.63 },
                {Key.F,349.23 },
                {Key.G,392.00 },
                {Key.H,440.00 },
                {Key.J,493.88 }

            };
        public Key TargetKey { get; set; }
        public bool IsDown { get; set; }

        public double Frequency => keyFrequencyDictionary[TargetKey];
        public static bool IsTargetKey(KeyEventArgs args) => keyFrequencyDictionary.ContainsKey(args.Key);

        public override string ToString() => $"Key:{TargetKey} IsDown:{IsDown}";
    }
}
