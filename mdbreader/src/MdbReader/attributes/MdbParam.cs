// This file is part of MdbReader. Licensed under the LGPL version 2.0.
// You should have received a coy of the GNU LGPL version along with this
// program. If not, see https://www.gnu.org/licenses/old-licenses/lgpl-2.0.html
//
// Copyright Micah Makaiwi.
// Based on code from libmdb (https://github.com/mdbtools/mdbtools)

using System;
using System.Collections.Generic;
using System.Text;

namespace mdbreader.src.MdbReader.attributes;

using System;
using System.Reflection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MdbParamAttribute : Attribute
{
    public string Name { get; }

    public MdbParamAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Resolves the effective name: either the specified Name or the member type name.
    /// </summary>
    public string GetEffectiveName(MemberInfo member)
    {
        if (!string.IsNullOrEmpty(Name))
            return Name;

        switch (member)
        {
            case PropertyInfo pi:
                return pi.PropertyType.Name;
            case FieldInfo fi:
                return fi.FieldType.Name;
            default:
                return "Unknown";
        }
    }

}
