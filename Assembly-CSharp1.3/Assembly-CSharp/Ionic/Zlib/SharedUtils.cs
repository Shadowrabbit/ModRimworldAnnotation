using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02001838 RID: 6200
	internal class SharedUtils
	{
		// Token: 0x060091D8 RID: 37336 RVA: 0x0034765E File Offset: 0x0034585E
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x060091D9 RID: 37337 RVA: 0x00347668 File Offset: 0x00345868
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x060091DA RID: 37338 RVA: 0x003476A9 File Offset: 0x003458A9
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x060091DB RID: 37339 RVA: 0x003476B6 File Offset: 0x003458B6
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
