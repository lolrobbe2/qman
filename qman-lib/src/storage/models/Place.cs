using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace qmanlib.src.storage.models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Place
    {
        public Int16 ID { get; set; }
        public string Name { get; set; }
        public Int16 ParentID { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString()!;
        public override string? ToString()
        {
            return $"[Location({Name}:{ID}, parent: {ParentID})]";
        }
    }
}
