using qmanlib.src.storage.models;
using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#nullable enable
namespace src
{
    public class WorkspaceManager
    {
        private static QbusDatabase? database;

        public static void Open(string filePath) {
            database = QbusDatbaseReader.Open(filePath);
        }
        public static bool WorkspaceOpened(){
            return database is not null;
        }
        public static IList<Module> GetModules()
        {
            return database?.Modules!.Select(mod => mod.ToModule()).ToList() ?? [];

        }
        public static IList<Module> GetModules(Place place)
        {
            return database?.Modules!.Select(mod => mod.ToModule()).Where(module => place.ID == module.LocationId).ToList() ?? [];

        }
        public static Module UpdateModule(Module module)
        {
            Module existingModule = GetModules()
                .First(mod => mod.Id== module.Id);

            existingModule = module;

            return existingModule;
        }


        #region PLACES
        public static IList<Place> GetPlaces()
        {
            return database?.Places!.Select(place => place.ToPlace()).ToList() ?? [];
        }

        public static Place GetRootPlace(){
            return new Place() { ID = 0, Name = "Home", ParentID = -1 };
        }

        public static IList<Place> GetChildren(Place place) {
            return GetPlaces().Where(child => child.ParentID == place.ID).ToList();
        }
        public static Place GetParentPlace(Place place){
            return GetPlaces().Where(place => place.ID == place.ParentID).First();
        }
        public static Place UpdatePlace(Place place)
        {
            Place existingPlace = database!.Places!
                .First(p => p.ID == place.ID);

            existingPlace.Name = place.Name;
            existingPlace.ParentID = place.ParentID;

            return existingPlace;
        }
        public static void AddPlace(Place place){
            place.ID = (short)database!.Places!.Count();
            database!.Places!.Add(new QdbPlace() { ID = place.ID, Name = place.Name, ParentID = place.ParentID });
        }
        #endregion
    }
}
