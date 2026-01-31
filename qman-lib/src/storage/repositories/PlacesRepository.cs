using qmanlib.src.storage.models;
using qmanlib.src.storage.qdb;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.repositories
{
    public class PlacesRepository
    {
        public IList<Place> Places { get; init; }
        public PlacesRepository(IEnumerable<QdbPlace> places)
        {
            Places = new List<Place>(places);
        }
    }
}
