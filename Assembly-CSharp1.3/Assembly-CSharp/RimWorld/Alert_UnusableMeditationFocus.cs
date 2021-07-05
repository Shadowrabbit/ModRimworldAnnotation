using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001280 RID: 4736
	public class Alert_UnusableMeditationFocus : Alert
	{
		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x0600712E RID: 28974 RVA: 0x0025B69C File Offset: 0x0025989C
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnEntries.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					if (pawn.timetable != null && pawn.timetable.CurrentAssignment == TimeAssignmentDefOf.Meditate && pawn.psychicEntropy.IsCurrentlyMeditating && !MeditationFocusDefOf.Natural.CanPawnUse(pawn))
					{
						JobDriver_Meditate jobDriver_Meditate = pawn.jobs.curDriver as JobDriver_Meditate;
						if (jobDriver_Meditate != null && !(jobDriver_Meditate.Focus != null) && !(jobDriver_Meditate is JobDriver_Reign))
						{
							foreach (Thing thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, MeditationUtility.FocusObjectSearchRadius, false))
							{
								if (thing.def == ThingDefOf.Plant_TreeAnima || thing.def == ThingDefOf.AnimusStone || thing.def == ThingDefOf.NatureShrine_Small || thing.def == ThingDefOf.NatureShrine_Large)
								{
									this.targets.Add(pawn);
									this.pawnEntries.Add(pawn.LabelShort + " (" + thing.LabelShort + ")");
									break;
								}
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600712F RID: 28975 RVA: 0x0025B858 File Offset: 0x00259A58
		public Alert_UnusableMeditationFocus()
		{
			this.defaultLabel = "UnusableMeditationFocusAlert".Translate();
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x0025B88B File Offset: 0x00259A8B
		public override TaggedString GetExplanation()
		{
			return "UnusableMeditationFocusAlertDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x0025B8AC File Offset: 0x00259AAC
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E40 RID: 15936
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E41 RID: 15937
		private List<string> pawnEntries = new List<string>();

		// Token: 0x020025FC RID: 9724
		public class Alert_PermitAvailable : Alert
		{
			// Token: 0x0600D4BA RID: 54458 RVA: 0x00405CF5 File Offset: 0x00403EF5
			public Alert_PermitAvailable()
			{
				this.defaultLabel = "PermitChoiceReadyAlert".Translate();
			}

			// Token: 0x17002097 RID: 8343
			// (get) Token: 0x0600D4BB RID: 54459 RVA: 0x00405D20 File Offset: 0x00403F20
			private List<GlobalTargetInfo> Targets
			{
				get
				{
					this.targets.Clear();
					foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
					{
						if (pawn.royalty != null && pawn.royalty.PermitPointsAvailable)
						{
							this.targets.Add(pawn);
						}
					}
					return this.targets;
				}
			}

			// Token: 0x17002098 RID: 8344
			// (get) Token: 0x0600D4BC RID: 54460 RVA: 0x00405DA4 File Offset: 0x00403FA4
			private Pawn FirstPawn
			{
				get
				{
					List<GlobalTargetInfo> list = this.Targets;
					if (!list.Any<GlobalTargetInfo>())
					{
						return null;
					}
					return (Pawn)((Thing)list[0]);
				}
			}

			// Token: 0x0600D4BD RID: 54461 RVA: 0x00405DD3 File Offset: 0x00403FD3
			public override AlertReport GetReport()
			{
				if (!ModsConfig.RoyaltyActive)
				{
					return false;
				}
				return AlertReport.CulpritsAre(this.Targets);
			}

			// Token: 0x0600D4BE RID: 54462 RVA: 0x00405DEE File Offset: 0x00403FEE
			public override TaggedString GetExplanation()
			{
				return "PermitChoiceReadyAlertDesc".Translate(this.FirstPawn.Named("PAWN"));
			}

			// Token: 0x0600D4BF RID: 54463 RVA: 0x00405E0A File Offset: 0x0040400A
			protected override void OnClick()
			{
				base.OnClick();
				this.FirstPawn.royalty.OpenPermitWindow();
			}

			// Token: 0x040090DD RID: 37085
			private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
		}
	}
}
