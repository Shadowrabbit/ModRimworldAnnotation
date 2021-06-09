using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001258 RID: 4696
	public static class TechLevelUtility
	{
		// Token: 0x06006669 RID: 26217 RVA: 0x001F96FC File Offset: 0x001F78FC
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

		// Token: 0x0600666A RID: 26218 RVA: 0x001F97BC File Offset: 0x001F79BC
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
			}), false);
			return true;
		}

		// Token: 0x0600666B RID: 26219 RVA: 0x00045F59 File Offset: 0x00044159
		public static bool IsNeolithicOrWorse(this TechLevel techLevel)
		{
			return techLevel != TechLevel.Undefined && techLevel <= TechLevel.Neolithic;
		}
	}
}
