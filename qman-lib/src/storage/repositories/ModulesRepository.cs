using qmanlib.src.storage.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qmanlib.src.storage.repositories
{
    public class ModulesRepository : IEnumerable<Module>
    {
        private IList<Module> Modules { get; init; }

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

        public void SetModuleParent(Int16 place, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            int index = Modules.IndexOf(Modules.First((module) => module.Location == name));
            Modules[index].LocationId = place;
        }
    }
}
