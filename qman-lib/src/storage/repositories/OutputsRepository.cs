using qmanlib.src.storage.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qmanlib.src.storage.repositories
{
    public class OutputsRepository : IEnumerable<Output>
    {
        private readonly IDictionary<string, byte> OutputCount = new Dictionary<string, byte> { { "40", 4 }, { "10", 4 }, { "05", 4 } }; //SWN04,DIMM04/500U,REL04SA 

        private IList<Output> Outputs { get; init; }
        public Output? this[short ID] => Outputs.FirstOrDefault((output)=> output.id == ID);
        public OutputsRepository(IEnumerable<Output> Outputs)
        {
            this.Outputs = Outputs.ToArray();
        }

        public IEnumerator<Output> GetEnumerator() => Outputs.GetEnumerator();
      

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public byte GetOutputCount(Module module)
        {
            string series = module.SerialNumber.Substring(0, 2);
            if (OutputCount.TryGetValue(series, out byte outputCount))
            {
                return outputCount;
            }
            return 0;
        }
        public IEnumerable<Output?> GetModuleOutputs(Module module)
        {
            IList<Output?> outputs = new List<Output?>();
            for (short i = 0; i < GetOutputCount(module); i++)
            {
               outputs.Add(this[module.Outputs[i]]);
            }
            return outputs;
        }
    }
}
