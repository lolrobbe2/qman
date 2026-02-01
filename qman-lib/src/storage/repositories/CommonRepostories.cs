using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.repositories
{
    /// <summary>
    /// This class contains all the storage repositories for the common model types.
    /// </summary>
    public class CommonRepostories
    {
        #region repositories
        public ModulesRepository Modules { get; init; }
        public PlacesRepository Places { get; init; }
        #endregion
        public CommonRepostories(QbusDatabase dataBase)
        {
            Modules = new ModulesRepository(dataBase.Modules);
            Places = new PlacesRepository(dataBase.Places);
        }
    }
}
