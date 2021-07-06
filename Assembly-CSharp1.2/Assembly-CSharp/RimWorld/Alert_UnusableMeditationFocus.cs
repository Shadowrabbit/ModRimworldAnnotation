using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001971 RID: 6513
	public class Alert_UnusableMeditationFocus : Alert
	{
		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x06008FFF RID: 36863 RVA: 0x002971B8 File Offset: 0x002953B8
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

		// Token: 0x06009000 RID: 36864 RVA: 0x00060A2D File Offset: 0x0005EC2D
		public Alert_UnusableMeditationFocus()
		{
			this.defaultLabel = "UnusableMeditationFocusAlert".Translate();
		}

		// Token: 0x06009001 RID: 36865 RVA: 0x00060A60 File Offset: 0x0005EC60
		public override TaggedString GetExplanation()
		{
			return "UnusableMeditationFocusAlertDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x06009002 RID: 36866 RVA: 0x00060A81 File Offset: 0x0005EC81
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04005B95 RID: 23445
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04005B96 RID: 23446
		private List<string> pawnEntries = new List<string>();

		// Token: 0x02001972 RID: 6514
		public class Alert_PermitAvailable : Alert
		{
			// Token: 0x06009003 RID: 36867 RVA: 0x00060A9C File Offset: 0x0005EC9C
			public Alert_PermitAvailable()
			{
				this.defaultLabel = "PermitChoiceReadyAlert".Translate();
			}

			// Token: 0x170016C2 RID: 5826
			// (get) Token: 0x06009004 RID: 36868 RVA: 0x00297374 File Offset: 0x00295574
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

			// Token: 0x170016C3 RID: 5827
			// (get) Token: 0x06009005 RID: 36869 RVA: 0x002973F8 File Offset: 0x002955F8
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

			// Token: 0x06009006 RID: 36870 RVA: 0x00060AC4 File Offset: 0x0005ECC4
			public override AlertReport GetReport()
			{
				if (!ModsConfig.RoyaltyActive)
				{
					return false;
				}
				return AlertReport.CulpritsAre(this.Targets);
			}

			// Token: 0x06009007 RID: 36871 RVA: 0x00060ADF File Offset: 0x0005ECDF
			public override TaggedString GetExplanation()
			{
				return "PermitChoiceReadyAlertDesc".Translate(this.FirstPawn.Named("PAWN"));
			}

			// Token: 0x06009008 RID: 36872 RVA: 0x00060AFB File Offset: 0x0005ECFB
			protected override void OnClick()
			{
				base.OnClick();
				this.FirstPawn.royalty.OpenPermitWindow();
			}

			// Token: 0x04005B97 RID: 23447
			private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
		}
	}
}
