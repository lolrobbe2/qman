// This file is part of MdbReader. Licensed under the LGPL version 2.0.
// You should have received a coy of the GNU LGPL version along with this
// program. If not, see https://www.gnu.org/licenses/old-licenses/lgpl-2.0.html
//
// Copyright Micah Makaiwi.
// Based on code from libmdb (https://github.com/mdbtools/mdbtools)

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using mdbreader.src.MdbReader.attributes;

using MMKiwi.MdbReader.Values;

namespace MMKiwi.MdbReader;

/// <summary>
/// A row in an Access Database. This class closely follows the API used in
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.data.common.dbdatareader">DbDataReader</see>
/// </summary>
public sealed partial class MdbDataRow
{
    internal MdbDataRow(ReadOnlySpan<IMdbValue> baseCollection, IEqualityComparer<string>? comparer, int dictionaryCreationThreshold)
    {
        Fields = new(baseCollection, comparer, dictionaryCreationThreshold);
    }
    private static readonly Regex NameNumberRegex = new Regex(@"^(?<name>[A-Za-z_]+)(?<number>\d+)$", RegexOptions.Compiled);

    FieldCollection Fields { get; }
    internal IEnumerable<IMdbValue> FieldValues => Fields.Values;

    /// <summary>
    /// The number of columns in the current row. 
    /// </summary>
    public int FieldCount => Fields.Count;

    /// <summary>
    /// Gets the value of the specified column in its native format given the column ordinal.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>
    public object? this[int index] => Fields.ElementAt(index).Value;

    /// <summary>
    /// Gets the value of the specified column in its native format given the column name.
    /// </summary>
    /// <param name="columnName">The column name</param>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <throws cref="IndexOutOfRangeException"> No column with the specified name was found.</throws>
    public object? this[string columnName] => Fields[columnName].Value;

    /// <summary>
    /// Gets a value that indicates whether the column contains non-existent or missing values.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <returns><c>true</c> if the specified column value is null; otherwise <c>false</c>.</returns>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>

    public bool IsNull(int index) => GetFieldValue(index).IsNull;

    /// <summary>
    /// Gets a value that indicates whether the column contains non-existent or missing values.
    /// </summary>
    /// <param name="columnName">The column name</param>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <returns><c>true</c> if the specified column value is null; otherwise <c>false</c>.</returns>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <throws cref="IndexOutOfRangeException"> No column with the specified name was found.</throws>
    public bool IsNull(string columnName) => GetFieldValue(columnName).IsNull;

    /// <summary>
    /// Gets the name of the specified column.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <returns>The name of the specified column.</returns>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>
    public string GetName(int index) => GetFieldValue(index).Column.Name;

    /// <summary>
    /// Gets a string representing the data type of the specified column.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <returns>The string representing the data type of the specified column.</returns>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>
    /// <remarks>
    /// See <seealso href="https://learn.microsoft.com/en-us/sql/odbc/microsoft/microsoft-access-data-types?view=sql-server-ver16" />
    /// for a list of all SQL data types and their equivalent names in the Access GUI.
    /// </remarks>
    public string GetColumnTypeName(int index) => GetFieldValue(index).Column.SqlTypeName;

    /// <summary>
    /// Gets a string representing the data type of the specified column.
    /// </summary>
    /// <param name="columnName">The column name</param>
    /// <returns>The string representing the data type of the specified column.</returns>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <throws cref="IndexOutOfRangeException"> No column with the specified name was found.</throws>
    /// <remarks>
    /// See <seealso href="https://learn.microsoft.com/en-us/sql/odbc/microsoft/microsoft-access-data-types?view=sql-server-ver16" />
    /// for a list of all SQL data types and their equivalent names in the Access GUI.
    /// </remarks>
    public string GetColumnTypeName(string columnName) => GetFieldValue(columnName).Column.SqlTypeName;

    /// <summary>
    /// Gets the data type of the specified column.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <returns>The <see cref="MdbColumnType" /> of the specified column.</returns>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>
    public MdbColumnType GetColumnType(int index)
    {
        return GetFieldValue(index).Column.Type;
    }

    /// <summary>
    /// Gets the data type of the specified column.
    /// </summary>
    /// <param name="columnName">The column name</param>
    /// <returns>The <see cref="MdbColumnType" /> of the specified column.</returns>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <throws cref="IndexOutOfRangeException"> No column with the specified name was found.</throws>
    public MdbColumnType GetColumnType(string columnName) => GetFieldValue(columnName).Column.Type;

    /// <summary>
    /// Gets the column information of the specified column.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <returns>The <see cref="MdbColumn" /> of the specified column.</returns>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>
    public MdbColumn GetColumnInfo(int index)
    {
        return GetFieldValue(index).Column;
    }

    /// <summary>
    /// Gets the column information of the specified column.
    /// </summary>
    /// <param name="columnName">The column name</param>
    /// <returns>The <see cref="MdbColumn" /> of the specified column.</returns>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <throws cref="IndexOutOfRangeException"> No column with the specified name was found.</throws>
    public MdbColumn GetColumnInfo(string columnName) => GetFieldValue(columnName).Column;

    /// <summary>
    /// Returns a list of all the columns in the specified row.
    /// </summary>    
    /// <returns>The <see cref="MdbColumn" /> objects for all fields in the row.</returns>
    public IEnumerable<MdbColumn> Columns => Fields.Select(f => f.Value.Column);

#warning TODO: Document what types are associated with each column
    /// <summary>
    /// Gets the value of the specified column in its native format.
    /// </summary>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <returns>The underlying data object, or null if the field is null.</returns>
    /// <throws cref="IndexOutOfRangeException">The index passed was outside of the range of 0 through <see cref="FieldCount" />.</throws>
    public object? GetValue(int index)
    {
        var fieldValue = GetFieldValue(index);
        if (fieldValue.IsNull) return null;
        return fieldValue.Value;
    }

    /// <summary>
    /// Gets the value of the specified column in its native format.
    /// </summary>
    /// <param name="columnName">The column name</param>
    /// <returns>The underlying data object, or null if the field is null.</returns>
    /// <throws cref="ArgumentNullException"><c>columnName</c> is <c>null</c>.</throws>
    /// <throws cref="IndexOutOfRangeException"> No column with the specified name was found.</throws>
    public object? GetValue(string columnName)
    {
        var fieldValue = GetFieldValue(columnName);
        if (fieldValue.IsNull) return null;
        return fieldValue.Value;
    }
    public static bool TryExtractNameAndNumber(string input, out string namePart, out int numberPart)
    {
        namePart = null;
        numberPart = 0;

        var match = NameNumberRegex.Match(input);
        if (!match.Success)
            return false;

        namePart = match.Groups["name"].Value;
        numberPart = int.Parse(match.Groups["number"].Value);
        return true;
    }
    public T As<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>() where T : class, new()
    {
        var type = typeof(T);
        MdbRowAttribute? attr = (MdbRowAttribute?)Attribute.GetCustomAttribute(type, typeof(MdbRowAttribute));
        if (attr == null)
        {
            throw new InvalidOperationException("T must have the MdbRowAttribute");
        }
        T instance = new T();

        foreach (MdbColumn col in Columns)
        {
            /*
            object? valueToSet = col.Type switch
            {
                MdbColumnType.Boolean => GetBoolean(col.Index),
                MdbColumnType.Byte => GetByte(col.Index),
                MdbColumnType.Int => GetString(col.Index),
                MdbColumnType.LongInt => GetInt32(col.Index),
                MdbColumnType.Currency => GetNullableDecimal(col.Index),
                MdbColumnType.Single => GetSingle(col.Index),
                MdbColumnType.Double => GetDouble(col.Index),
                MdbColumnType.DateTime => GetDateTime(col.Index),
                MdbColumnType.Binary => GetString(col.Index),
                MdbColumnType.Text => GetString(col.Index),
                MdbColumnType.Memo => GetString(col.Index),
                MdbColumnType.Numeric => GetDecimal(col.Index),
                MdbColumnType.Complex => GetInt32(col.Index),
                _ => null
            };
            */
            object? valueToSet = GetValue(col.Name);
            if (TryExtractNameAndNumber(col.Name, out string name, out int number))
            {
                PropertyInfo? prop = type?.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

                var arrayInstance = prop.GetValue(instance) as Array;

                if (arrayInstance != null && number < arrayInstance.Length)
                {


                    // 2. Set the value at the specific index
                    if (valueToSet != null)
                    {
                        arrayInstance.SetValue(valueToSet, number);
                    }
                }
            }
            else
            {
                PropertyInfo? prop = type?.GetProperty(col.Name, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null && type != null)
                {
                    prop = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                               .FirstOrDefault(p => p.GetCustomAttribute<MdbParamAttribute>()?.Name == col.Name);
                }
                prop!.SetValue(instance, valueToSet);
            }
        }
        return instance;
    }
}
