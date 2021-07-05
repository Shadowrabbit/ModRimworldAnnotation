using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001289 RID: 4745
	public class Alert_AnimaLinkingReady : Alert
	{
		// Token: 0x0600715A RID: 29018 RVA: 0x0025C8FE File Offset: 0x0025AAFE
		public Alert_AnimaLinkingReady()
		{
			this.defaultLabel = "AnimaLinkingReadyLabel".Translate();
		}

		// Token: 0x0600715B RID: 29019 RVA: 0x0025C93C File Offset: 0x0025AB3C
		private void GetTargets()
		{
			this.culprits.Clear();
			this.targetLabels.Clear();
			this.tempPawns.Clear();
			foreach (LordJob_Ritual lordJob_Ritual in Find.IdeoManager.GetActiveRituals(Find.CurrentMap))
			{
				if (lordJob_Ritual.Ritual != null && lordJob_Ritual.Ritual.def == PreceptDefOf.AnimaTreeLinking)
				{
					return;
				}
			}
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.GetPsylinkLevel() < pawn.GetMaxPsylinkLevel() && MeditationFocusDefOf.Natural.CanPawnUse(pawn))
				{
					this.tempPawns.Add(pawn);
				}
			}
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeAnima))
			{
				CompPsylinkable compPsylinkable = thing.TryGetComp<CompPsylinkable>();
				int count = compPsylinkable.CompSubplant.SubplantsForReading.Count;
				bool flag = false;
				foreach (Pawn pawn2 in this.tempPawns)
				{
					if (compPsylinkable.GetRequiredPlantCount(pawn2) <= count)
					{
						if (!this.culprits.Contains(pawn2))
						{
							this.culprits.Add(pawn2);
							this.targetLabels.Add(pawn2.NameFullColored);
						}
						flag = true;
					}
				}
				if (flag)
				{
					this.culprits.Add(thing);
				}
			}
		}

		// Token: 0x0600715C RID: 29020 RVA: 0x0025CB3C File Offset: 0x0025AD3C
		public override TaggedString GetExplanation()
		{
			return "AnimaLinkingReadyDesc".Translate() + ":\n\n" + this.targetLabels.ToLineList(" -  ") + "\n\n" + "AnimaLinkingReadyDescExtra".Translate();
		}

		// Token: 0x0600715D RID: 29021 RVA: 0x0025CB8B File Offset: 0x0025AD8B
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive || Find.CurrentMap == null)
			{
				return AlertReport.Inactive;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.culprits);
		}

		// Token: 0x04003E57 RID: 15959
		private List<Thing> culprits = new List<Thing>();

		// Token: 0x04003E58 RID: 15960
		private List<string> targetLabels = new List<string>();

		// Token: 0x04003E59 RID: 15961
		private List<Pawn> tempPawns = new List<Pawn>();
	}
}
