using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BD RID: 4541
	public static class ReliquaryUtility
	{
		// Token: 0x06006D64 RID: 28004 RVA: 0x0024A584 File Offset: 0x00248784
		public static float GetRelicConvertPowerFactorForPawn(Pawn pawn, StringBuilder sb = null)
		{
			if (!ModsConfig.IdeologyActive || !pawn.Spawned || pawn.Faction == null || pawn.Ideo == null)
			{
				return 1f;
			}
			int num = 0;
			foreach (Thing thing in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Reliquary))
			{
				if (thing.Faction == pawn.Faction)
				{
					CompRelicContainer compRelicContainer = thing.TryGetComp<CompRelicContainer>();
					if (compRelicContainer != null && !compRelicContainer.Empty)
					{
						CompStyleable compStyleable = compRelicContainer.ContainedThing.TryGetComp<CompStyleable>();
						Precept_ThingStyle precept_ThingStyle = (compStyleable != null) ? compStyleable.SourcePrecept : null;
						if (precept_ThingStyle != null && precept_ThingStyle.ideo == pawn.Ideo)
						{
							num++;
						}
					}
				}
			}
			float num2 = ReliquaryUtility.ConvertPowerFactorFromInstalledRelicsCurve.Evaluate((float)num);
			if (sb != null)
			{
				sb.AppendInNewLine(" -  " + "AbilityIdeoConvertBreakdownRelic".Translate(num.Named("RELICCOUNT")).CapitalizeFirst() + " " + num2.ToStringPercent());
			}
			return num2;
		}

		// Token: 0x04003CB8 RID: 15544
		private static readonly SimpleCurve ConvertPowerFactorFromInstalledRelicsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.1f),
				true
			},
			{
				new CurvePoint(3f, 1.2f),
				true
			},
			{
				new CurvePoint(5f, 1.25f),
				true
			},
			{
				new CurvePoint(10f, 1.3f),
				true
			}
		};
	}
}
