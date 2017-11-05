#pragma warning disable 1591

using System;
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
}