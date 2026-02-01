using qmanlib.src.storage.models;
using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qmanlib.src.storage.repositories
{
    public class PlacesRepository
    {
        public IList<Place> Places { get; init; }
        public PlacesRepository(IEnumerable<QdbPlace> places)
        {
            Places = new List<Place>(places);
            Places.Add(new Place() { ID = 0, Name = "Home", ParentID = -1 });
        }
        public Place GetRoot()
        {
            return Places.First((place) => place.ID == 0);
        }
        public IEnumerable<Place> GetChildren(Place place)
        {
            return Places.Where((childPlace) => childPlace.ParentID == place.ID);
        }
    }
}
