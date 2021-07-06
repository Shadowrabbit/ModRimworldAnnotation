using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001355 RID: 4949
	public static class ZonePresetNames
	{
		// Token: 0x06006B85 RID: 27525 RVA: 0x000492C3 File Offset: 0x000474C3
		public static string PresetName(this StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				return "DumpingStockpile".Translate();
			}
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				return "Stockpile".Translate();
			}
			return "Zone".Translate();
		}
	}
}
