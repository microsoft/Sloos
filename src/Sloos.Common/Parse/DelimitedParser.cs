// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Sloos.Common
{
    public sealed class DelimitedParser : IDataReader
    {
        private readonly char delimiter;
        private readonly DelimitedHeader header;
        private readonly CharReader reader;

        private readonly string[] row;

        private DelimitedParser(
            DelimitedParserSettings settings,
            CharReader reader)
        {
            this.header = settings.DelimitedHeader;
            this.reader = reader;
            this.delimiter = settings.Delimiter;

            this.row = new string[header.Count];
        }

        public bool IsClosed { get; set; }

        public int Depth => 1;

        public int RecordsAffected => -1;

        public int FieldCount => this.row.Length;

        public object this[string name] => this.row[this.GetOrdinal(name)];

        public object this[int i] => this.row[i];

        public void Close()
        {
            if (!this.IsClosed)
            {
                this.reader.Close();
                this.IsClosed = true;
            }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            if (!this.reader.EndOfStream)
            {
                Array.Clear(this.row, 0, this.row.Length);
                Array.Copy(new DelimitedRecordParser(this.reader, this.delimiter).Parse().ToArray(), this.row, this.row.Length);

                return true;
            }

            return false;
        }

        public bool Read()
        {
            return this.NextResult();
        }

        public void Dispose()
        {
            this.Close();
        }

        public bool GetBoolean(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToBoolean(this.row[i]);
        }

        public byte GetByte(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToByte(this.row[i]);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToChar(this.row[i]);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            return this.header.DelimitedColumns[i].Type.Name;
        }

        public DateTime GetDateTime(int i)
        {
            this.ValidateIndexRange(i);
            var dateTime = DateTime.Parse(
                this.row[i],
                CultureInfo.CurrentCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal);

            return dateTime.ToUniversalTime();
        }

        public decimal GetDecimal(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToDecimal(this.row[i]);
        }

        public double GetDouble(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToDouble(this.row[i]);
        }

        public Type GetFieldType(int i)
        {
            return this.header.DelimitedColumns[i].Type;
        }

        public float GetFloat(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToSingle(this.row[i]);
        }

        public Guid GetGuid(int i)
        {
            this.ValidateIndexRange(i);
            return Guid.Parse(this.row[i]);
        }

        public short GetInt16(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToInt16(this.row[i]);
        }

        public int GetInt32(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToInt32(this.row[i]);
        }

        public long GetInt64(int i)
        {
            this.ValidateIndexRange(i);
            return Convert.ToInt64(this.row[i]);
        }

        public string GetName(int i)
        {
            return this.header.DelimitedColumns[i].Name;
        }

        public int GetOrdinal(string name)
        {
            return this.header.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            this.ValidateIndexRange(i);
            return this.row[i];
        }

        public object GetValue(int i)
        {
            //return i == 0 ? null : this.row[i - 1];
            this.ValidateIndexRange(i);
            return this.row[i];
        }

        public int GetValues(object[] values)
        {
            Array.Copy(this.row, values, this.row.Length);
            return this.row.Length;
        }

        public bool IsDBNull(int i)
        {
            var type = this.header.DelimitedColumns[i].Type;
            if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
            {
                return false;
            }

            return string.IsNullOrEmpty(this.row[i]);
        }

        public IEnumerable<IEnumerable<string>> Parse()
        {
            while (!this.reader.EndOfStream)
            {
                var parser = new DelimitedRecordParser(this.reader, this.delimiter);
                yield return parser.Parse();
            }
        }

        private void ValidateIndexRange(int index)
        {
            if (index < 0 || index >= this.row.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public static DelimitedParser Create(
            DelimitedParserSettings settings,
            Stream stream)
        {
            var reader = new CharReader(stream);
            return new DelimitedParser(settings, reader);
        }
    }
}
