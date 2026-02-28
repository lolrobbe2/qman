using qmanlib.src.storage.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace qmanlib.src.storage.repositories
{
    public class ModulesRepository : IEnumerable<Module>
    {
        private IList<Module> Modules { get; init; }

        public Module this[int id] => Modules.First(m => m.Id == id);   
        public ModulesRepository(IEnumerable<Module> modules)
        {
            Modules = new List<Module>(modules);
        }
        IEnumerator<Module> IEnumerable<Module>.GetEnumerator() => Modules.GetEnumerator();

        public IEnumerator GetEnumerator() => Modules.GetEnumerator();

        public IEnumerable<Module> GetModulesByType(string moduleType)
        {
            return Modules.Where((module) => string.Equals(module.Series, moduleType));
        }

        public IEnumerable<Module> GetModulesByPlace(Place place)
        {
            return Modules.Where((module) => module.LocationId == place.ID);
        }
        public Module? GetModuleById(Int16 id)
        {
            return Modules.Where((module) => module.Id == id).First();
        }

        public void SetModuleParent(Int16 place, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            int index = Modules.IndexOf(Modules.First((module) => module.Location == name));
            Modules[index].LocationId = place;
        }
        public void SetModuleName(int id, string name)
        {
            for (int i = 0; i < Modules.Count; i++)
                if (Modules[i].Id == id)
                    Modules[i].Location = name;
        }
    }
}
