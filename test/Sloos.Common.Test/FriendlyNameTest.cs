using Xunit;

using Sloos;
using System;

namespace Sloos.Common.Test
{
    public class FriendlyNameTest
    {
        [Fact]
        public void FriendlyNames()
        {
            Assert.Equal("byte", typeof(byte).FriendlyName());
            Assert.Equal("bool", typeof(bool).FriendlyName());
            Assert.Equal("sbyte", typeof(sbyte).FriendlyName());
            Assert.Equal("char", typeof(char).FriendlyName());
            Assert.Equal("short", typeof(short).FriendlyName());
            Assert.Equal("ushort", typeof(ushort).FriendlyName());
            Assert.Equal("int", typeof(int).FriendlyName());
            Assert.Equal("uint", typeof(uint).FriendlyName());
            Assert.Equal("long", typeof(long).FriendlyName());
            Assert.Equal("ulong", typeof(ulong).FriendlyName());
            Assert.Equal("float", typeof(float).FriendlyName());
            Assert.Equal("double", typeof(double).FriendlyName());
            Assert.Equal("decimal", typeof(decimal).FriendlyName());
            Assert.Equal("Guid", typeof(Guid).FriendlyName());
            Assert.Equal("TimeSpan", typeof(TimeSpan).FriendlyName());
            Assert.Equal("DateTime", typeof(DateTime).FriendlyName());
            Assert.Equal("DateTimeOffset", typeof(DateTimeOffset).FriendlyName());

            Assert.Equal("byte?", typeof(byte?).FriendlyName());
            Assert.Equal("bool?", typeof(bool?).FriendlyName());
            Assert.Equal("sbyte?", typeof(sbyte?).FriendlyName());
            Assert.Equal("char?", typeof(char?).FriendlyName());
            Assert.Equal("short?", typeof(short?).FriendlyName());
            Assert.Equal("ushort?", typeof(ushort?).FriendlyName());
            Assert.Equal("int?", typeof(int?).FriendlyName());
            Assert.Equal("uint?", typeof(uint?).FriendlyName());
            Assert.Equal("long?", typeof(long?).FriendlyName());
            Assert.Equal("ulong?", typeof(ulong?).FriendlyName());
            Assert.Equal("float?", typeof(float?).FriendlyName());
            Assert.Equal("double?", typeof(double?).FriendlyName());
            Assert.Equal("decimal?", typeof(decimal?).FriendlyName());
            Assert.Equal("Guid?", typeof(Guid?).FriendlyName());
            Assert.Equal("TimeSpan?", typeof(TimeSpan?).FriendlyName());
            Assert.Equal("DateTime?", typeof(DateTime?).FriendlyName());
            Assert.Equal("DateTimeOffset?", typeof(DateTimeOffset?).FriendlyName());

            Assert.Equal(nameof(FriendlyNameTest), typeof(FriendlyNameTest).FriendlyName());
        }
    }
}
