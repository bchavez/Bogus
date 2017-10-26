using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bogus.Bson
{
   /// <summary>
   /// Most, if not all of this BSON implementation was copied from https://github.com/kernys/Kernys.Bson.
   /// Just polished it up a bit for Bogus in 2017/C# 7.1.
   /// </summary>
   public class BValue
   {
      private BValueType valueType;
      private Double _double;
      private string _string;
      private byte[] _binary;
      private bool _bool;
      private DateTime _dateTime;
      private Int32 _int32;
      private Int64 _int64;

      public BValueType ValueType => valueType;

      public Double DoubleValue
      {
         get
         {
            switch( this.valueType )
            {
               case BValueType.Int32:
                  return _int32;
               case BValueType.Int64:
                  return _int64;
               case BValueType.Double:
                  return _double;
               case BValueType.None:
                  return float.NaN;
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to double");
         }
      }

      public Int32 Int32Value
      {
         get
         {
            switch( this.valueType )
            {
               case BValueType.Int32:
                  return _int32;
               case BValueType.Int64:
                  return (Int32)_int64;
               case BValueType.Double:
                  return (Int32)_double;
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to Int32");
         }
      }

      public Int64 Int64Value
      {
         get
         {
            switch( this.valueType )
            {
               case BValueType.Int32:
                  return _int32;
               case BValueType.Int64:
                  return _int64;
               case BValueType.Double:
                  return (Int64)_double;
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to Int64");
         }
      }

      public byte[] BinaryValue
      {
         get
         {
            switch( valueType )
            {
               case BValueType.Binary:
                  return _binary;
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to binary");
         }
      }

      public DateTime DateTimeValue
      {
         get
         {
            switch( valueType )
            {
               case BValueType.UTCDateTime:
                  return _dateTime;
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to DateTime");
         }
      }

      public String StringValue
      {
         get
         {
            switch( valueType )
            {
               case BValueType.Int32:
                  return Convert.ToString(_int32);
               case BValueType.Int64:
                  return Convert.ToString(_int64);
               case BValueType.Double:
                  return Convert.ToString(_double);
               case BValueType.String:
                  return _string != null ? _string.TrimEnd((char)0) : null;
               case BValueType.Binary:
                  return Encoding.UTF8.GetString(_binary).TrimEnd((char)0);
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to string");
         }
      }

      public bool BoolValue
      {
         get
         {
            switch( valueType )
            {
               case BValueType.Boolean:
                  return _bool;
            }

            throw new Exception($"Original type is {this.valueType}. Cannot convert from {this.valueType} to bool");
         }
      }

      public bool IsNone => valueType == BValueType.None;

      public virtual BValue this[string key]
      {
         get { return null; }
         set { }
      }

      public virtual BValue this[int index]
      {
         get { return null; }
         set { }
      }

      public virtual void Clear() { }
      public virtual void Add(string key, BValue value) { }
      public virtual void Add(BValue value) { }
      public virtual bool Contains(BValue v) { return false; }
      public virtual bool ContainsKey(string key) { return false; }

      public static implicit operator BValue(double v) => new BValue(v);

      public static implicit operator BValue(Int32 v) => new BValue(v);

      public static implicit operator BValue(Int64 v) => new BValue(v);

      public static implicit operator BValue(byte[] v) => new BValue(v);

      public static implicit operator BValue(DateTime v) => new BValue(v);

      public static implicit operator BValue(string v) => new BValue(v);

      public static implicit operator double(BValue v) => v.DoubleValue;

      public static implicit operator Int32(BValue v) => v.Int32Value;

      public static implicit operator Int64(BValue v) => v.Int64Value;

      public static implicit operator byte[] (BValue v) => v.BinaryValue;

      public static implicit operator DateTime(BValue v) => v.DateTimeValue;

      public static implicit operator string(BValue v) => v.StringValue;

      protected BValue(BValueType valueType)
      {
         this.valueType = valueType;
      }

      public BValue()
      {
         this.valueType = BValueType.None;
      }

      public BValue(double v)
      {
         this.valueType = BValueType.Double;
         _double = v;
      }

      public BValue(String v)
      {
         this.valueType = BValueType.String;
         _string = v;
      }

      public BValue(byte[] v)
      {
         this.valueType = BValueType.Binary;
         _binary = v;
      }

      public BValue(bool v)
      {
         this.valueType = BValueType.Boolean;
         _bool = v;
      }

      public BValue(DateTime dt)
      {
         this.valueType = BValueType.UTCDateTime;
         _dateTime = dt;
      }

      public BValue(Int32 v)
      {
         this.valueType = BValueType.Int32;
         _int32 = v;
      }

      public BValue(Int64 v)
      {
         this.valueType = BValueType.Int64;
         _int64 = v;
      }


      public static bool operator ==(BValue a, object b) => ReferenceEquals(a, b);

      public static bool operator !=(BValue a, object b) => !(a == b);
   }

   public enum BValueType
   {
      Double,
      String,
      Array,
      Binary,
      Boolean,
      UTCDateTime,
      None,
      Int32,
      Int64,
      Object
   };


   public class BObject : BValue, IEnumerable
   {
      private Dictionary<string, BValue> map = new Dictionary<string, BValue>();

      public BObject() : base(BValueType.Object)
      {
      }

      public ICollection<string> Keys => map.Keys;

      public ICollection<BValue> Values => map.Values;

      public int Count => map.Count;

      public override BValue this[string key]
      {
         get => map[key];
         set => map[key] = value;
      }
      
      public override void Clear() => map.Clear();

      public override void Add(string key, BValue value) => map.Add(key, value);


      public override bool Contains(BValue v) => map.ContainsValue(v);

      public override bool ContainsKey(string key) => map.ContainsKey(key);

      public bool Remove(string key) => map.Remove(key);

      public bool TryGetValue(string key, out BValue value) => map.TryGetValue(key, out value);

      IEnumerator IEnumerable.GetEnumerator()
      {
         return map.GetEnumerator();
      }
   }


   public class BArray : BValue, IEnumerable
   {
      private List<BValue> items = new List<BValue>();

      public BArray() : base(BValueType.Array)
      {
      }

      public override BValue this[int index]
      {
         get { return items[index]; }
         set { items[index] = value; }
      }

      public int Count => items.Count;

      public override void Add(BValue v) => items.Add(v);

      public int IndexOf(BValue item) => items.IndexOf(item);

      public void Insert(int index, BValue item) => items.Insert(index, item);

      public bool Remove(BValue v) => items.Remove(v);

      public void RemoveAt(int index) => items.RemoveAt(index);

      public override void Clear() => items.Clear();

      public virtual bool Contains(BValue v) => items.Contains(v);

      IEnumerator IEnumerable.GetEnumerator()
      {
         return items.GetEnumerator();
      }
   }

   public class Bson
   {
      private MemoryStream stream;
      private BinaryReader reader;

      public static BObject Load(byte[] buf)
      {
         var bson = new Bson(buf);

         return bson.DecodeDocument();
      }

      private Bson(byte[] buf)
      {
         stream = new MemoryStream(buf);
         reader = new BinaryReader(stream);
      }

      private BValue DecodeElement(out string name)
      {
         byte elementType = reader.ReadByte();

         if( elementType == 0x01 )
         { // Double
            name = DecodeCString();
            return new BValue(reader.ReadDouble());
         }
         if( elementType == 0x02 )
         { // String
            name = DecodeCString();
            return new BValue(DecodeString());
         }
         if( elementType == 0x03 )
         { // Document
            name = DecodeCString();
            return DecodeDocument();
         }
         if( elementType == 0x04 )
         { // Array
            name = DecodeCString();
            return DecodeArray();
         }
         if( elementType == 0x05 )
         { // Binary
            name = DecodeCString();
            int length = reader.ReadInt32();
            byte binaryType = reader.ReadByte();

            return new BValue(reader.ReadBytes(length));
         }
         if( elementType == 0x08 )
         { // Boolean
            name = DecodeCString();
            return new BValue(reader.ReadBoolean());
         }
         if( elementType == 0x09 )
         { // DateTime
            name = DecodeCString();
            Int64 time = reader.ReadInt64();
            return new BValue(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(time * 10000));
         }
         if( elementType == 0x0A )
         { // None
            name = DecodeCString();
            return new BValue();
         }
         if( elementType == 0x10 )
         { // Int32
            name = DecodeCString();
            return new BValue(reader.ReadInt32());
         }
         if( elementType == 0x12 )
         { // Int64
            name = DecodeCString();
            return new BValue(reader.ReadInt64());
         }

         throw new Exception($"Don't know elementType={elementType}");
      }

      private BObject DecodeDocument()
      {
         int length = reader.ReadInt32() - 4;

         BObject obj = new BObject();

         int i = (int)reader.BaseStream.Position;
         while( reader.BaseStream.Position < i + length - 1 )
         {
            string name;
            BValue value = DecodeElement(out name);
            obj.Add(name, value);
         }

         reader.ReadByte(); // zero
         return obj;
      }

      private BArray DecodeArray()
      {
         BObject obj = DecodeDocument();

         int i = 0;
         BArray array = new BArray();
         while( obj.ContainsKey(Convert.ToString(i)) )
         {
            array.Add(obj[Convert.ToString(i)]);

            i += 1;
         }

         return array;
      }

      private string DecodeString()
      {
         int length = reader.ReadInt32();
         byte[] buf = reader.ReadBytes(length);

         return Encoding.UTF8.GetString(buf);
      }

      private string DecodeCString()
      {
         using( var ms = new MemoryStream() )
         {
            while( true )
            {
               byte buf = reader.ReadByte();
               if( buf == 0 )
                  break;
               ms.WriteByte(buf);
            }
            return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Position);
         }
      }
   }
}

