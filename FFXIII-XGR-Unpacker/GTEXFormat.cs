using System;

namespace FFXIII_XGR_Unpacker
{
    public class GTEXFormat
    {
        public enum GtexPixelFormat : byte
        {
            X8R8G8B8 = 0x03,
            A8R8G8B8 = 0x04,
            DXT1 = 0x18,
            DXT3 = 0x19,
            DXT5 = 0x1A
        }
        public static int GetFormat(GtexPixelFormat gtexFormat)
        {
            switch (gtexFormat)
            {
                case GtexPixelFormat.DXT1:
                    return 0x31545844;
                case GtexPixelFormat.DXT3:
                    return 0x33545844;
                case GtexPixelFormat.DXT5:
                    return 0x35545844;
                default:
                    return 0;
            }
        }
        public static int GetFlags(GtexPixelFormat gtexFormat)
        {
            switch (gtexFormat)
            {
                case GtexPixelFormat.A8R8G8B8:
                    return 0x00000041;
                case GtexPixelFormat.X8R8G8B8:
                    return 0x00000040;
                default:
                    return 0x00000004;
            }
        }
        public static int GetRGBBitCount(GtexPixelFormat gtexFormat)
        {
            if (gtexFormat == GtexPixelFormat.A8R8G8B8 || gtexFormat == GtexPixelFormat.X8R8G8B8)
            {
                return 0x00000020;
            }
            else
            {
                return 0;
            }
        }
        public static int GetRBitMask(GtexPixelFormat gtexFormat)
        {
            if (gtexFormat == GtexPixelFormat.A8R8G8B8 || gtexFormat == GtexPixelFormat.X8R8G8B8)
            {
                return 0x00FF0000;
            }
            else
            {
                return 0;
            }
        }
        public static uint GetGBitMask(GtexPixelFormat gtexFormat)
        {
            if (gtexFormat == GtexPixelFormat.A8R8G8B8 || gtexFormat == GtexPixelFormat.X8R8G8B8)
            {
                return 0x0000FF00;
            }
            else
            {
                return 0;
            }
        }
        public static int GetBBitMask(GtexPixelFormat gtexFormat)
        {
            if (gtexFormat == GtexPixelFormat.A8R8G8B8 || gtexFormat == GtexPixelFormat.X8R8G8B8)
            {
                return 0x000000FF;
            }
            else
            {
                return 0;
            }
        }
        public static uint GetABitMask(GtexPixelFormat gtexFormat)
        {
            switch (gtexFormat)
            {
                case GtexPixelFormat.A8R8G8B8:
                    return 0xFF000000;
                default:
                    return 0;
            }
        }
    }
}
