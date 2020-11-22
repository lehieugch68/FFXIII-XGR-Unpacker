using System;
using System.IO;

namespace FFXIII_XGR_Unpacker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Final Fantasy XIII XGR Unpacker by LeHieu - VietHoaGame";
            if (args.Length > 0)
            {
                foreach (string file in args)
                {
                    string ext = Path.GetExtension(file).ToLower();
                    FileAttributes attr = File.GetAttributes(file);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        Console.WriteLine($"{Path.GetFileNameWithoutExtension(file)}.win32.xgr");
                        XGR.Repack(file, Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}.win32.xgr"));
                    }
                    else if (ext == ".xgr")
                    {
                        if (!Directory.Exists(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)))) Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                        XGR.Unpack(file, Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    }
                }
            }
            else
            {
                Console.WriteLine("Please drag and drop files/folder into this tool to unpack/repack.");
            }
            Console.ReadKey();
        }
    }
}
