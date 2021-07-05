using System;

namespace Verse
{
	// Token: 0x02000217 RID: 535
	public static class MapExposeUtility
	{
		// Token: 0x06000F4A RID: 3914 RVA: 0x00056C38 File Offset: 0x00054E38
		public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUshort(map, shortReader);
			}
			DataExposeUtility.ByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUshort(arr, map, shortWriter);
			}
		}
	}
}
