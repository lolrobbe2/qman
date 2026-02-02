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
        private IList<Output> Outputs { get; init; }

        public OutputsRepository(IEnumerable<Output> Outputs)
        {
            this.Outputs = Outputs.ToArray();
        }

        public IEnumerator<Output> GetEnumerator() => Outputs.GetEnumerator();
      

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Output> GetModuleOutputs(Module module)
        {
            return Outputs.Where((output) => module.Outputs.Contains(output.id));
        }
    }
}
