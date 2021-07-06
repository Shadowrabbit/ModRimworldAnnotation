using System;

namespace Verse
{
	// Token: 0x020007D2 RID: 2002
	public static class DataSerializeUtility
	{
		// Token: 0x0600323B RID: 12859 RVA: 0x0014D13C File Offset: 0x0014B33C
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000187F7 File Offset: 0x000169F7
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000187F7 File Offset: 0x000169F7
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x0014D168 File Offset: 0x0014B368
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

		// Token: 0x0600323F RID: 12863 RVA: 0x0014D194 File Offset: 0x0014B394
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

		// Token: 0x06003240 RID: 12864 RVA: 0x0014D1E0 File Offset: 0x0014B3E0
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x0014D214 File Offset: 0x0014B414
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x0014D258 File Offset: 0x0014B458
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

		// Token: 0x06003243 RID: 12867 RVA: 0x0014D290 File Offset: 0x0014B490
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

		// Token: 0x06003244 RID: 12868 RVA: 0x0014D300 File Offset: 0x0014B500
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x0014D334 File Offset: 0x0014B534
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0014D378 File Offset: 0x0014B578
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
