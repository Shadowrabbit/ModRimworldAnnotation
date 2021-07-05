using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D14 RID: 3348
	public static class ZonePresetNames
	{
		// Token: 0x06004E63 RID: 20067 RVA: 0x001A4505 File Offset: 0x001A2705
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
