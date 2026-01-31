using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.models
{
    public class Place
    {
        public Int16 ID { get; set; }
        public string Name { get; set; }
        public Int16 ParentID { get; set; }
    }
}
