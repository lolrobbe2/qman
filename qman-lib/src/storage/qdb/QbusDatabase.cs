using MMKiwi.MdbReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qmanlib.src.storage.qdb
{
    public class QbusDatabase
    {
        public MdbConnection Handle { get; init; }
        private MdbTables Tables => Handle.Tables;
        public IList<QdbController> Controllers { get; init; }
        public IList<QdbModule> Modules { get; init; }
        public IList<QdbPlace> Places { get; init; }
        public IList<QdbOutput> Outputs { get; init; }

        public QbusDatabase(MdbConnection handle)
        {
            Handle = handle;
            Controllers = GetControllers();
            Modules = GetModules();
            Places = GetPlaces();
            Outputs = GetOutputs();
        }
        private IList<QdbController> GetControllers()
        {
            MdbTable table = Tables["Controler"];
            MdbRows rows = table.Rows;
            return rows.As<QdbController>().ToList();

        }
        private IList<QdbModule> GetModules()
        {
            MdbTable table = Tables["Modules"];
            MdbRows rows = table.Rows;
            return rows.As<QdbModule>().ToList();

        }
        private IList<QdbPlace> GetPlaces()
        {
            MdbTable table = Tables["Plaatsen"];
            MdbRows rows = table.Rows;
            return rows.As<QdbPlace>().ToList();
        }

        private IList<QdbOutput> GetOutputs()
        {
            MdbTable table = Tables["Outputs"];
            MdbRows rows = table.Rows;
            return rows.As<QdbOutput>().ToList();
        }
    }
}
