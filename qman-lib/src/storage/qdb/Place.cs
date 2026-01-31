using MMKiwi.MdbReader;
using qmanlib.src.storage.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace qmanlib.src.storage.qdb
{
    public enum QdbPlaceIndex
    {
        Id,
        Name,
        ParentID
    }
    public class QdbPlace : Place
    {

        public QdbPlace(MdbDataRow item)
        {
            ID = item.GetInt16((int)QdbPlaceIndex.Id);
            Name = item.GetString((int)QdbPlaceIndex.Name) ?? "NO NAME";
            ParentID = item.GetInt16((int)QdbPlaceIndex.ParentID);
        }
    }
}
