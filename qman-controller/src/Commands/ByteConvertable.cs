using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace qman.controller.src.Commands
{
    internal struct ByteConvertable
    {
        public static byte[] GetBytes<T>(T str) where T : struct
        {

            int size = Marshal.SizeOf(typeof(T));
            int Pack = str.GetType().StructLayoutAttribute!.Pack;
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}
