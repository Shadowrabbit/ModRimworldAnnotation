using System;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017AC RID: 6060
	public static class CaravanRideableUtility
	{
		// Token: 0x06008C7E RID: 35966 RVA: 0x00327142 File Offset: 0x00325342
		public static bool IsCaravanRideable(this Pawn pawn)
		{
			return !pawn.Downed && pawn.ageTracker.CurLifeStage.caravanRideable && pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true) > 1f;
		}

		// Token: 0x06008C7F RID: 35967 RVA: 0x00327173 File Offset: 0x00325373
		public static bool IsCaravanRideable(this ThingDef def)
		{
			return def.StatBaseDefined(StatDefOf.CaravanRidingSpeedFactor);
		}

		// Token: 0x06008C80 RID: 35968 RVA: 0x00327180 File Offset: 0x00325380
		public static string RideableLifeStagesDesc(RaceProperties raceProps)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LifeStageAge lifeStageAge in raceProps.lifeStageAges)
			{
				if (lifeStageAge.def.caravanRideable)
				{
					stringBuilder.AppendWithComma(lifeStageAge.def.label);
				}
			}
			return stringBuilder.ToString().CapitalizeFirst();
		}

		// Token: 0x06008C81 RID: 35969 RVA: 0x003271FC File Offset: 0x003253FC
		public static string GetIconTooltipText(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true);
			return "RideableAnimalTip".Translate() + "\n\n" + StatDefOf.CaravanRidingSpeedFactor.LabelCap + ": " + statValue.ToStringPercent();
		}
	}
}
