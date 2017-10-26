using System;
using System.IO;
using System.Text;

namespace Bogus.Bson
{
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

