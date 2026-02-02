using mdbreader.src.MdbReader.attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.models
{
    
    public class Output
    {
        [MdbParam("id")]
        public Int16 id { get; set; }
        [MdbParam("naam")]
        public string Name { get; set; }
        public string TXName { get; set; }
        [MdbParam("modeid")]
        public Int16 ModeId { get; set; }
        [MdbParam("adres")]
        public byte Adres { get; set; }
        [MdbParam("subadres")]
        public byte SubAdres { get; set; }
        [MdbParam("simul")]
        public byte Simulate { get; set; }
        [MdbParam("warning")]
        public byte Warning { get; set; }
        [MdbParam("par")]
        public byte[] Parameters { get; set; } = new byte[36];
        [MdbParam("Gewijzigd")]
        public string Changed { get; set; }
        public Int16 SoftLinkID { get; set; }
        public bool Fictief { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double Multiplier { get; set; }
        public string Unit { get; set; }
        public Int16 LinkID { get; set; }
        [MdbParam("Output")]
        public Int16[] Outputs { get; set; } = new Int16[7];
        [MdbParam("PlaatsID")]
        public Int16 LocationID { get; set; }
        public bool HideExtern { get; set; }
    }
}
