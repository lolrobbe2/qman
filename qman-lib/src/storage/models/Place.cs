using mdbreader.src.MdbReader.attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace qmanlib.src.storage.models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Place
    {
        [MdbParam("id")]
        public Int16 ID { get; set; }
        [MdbParam("Naam")]
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
