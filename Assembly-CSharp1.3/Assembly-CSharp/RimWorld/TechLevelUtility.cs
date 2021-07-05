using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C5C RID: 3164
	public static class TechLevelUtility
	{
		// Token: 0x060049E6 RID: 18918 RVA: 0x00186E3C File Offset: 0x0018503C
		public static string ToStringHuman(this TechLevel tl)
		{
			switch (tl)
			{
			case TechLevel.Undefined:
				return "Undefined".Translate();
			case TechLevel.Animal:
				return "TechLevel_Animal".Translate();
			case TechLevel.Neolithic:
				return "TechLevel_Neolithic".Translate();
			case TechLevel.Medieval:
				return "TechLevel_Medieval".Translate();
			case TechLevel.Industrial:
				return "TechLevel_Industrial".Translate();
			case TechLevel.Spacer:
				return "TechLevel_Spacer".Translate();
			case TechLevel.Ultra:
				return "TechLevel_Ultra".Translate();
			case TechLevel.Archotech:
				return "TechLevel_Archotech".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060049E7 RID: 18919 RVA: 0x00186EFC File Offset: 0x001850FC
		public static bool CanSpawnWithEquipmentFrom(this TechLevel pawnLevel, TechLevel gearLevel)
		{
			if (gearLevel == TechLevel.Undefined)
			{
				return false;
			}
			switch (pawnLevel)
			{
			case TechLevel.Undefined:
				return false;
			case TechLevel.Neolithic:
				return gearLevel <= TechLevel.Neolithic;
			case TechLevel.Medieval:
				return gearLevel <= TechLevel.Medieval;
			case TechLevel.Industrial:
				return gearLevel == TechLevel.Industrial;
			case TechLevel.Spacer:
				return gearLevel == TechLevel.Spacer || gearLevel == TechLevel.Industrial;
			case TechLevel.Ultra:
				return gearLevel == TechLevel.Ultra || gearLevel == TechLevel.Spacer;
			case TechLevel.Archotech:
				return gearLevel == TechLevel.Archotech;
			}
			Log.Error(string.Concat(new object[]
			{
				"Unknown tech levels ",
				pawnLevel,
				", ",
				gearLevel
			}));
			return true;
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x00186F9B File Offset: 0x0018519B
		public static bool IsNeolithicOrWorse(this TechLevel techLevel)
		{
			return techLevel != TechLevel.Undefined && techLevel <= TechLevel.Neolithic;
		}
	}
}
