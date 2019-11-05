using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesizer.Model
{
    public class FilterParameter
    {
        public FilterType Type { get; set; }

        public float Frequency { get; set; }
        public float Q { get; set; }
    }
}
