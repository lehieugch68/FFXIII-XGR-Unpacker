using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Be.IO;

namespace FFXIII_XGR_Unpacker
{
    public class XGR
    {
        public static void Repack(string dir, string xgr)
        {
            string imgb = Path.Combine(Path.GetDirectoryName(xgr), $"{Path.GetFileNameWithoutExtension(xgr)}.imgb");
            if (!File.Exists(imgb)) throw new Exception("IMGB file not found.");
            FileStream stream = File.Open(xgr, FileMode.Open, FileAccess.ReadWrite);
            FileStream imgbStream = File.Open(imgb, FileMode.Open, FileAccess.ReadWrite);
            BeBinaryReader reader = new BeBinaryReader(stream);
            BinaryWriter xgrWriter = new BinaryWriter(stream);
            BinaryWriter imgbWriter = new BinaryWriter(imgbStream);
            if (reader.ReadInt32() == 0x57504400)
            {
                uint fileCount = reader.ReadUInt32();
                reader.BaseStream.Position += 8; //zeroes
                for (int i = 0; i < fileCount; i++)
                {
                    string fileName = Encoding.ASCII.GetString(reader.ReadBytes(16)).Replace($"{(char)0}", string.Empty);
                    uint infoOffset = reader.ReadUInt32();
                    uint infoSize = reader.ReadUInt32();
                    string fileExt = Encoding.ASCII.GetString(reader.ReadBytes(8)).Replace($"{(char)0}", string.Empty);
                    string filePath = Path.Combine(dir, $"{fileName}.{fileExt}");
                    if (!File.Exists($"{filePath}.dds") && !File.Exists(filePath)) continue;
                    long current = reader.BaseStream.Position;
                    reader.BaseStream.Position = infoOffset;
                    if (fileExt == "txbh")
                    {
                        byte[] bytes = File.ReadAllBytes($"{filePath}.dds").Skip(128).ToArray();
                        reader.BaseStream.Position += 88;
                        uint offsetImgb = reader.ReadUInt32();
                        uint size = reader.ReadUInt32();
                        if (bytes.Length != size) throw new Exception("The new file size should be the same as the original file size.");
                        imgbWriter.BaseStream.Position = offsetImgb;
                        imgbWriter.Write(bytes);
                    }
                    else
                    {
                        byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes.Length != infoSize) throw new Exception("The new file size should be the same as the original file size.");
                        xgrWriter.BaseStream.Position = infoOffset;
                        xgrWriter.Write(bytes);
                    }
                    Console.WriteLine($"Repacked: {fileName}.{fileExt}");
                    reader.BaseStream.Position = current;
                }
            }
            else
            {
                throw new Exception("File is not a XGR file.");
            }
            reader.Close();
            xgrWriter.Close();
            stream.Close();
            imgbWriter.Close();
            imgbStream.Close();
        }
        public static void Unpack(string xgr, string dir)
        {
            string imgb = Path.Combine(Path.GetDirectoryName(xgr), $"{Path.GetFileNameWithoutExtension(xgr)}.imgb");
            if (!File.Exists(imgb)) throw new Exception("IMGB file not found.");
            FileStream stream = File.OpenRead(xgr);
            FileStream imgbStream = File.OpenRead(imgb);
            BeBinaryReader reader = new BeBinaryReader(stream);
            BinaryReader imgbReader = new BinaryReader(imgbStream);
            if (reader.ReadInt32() == 0x57504400)
            {
                uint fileCount = reader.ReadUInt32();
                reader.BaseStream.Position += 8; //zeroes
                for (int i = 0; i < fileCount; i++)
                {
                    string fileName = Encoding.ASCII.GetString(reader.ReadBytes(16)).Replace($"{(char)0}", string.Empty);
                    uint infoOffset = reader.ReadUInt32();
                    uint infoSize = reader.ReadUInt32();
                    string fileExt = Encoding.ASCII.GetString(reader.ReadBytes(8)).Replace($"{(char)0}", string.Empty);
                    string filePath = Path.Combine(dir, $"{fileName}.{fileExt}");
                    long current = reader.BaseStream.Position;
                    reader.BaseStream.Position = infoOffset;
                    if (fileExt == "txbh")
                    {
                        string magicWord = Encoding.ASCII.GetString(reader.ReadBytes(8)).Replace($"{(char)0}", string.Empty);
                        reader.BaseStream.Position += 56; //unknow
                        string fileType = Encoding.ASCII.GetString(reader.ReadBytes(4)).Replace($"{(char)0}", string.Empty);
                        reader.BaseStream.Position += 2; //unknow
                        GTEXFormat.GtexPixelFormat format = (GTEXFormat.GtexPixelFormat)reader.ReadByte();
                        byte mimapCount = reader.ReadByte();
                        reader.ReadByte(); //unknow
                        bool isCubeMap = reader.ReadBoolean();
                        ushort width = reader.ReadUInt16();
                        ushort height = reader.ReadUInt16();
                        short depth = reader.ReadInt16();
                        int linerSize = reader.ReadInt32();
                        reader.BaseStream.Position += 4;
                        uint offsetImgb = reader.ReadUInt32();
                        uint size = reader.ReadUInt32();
                        imgbReader.BaseStream.Position = offsetImgb;
                        byte[] raw = imgbReader.ReadBytes((int)size);
                        byte[] result = DDS.Create(width, height, format, mimapCount, depth, linerSize, raw);
                        File.WriteAllBytes($"{filePath}.dds", result);
                    }
                    else
                    {
                        byte[] bytes = reader.ReadBytes((int)infoSize);
                        File.WriteAllBytes($"{filePath}.{fileExt}", bytes);
                    }
                    Console.WriteLine($"Unpacked: {fileName}.{fileExt}");
                    reader.BaseStream.Position = current;
                }
            }
            else
            {
                throw new Exception("File is not a XGR file.");
            }
            reader.Close();
            stream.Close();
            imgbReader.Close();
            imgbStream.Close();
        }
    }
}
