using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x0200221C RID: 8732
	internal class SharedUtils
	{
		// Token: 0x0600BB90 RID: 48016 RVA: 0x00079645 File Offset: 0x00077845
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x0600BB91 RID: 48017 RVA: 0x003615B0 File Offset: 0x0035F7B0
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

		// Token: 0x0600BB92 RID: 48018 RVA: 0x0007964D File Offset: 0x0007784D
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x0600BB93 RID: 48019 RVA: 0x0007965A File Offset: 0x0007785A
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
