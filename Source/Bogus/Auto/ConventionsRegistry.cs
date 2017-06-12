using System;
using System.Collections.Generic;

namespace Bogus.Auto
{
    internal static class ConventionsRegistry
    {
        internal static readonly IEnumerable<INameBindingGenerator> NameBindingGenerators = new List<INameBindingGenerator>
        {
          new NameBindingGenerator<string>(c => c.FakerHub.Name.FirstName(), "FirstName"),
          new NameBindingGenerator<string>(c => c.FakerHub.Name.LastName(), "LastName"),
          new NameBindingGenerator<string>(c => c.FakerHub.Internet.Email(),"Email")
        };

        internal static readonly IEnumerable<ITypeBindingGenerator> TypeBindingGenerators = new List<ITypeBindingGenerator>
        {
          new TypeBindingGenerator<bool>(c => c.FakerHub.Random.Bool()),
          new TypeBindingGenerator<byte>(c => c.FakerHub.Random.Byte()),
          new TypeBindingGenerator<char>(c => c.FakerHub.Random.Char()),
          new TypeBindingGenerator<DateTime>(c => c.FakerHub.Date.Recent()),
          new TypeBindingGenerator<decimal>(c => c.FakerHub.Random.Decimal()),
          new TypeBindingGenerator<double>(c => c.FakerHub.Random.Double()),
          new TypeBindingGenerator<float>(c => c.FakerHub.Random.Float()),
          new TypeBindingGenerator<Guid>(c => c.FakerHub.Random.Uuid()),
          new TypeBindingGenerator<int>(c => c.FakerHub.Random.Int()),
          new TypeBindingGenerator<long>(c => c.FakerHub.Random.Long()),
          new TypeBindingGenerator<sbyte>(c => c.FakerHub.Random.SByte()),
          new TypeBindingGenerator<short>(c => c.FakerHub.Random.Short()),
          new TypeBindingGenerator<string>(c => c.FakerHub.Lorem.Word()),
          new TypeBindingGenerator<uint>(c => c.FakerHub.Random.UInt()),
          new TypeBindingGenerator<ulong>(c => c.FakerHub.Random.ULong()),
          new TypeBindingGenerator<ushort>(c => c.FakerHub.Random.UShort())
        };
    }
}
