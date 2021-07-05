using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006B3 RID: 1715
	public class Trigger_PawnExperiencingDangerousTemperatures : Trigger
	{
		// Token: 0x06002F73 RID: 12147 RVA: 0x001190EC File Offset: 0x001172EC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity > this.temperatureHediffThreshold)
						{
							return true;
						}
						Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity > this.temperatureHediffThreshold)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04001CFB RID: 7419
		private float temperatureHediffThreshold = 0.15f;
	}
}
