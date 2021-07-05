using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F0 RID: 4336
	public static class PsychicDroneLevelUtility
	{
		// Token: 0x060067CE RID: 26574 RVA: 0x00232118 File Offset: 0x00230318
		public static string GetLabel(this PsychicDroneLevel level)
		{
			switch (level)
			{
			case PsychicDroneLevel.None:
				return "PsychicDroneLevel_None".Translate();
			case PsychicDroneLevel.GoodMedium:
				return "PsychicDroneLevel_GoodMedium".Translate();
			case PsychicDroneLevel.BadLow:
				return "PsychicDroneLevel_BadLow".Translate();
			case PsychicDroneLevel.BadMedium:
				return "PsychicDroneLevel_BadMedium".Translate();
			case PsychicDroneLevel.BadHigh:
				return "PsychicDroneLevel_BadHigh".Translate();
			case PsychicDroneLevel.BadExtreme:
				return "PsychicDroneLevel_BadExtreme".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x002321AA File Offset: 0x002303AA
		public static string GetLabelCap(this PsychicDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
