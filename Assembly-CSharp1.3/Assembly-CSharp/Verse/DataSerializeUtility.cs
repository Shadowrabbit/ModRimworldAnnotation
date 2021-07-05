using System;

namespace Verse
{
	// Token: 0x02000478 RID: 1144
	public static class DataSerializeUtility
	{
		// Token: 0x06002279 RID: 8825 RVA: 0x000DABC4 File Offset: 0x000D8DC4
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00072AAA File Offset: 0x00070CAA
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00072AAA File Offset: 0x00070CAA
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000DABF0 File Offset: 0x000D8DF0
		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, arr[i]);
			}
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000DAC1C File Offset: 0x000D8E1C
		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int i = 0; i < elements; i++)
			{
				ushort num = reader(i);
				array[i * 2] = (byte)(num & 255);
				array[i * 2 + 1] = (byte)(num >> 8 & 255);
			}
			return array;
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000DAC68 File Offset: 0x000D8E68
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000DAC9C File Offset: 0x000D8E9C
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000DACE0 File Offset: 0x000D8EE0
		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (ushort)((int)arr[i * 2] | (int)arr[i * 2 + 1] << 8));
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000DAD18 File Offset: 0x000D8F18
		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				int num = reader(i);
				array[i * 4] = (byte)(num & 255);
				array[i * 4 + 1] = (byte)(num >> 8 & 255);
				array[i * 4 + 2] = (byte)(num >> 16 & 255);
				array[i * 4 + 3] = (byte)(num >> 24 & 255);
			}
			return array;
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000DAD88 File Offset: 0x000D8F88
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000DADBC File Offset: 0x000D8FBC
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000DAE00 File Offset: 0x000D9000
		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24);
			}
		}
	}
}
