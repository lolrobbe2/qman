// This file is part of MdbReader. Licensed under the LGPL version 2.0.
// You should have received a coy of the GNU LGPL version along with this
// program. If not, see https://www.gnu.org/licenses/old-licenses/lgpl-2.0.html
//
// Copyright Micah Makaiwi.
// Based on code from libmdb (https://github.com/mdbtools/mdbtools)

using System;
using System.Collections.Generic;
using System.Linq;

namespace MMKiwi.MdbReader.Schema;

internal class MdbRealIndex
{
    private MdbRealIndex(ReadOnlySpan<MdbRealIndexColumn> columns, int usedPages, int firstDataPointer, int flags, int numIndexRows)
    {
        Columns = columns.ToArray();
        UsedPages = usedPages;
        FirstDataPointer = firstDataPointer;
        Flags = flags;
        NumIndexRows = numIndexRows;
    }

    public IList<MdbRealIndexColumn> Columns { get; }
    public int UsedPages { get; }
    public int FirstDataPointer { get; }
    public int Flags { get; }
    public int NumIndexRows { get; }

    internal class Builder
    {
        public MdbRealIndexColumn.Builder[] Columns { get; } = new MdbRealIndexColumn.Builder[10];
        public int UsedPages { get; set; }
        public int FirstDataPointer { get; set; }
        public ushort Flags { get; set; }
        public int NumIndexRows { get; set; }

        public MdbRealIndex Build()
            => new MdbRealIndex(
                columns: Columns.Where(i => i.ColNum > 0).Select(i => i.Build()).ToArray().AsSpan(),
                usedPages: UsedPages,
                flags: Flags,
                firstDataPointer: FirstDataPointer,
                numIndexRows: NumIndexRows);
    }
}
