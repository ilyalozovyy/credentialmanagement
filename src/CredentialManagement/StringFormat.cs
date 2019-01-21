using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static CredentialManagement.NativeMethods;

namespace CredentialManagement {
    public enum StringFormat {
        Unicode,
        Ansi,
    }

    internal abstract class StringFormatProvider {
        public abstract byte[] GetBytes(string Value);
        public abstract IntPtr StringToCoTaskMem(string Value);
        public abstract string PtrToString(IntPtr Value, int Size);

        public static StringFormatProvider GetProvider(StringFormat Format) {
            var ret = default(StringFormatProvider);

            switch (Format) {
                case StringFormat.Ansi:
                    ret = new AnsiStringFormatProvider();
                    break;
                case StringFormat.Unicode:
                    ret = new UnicodeStringFormatProvider();
                    break;
                default:
                    break;
            }

            return ret;
        }

    }

    internal class AnsiStringFormatProvider : StringFormatProvider {
        public override byte[] GetBytes(string Value) {
            return System.Text.Encoding.ASCII.GetBytes(Value);
        }

        public override IntPtr StringToCoTaskMem(string Value) {
            return Marshal.StringToCoTaskMemAnsi(Value);
        }

        public override string PtrToString(IntPtr Value, int Size) {
            return Marshal.PtrToStringAnsi(Value, Size);
        }
    }

    internal class UnicodeStringFormatProvider : StringFormatProvider {
        public override byte[] GetBytes(string Value) {
            return System.Text.Encoding.Unicode.GetBytes(Value);
        }

        public override IntPtr StringToCoTaskMem(string Value) {
            return Marshal.StringToCoTaskMemUni(Value);
        }

        public override string PtrToString(IntPtr Value, int Size) {
            return Marshal.PtrToStringUni(Value, Size / 2);
        }
    }



}
