using mdbreader.src.MdbReader.attributes;
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
    [MdbRow]
    public class QdbPlace : Place
    {

    }
}
