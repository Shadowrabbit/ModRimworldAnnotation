using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001287 RID: 4743
	public class Alert_NoRecentSlaughter : Alert
	{
		// Token: 0x06007151 RID: 29009 RVA: 0x0025C390 File Offset: 0x0025A590
		public Alert_NoRecentSlaughter()
		{
			this.defaultLabel = "AlertNoRecentSlaughter".Translate();
		}

		// Token: 0x06007152 RID: 29010 RVA: 0x0025C3D0 File Offset: 0x0025A5D0
		private void GetTargets()
		{
			this.ideoLabels.Clear();
			this.targetLabels.Clear();
			this.targets.Clear();
			int num = Mathf.Max(0, Faction.OfPlayer.ideos.LastAnimalSlaughterTick);
			if (Find.TickManager.TicksGame - num < 600000)
			{
				return;
			}
			bool flag = false;
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.HasMeme(MemeDefOf.Rancher))
				{
					this.ideoLabels.Add(ideo.name.Colorize(ideo.Color));
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (pawn.Ideo != null && pawn.Ideo.HasMeme(MemeDefOf.Rancher) && !pawn.IsSlave && !pawn.IsPrisoner && ThoughtDefOf.NoRecentAnimalSlaughter.minExpectationForNegativeThought != null && ExpectationsUtility.CurrentExpectationFor(pawn.MapHeld).order >= ThoughtDefOf.NoRecentAnimalSlaughter.minExpectationForNegativeThought.order)
				{
					this.targets.Add(pawn);
					this.targetLabels.Add(pawn.NameFullColored.Resolve());
				}
			}
		}

		// Token: 0x06007153 RID: 29011 RVA: 0x0025C56C File Offset: 0x0025A76C
		public override TaggedString GetExplanation()
		{
			return "AlertNoRecentSlaughterDesc".Translate("(" + "BodySizeOrAbove".Translate(0.7f) + ")") + ":\n" + this.ideoLabels.ToLineList(" - ") + "\n\n" + "AlertNoRecentSlaughterDescPost".Translate() + ":\n" + this.targetLabels.ToLineList("  - ");
		}

		// Token: 0x06007154 RID: 29012 RVA: 0x0025C607 File Offset: 0x0025A807
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04003E50 RID: 15952
		private List<string> ideoLabels = new List<string>();

		// Token: 0x04003E51 RID: 15953
		private List<string> targetLabels = new List<string>();

		// Token: 0x04003E52 RID: 15954
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
