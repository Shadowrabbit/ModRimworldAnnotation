using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200177E RID: 6014
	public static class PsychicDroneLevelUtility
	{
		// Token: 0x06008497 RID: 33943 RVA: 0x00274138 File Offset: 0x00272338
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

		// Token: 0x06008498 RID: 33944 RVA: 0x00058CC9 File Offset: 0x00056EC9
		public static string GetLabelCap(this PsychicDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
