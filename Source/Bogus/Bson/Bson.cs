#pragma warning disable 1591

using System;
using System.IO;
using System.Text;

namespace Bogus.Bson
{
   public class Bson
   {
      private readonly MemoryStream stream;
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
         {
            // Double
            name = DecodeCString();
            return new BValue(reader.ReadDouble());
         }
         if( elementType == 0x02 )
         {
            // String
            name = DecodeCString();
            return new BValue(DecodeString());
         }
         if( elementType == 0x03 )
         {
            // Document
            name = DecodeCString();
            return DecodeDocument();
         }
         if( elementType == 0x04 )
         {
            // Array
            name = DecodeCString();
            return DecodeArray();
         }
         if( elementType == 0x05 )
         {
            // Binary
            name = DecodeCString();
            int length = reader.ReadInt32();
            byte binaryType = reader.ReadByte();

            return new BValue(reader.ReadBytes(length));
         }
         if( elementType == 0x08 )
         {
            // Boolean
            name = DecodeCString();
            return new BValue(reader.ReadBoolean());
         }
         if( elementType == 0x09 )
         {
            // DateTime
            name = DecodeCString();
            Int64 time = reader.ReadInt64();
            return new BValue(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + new TimeSpan(time * 10000));
         }
         if( elementType == 0x0A )
         {
            // None
            name = DecodeCString();
            return new BValue();
         }
         if( elementType == 0x10 )
         {
            // Int32
            name = DecodeCString();
            return new BValue(reader.ReadInt32());
         }
         if( elementType == 0x12 )
         {
            // Int64
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
            BValue value = DecodeElement(out var name);
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
         using var ms = new MemoryStream();
         while( true )
         {
            byte buf = reader.ReadByte();
            if( buf == 0 )
               break;
            ms.WriteByte(buf);
         }
         return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Position);
      }


      #region ENCODING

      private Bson()
      {
         stream = new MemoryStream();
      }

      public static byte[] Serialize(BObject obj)
      {
         var bson = new Bson();
         var ms = new MemoryStream();

         bson.EncodeDocument(ms, obj);

         byte[] buf = new byte[ms.Position];
         ms.Seek(0, SeekOrigin.Begin);
         ms.Read(buf, 0, buf.Length);

         return buf;
      }

      private void EncodeElement(MemoryStream ms, string name, BValue v)
      {
         switch( v.ValueType )
         {
            case BValueType.Double:
               ms.WriteByte(0x01);
               EncodeCString(ms, name);
               EncodeDouble(ms, v.DoubleValue);
               return;
            case BValueType.String:
               ms.WriteByte(0x02);
               EncodeCString(ms, name);
               EncodeString(ms, v.StringValue);
               return;
            case BValueType.Object:
               ms.WriteByte(0x03);
               EncodeCString(ms, name);
               EncodeDocument(ms, v as BObject);
               return;
            case BValueType.Array:
               ms.WriteByte(0x04);
               EncodeCString(ms, name);
               EncodeArray(ms, v as BArray);
               return;
            case BValueType.Binary:
               ms.WriteByte(0x05);
               EncodeCString(ms, name);
               EncodeBinary(ms, v.BinaryValue);
               return;
            case BValueType.Boolean:
               ms.WriteByte(0x08);
               EncodeCString(ms, name);
               EncodeBool(ms, v.BoolValue);
               return;
            case BValueType.UTCDateTime:
               ms.WriteByte(0x09);
               EncodeCString(ms, name);
               EncodeUTCDateTime(ms, v.DateTimeValue);
               return;
            case BValueType.None:
               ms.WriteByte(0x0A);
               EncodeCString(ms, name);
               return;
            case BValueType.Int32:
               ms.WriteByte(0x10);
               EncodeCString(ms, name);
               EncodeInt32(ms, v.Int32Value);
               return;
            case BValueType.Int64:
               ms.WriteByte(0x12);
               EncodeCString(ms, name);
               EncodeInt64(ms, v.Int64Value);
               return;
         }
      }

      private void EncodeDocument(MemoryStream ms, BObject obj)
      {
         var dms = new MemoryStream();
         foreach( string str in obj.Keys )
         {
            EncodeElement(dms, str, obj[str]);
         }

         var bw = new BinaryWriter(ms);
         bw.Write((Int32)(dms.Position + 4 + 1));
         bw.Write(dms.ToArray(), 0, (int)dms.Position);
         bw.Write((byte)0);
      }

      private void EncodeArray(MemoryStream ms, BArray lst)
      {
         var obj = new BObject();
         for( int i = 0; i < lst.Count; ++i )
         {
            obj.Add(Convert.ToString(i), lst[i]);
         }

         EncodeDocument(ms, obj);
      }

      private void EncodeBinary(MemoryStream ms, byte[] buf)
      {
         byte[] aBuf = BitConverter.GetBytes(buf.Length);
         ms.Write(aBuf, 0, aBuf.Length);
         ms.WriteByte(0);
         ms.Write(buf, 0, buf.Length);
      }

      private void EncodeCString(MemoryStream ms, string v)
      {
         byte[] buf = new UTF8Encoding().GetBytes(v);
         ms.Write(buf, 0, buf.Length);
         ms.WriteByte(0);
      }

      private void EncodeString(MemoryStream ms, string v)
      {
         byte[] strBuf = new UTF8Encoding().GetBytes(v);
         byte[] buf = BitConverter.GetBytes(strBuf.Length + 1);

         ms.Write(buf, 0, buf.Length);
         ms.Write(strBuf, 0, strBuf.Length);
         ms.WriteByte(0);
      }

      private void EncodeDouble(MemoryStream ms, double v)
      {
         byte[] buf = BitConverter.GetBytes(v);
         ms.Write(buf, 0, buf.Length);
      }

      private void EncodeBool(MemoryStream ms, bool v)
      {
         byte[] buf = BitConverter.GetBytes(v);
         ms.Write(buf, 0, buf.Length);
      }

      private void EncodeInt32(MemoryStream ms, Int32 v)
      {
         byte[] buf = BitConverter.GetBytes(v);
         ms.Write(buf, 0, buf.Length);
      }

      private void EncodeInt64(MemoryStream ms, Int64 v)
      {
         byte[] buf = BitConverter.GetBytes(v);
         ms.Write(buf, 0, buf.Length);
      }

      private void EncodeUTCDateTime(MemoryStream ms, DateTime dt)
      {
         TimeSpan span;
         if( dt.Kind == DateTimeKind.Local )
         {
            span = (dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime());
         }
         else
         {
            span = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
         }
         byte[] buf = BitConverter.GetBytes((Int64)(span.TotalSeconds * 1000));
         ms.Write(buf, 0, buf.Length);
      }
      #endregion
   }
}

