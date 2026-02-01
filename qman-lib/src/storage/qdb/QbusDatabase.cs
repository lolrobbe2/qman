using MMKiwi.MdbReader;
using System;
using System.Collections.Generic;
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
        public QbusDatabase(MdbConnection handle)
        {
            Handle = handle;
            Controllers = GetControllers();
            Modules = GetModules();
            Places = GetPlaces();
        }
        private IList<QdbController> GetControllers()
        {
            MdbTable table = Tables["Controler"];
            MdbRows rows = table.Rows;
            IList<QdbController> controllers = new List<QdbController>();
            foreach (var item in rows)
            {
                controllers.Add(new QdbController(item));
            }
            return Controllers;
        }
        private IList<QdbModule> GetModules()
        {
            MdbTable table = Tables["Modules"];
            MdbRows rows = table.Rows;
            IList<QdbModule> modules = new List<QdbModule>();
            foreach (var item in rows)
            {
                modules.Add(new QdbModule(item));
            }
            return modules;
        }
        private IList<QdbPlace> GetPlaces()
        {
            MdbTable table = Tables["Plaatsen"];
            MdbRows rows = table.Rows;
            IList<QdbPlace> places = new List<QdbPlace>();
            foreach (var item in rows)
            {
                places.Add(new QdbPlace(item));
            }
            return places;
        }
    }
}
