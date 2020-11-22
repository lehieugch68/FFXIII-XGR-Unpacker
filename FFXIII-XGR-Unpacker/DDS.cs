using System;
using System.IO;

namespace FFXIII_XGR_Unpacker
{
    public class DDS
    {
        public static byte[] Create(ushort width, ushort height, GTEXFormat.GtexPixelFormat format, byte mimapCount, short depth, int linerSize, byte[] raw)
        {
            MemoryStream result = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(result);
            writer.Write(0x20534444); //DDS magic

            writer.Write(0x0000007c); //header size
            writer.Write(0x00801007); //flags
            writer.Write((uint)height);
            writer.Write((uint)width);
            writer.Write(linerSize); //liner
            writer.Write((int)depth); //depth
            writer.Write((int)mimapCount); //mimap count
            writer.Write(new byte[11 * 4]); //reserved1
                                            //DDS Pixel Format
            writer.Write(0x000020); //size
            writer.Write(GTEXFormat.GetFlags(format)); //flags
            writer.Write(GTEXFormat.GetFormat(format)); //fourCC
            writer.Write(GTEXFormat.GetRGBBitCount(format)); //RGB bit count
            writer.Write(GTEXFormat.GetRBitMask(format)); //RBit mark
            writer.Write(GTEXFormat.GetGBitMask(format)); //GBit mark
            writer.Write(GTEXFormat.GetBBitMask(format)); //BBit mark
            writer.Write(GTEXFormat.GetABitMask(format)); //ABit mark
            writer.Write(0x00001000); //caps
            writer.Write(new byte[4]); //caps2
            writer.Write(new byte[4]); //caps3
            writer.Write(new byte[4]); //caps4
            writer.Write(new byte[4]); //reserved2
                                       //Raw Texture
            writer.Write(raw);
            writer.Close();
            return result.ToArray();
        }
    }
}
