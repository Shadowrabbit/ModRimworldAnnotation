using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace Verse
{
	// Token: 0x020007C7 RID: 1991
	public static class CompressUtility
	{
		// Token: 0x06003213 RID: 12819 RVA: 0x0014BF5C File Offset: 0x0014A15C
		public static byte[] Compress(byte[] input)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
				{
					deflateStream.Write(input, 0, input.Length);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x0014BFC0 File Offset: 0x0014A1C0
		public static byte[] Decompress(byte[] input)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(input))
			{
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
				{
					List<byte[]> list = null;
					byte[] array;
					int num;
					for (;;)
					{
						array = new byte[65536];
						num = deflateStream.Read(array, 0, array.Length);
						if (num < array.Length && list == null)
						{
							break;
						}
						if (num < array.Length)
						{
							goto Block_7;
						}
						if (list == null)
						{
							list = new List<byte[]>();
						}
						list.Add(array);
					}
					byte[] array2 = new byte[num];
					Array.Copy(array, array2, num);
					return array2;
					Block_7:
					byte[] array3 = new byte[num + list.Count * array.Length];
					for (int i = 0; i < list.Count; i++)
					{
						Array.Copy(list[i], 0, array3, i * array.Length, array.Length);
					}
					Array.Copy(array, 0, array3, list.Count * array.Length, num);
					result = array3;
				}
			}
			return result;
		}
	}
}
