using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200180F RID: 6159
	public class CompPsylinkable : ThingComp
	{
		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x0600884A RID: 34890 RVA: 0x0005B853 File Offset: 0x00059A53
		public CompProperties_Psylinkable Props
		{
			get
			{
				return (CompProperties_Psylinkable)this.props;
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x0600884B RID: 34891 RVA: 0x0005B860 File Offset: 0x00059A60
		public CompSpawnSubplant CompSubplant
		{
			get
			{
				return this.parent.TryGetComp<CompSpawnSubplant>();
			}
		}

		// Token: 0x0600884C RID: 34892 RVA: 0x0027E5C4 File Offset: 0x0027C7C4
		private IEnumerable<Pawn> GetPawnsThatCanPsylink(int level = -1)
		{
			return from p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists
			where this.Props.requiredFocus.CanPawnUse(p) && this.GetRequiredPlantCount(p) <= this.CompSubplant.SubplantsForReading.Count && (level == -1 || p.GetPsylinkLevel() == level)
			select p;
		}

		// Token: 0x0600884D RID: 34893 RVA: 0x0005B86D File Offset: 0x00059A6D
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CompSubplant.onGrassGrown = new Action(this.OnGrassGrown);
		}

		// Token: 0x0600884E RID: 34894 RVA: 0x0027E5FC File Offset: 0x0027C7FC
		private void OnGrassGrown()
		{
			bool flag = false;
			foreach (Pawn item in this.GetPawnsThatCanPsylink(-1))
			{
				if (!this.pawnsThatCanPsylinkLastGrassGrow.Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
				string text = "";
				for (int i = 0; i < this.Props.requiredSubplantCountPerPsylinkLevel.Count; i++)
				{
					IEnumerable<string> enumerable = from p in this.GetPawnsThatCanPsylink(i)
					select p.LabelShort;
					if (enumerable.Count<string>() > 0)
					{
						text = string.Concat(new object[]
						{
							text,
							"- " + "Level".Translate() + " ",
							i + 1,
							": ",
							this.Props.requiredSubplantCountPerPsylinkLevel[i],
							" ",
							compSpawnSubplant.Props.subplant.label,
							" (",
							enumerable.ToCommaList(false),
							")\n"
						});
					}
				}
				Find.LetterStack.ReceiveLetter(this.Props.enoughPlantsLetterLabel, this.Props.enoughPlantsLetterText.Formatted(compSpawnSubplant.SubplantsForReading.Count, text.TrimEndNewlines()), LetterDefOf.NeutralEvent, new LookTargets(this.GetPawnsThatCanPsylink(-1)), null, null, null, null);
			}
			this.pawnsThatCanPsylinkLastGrassGrow.Clear();
			this.pawnsThatCanPsylinkLastGrassGrow.AddRange(this.GetPawnsThatCanPsylink(-1));
		}

		// Token: 0x0600884F RID: 34895 RVA: 0x0027E7E8 File Offset: 0x0027C9E8
		private int GetRequiredPlantCount(Pawn pawn)
		{
			int psylinkLevel = pawn.GetPsylinkLevel();
			if (this.parent.TryGetComp<CompSpawnSubplant>() == null)
			{
				Log.Warning("CompPsylinkable with requiredSubplantCountPerPsylinkLevel set on a Thing without CompSpawnSubplant!", false);
				return -1;
			}
			int result;
			if (this.Props.requiredSubplantCountPerPsylinkLevel.Count <= psylinkLevel)
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel.Last<int>();
			}
			else
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel[psylinkLevel];
			}
			return result;
		}

		// Token: 0x06008850 RID: 34896 RVA: 0x0027E850 File Offset: 0x0027CA50
		public AcceptanceReport CanPsylink(Pawn pawn, LocalTargetInfo? knownSpot = null)
		{
			if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!this.Props.requiredFocus.CanPawnUse(pawn))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedFocus".Translate(this.Props.requiredFocus.label));
			}
			if (pawn.GetPsylinkLevel() >= pawn.GetMaxPsylinkLevel())
			{
				return new AcceptanceReport("InstallImplantAlreadyMaxLevel".Translate());
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, this.parent, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(this.parent, pawn);
				return new AcceptanceReport((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2));
			}
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				return new AcceptanceReport("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()));
			}
			LocalTargetInfo localTargetInfo;
			if (knownSpot != null)
			{
				if (!this.CanUseSpot(pawn, knownSpot.Value))
				{
					return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
				}
			}
			else if (!this.TryFindLinkSpot(pawn, out localTargetInfo))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06008851 RID: 34897 RVA: 0x0027EA1C File Offset: 0x0027CC1C
		public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
		{
			spot = MeditationUtility.FindMeditationSpot(pawn).spot;
			if (this.CanUseSpot(pawn, spot))
			{
				return true;
			}
			int num = GenRadial.NumCellsInRadius(2.9f);
			int num2 = GenRadial.NumCellsInRadius(3.9f);
			for (int i = num; i < num2; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.CanUseSpot(pawn, c))
				{
					spot = c;
					return true;
				}
			}
			spot = IntVec3.Zero;
			return false;
		}

		// Token: 0x06008852 RID: 34898 RVA: 0x0027EAB8 File Offset: 0x0027CCB8
		private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
		{
			IntVec3 cell = spot.Cell;
			return cell.DistanceTo(this.parent.Position) <= 3.9f && cell.Standable(this.parent.Map) && GenSight.LineOfSight(cell, this.parent.Position, this.parent.Map, false, null, 0, 0) && pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
		}

		// Token: 0x06008853 RID: 34899 RVA: 0x0005B886 File Offset: 0x00059A86
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (pawn.Dead || pawn.Drafted)
			{
				yield break;
			}
			string text = "BeginLinkingRitualFloatMenu".Translate();
			AcceptanceReport acceptanceReport = this.CanPsylink(pawn, null);
			if (!acceptanceReport.Accepted && !string.IsNullOrWhiteSpace(acceptanceReport.Reason))
			{
				text = text + ": " + acceptanceReport.Reason;
			}
			Action <>9__1;
			yield return new FloatMenuOption(text, delegate()
			{
				TaggedString psylinkAffectedByTraitsNegativelyWarning = RoyalTitleUtility.GetPsylinkAffectedByTraitsNegativelyWarning(pawn);
				if (psylinkAffectedByTraitsNegativelyWarning != null)
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text2 = psylinkAffectedByTraitsNegativelyWarning;
					string buttonAText = "Confirm".Translate();
					Action buttonAAction;
					if ((buttonAAction = <>9__1) == null)
					{
						buttonAAction = (<>9__1 = delegate()
						{
							this.BeginLinkingRitual(pawn);
						});
					}
					windowStack.Add(new Dialog_MessageBox(text2, buttonAText, buttonAAction, "GoBack".Translate(), null, null, false, null, null));
					return;
				}
				this.BeginLinkingRitual(pawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null)
			{
				Disabled = !acceptanceReport.Accepted
			};
			yield break;
		}

		// Token: 0x06008854 RID: 34900 RVA: 0x0027EB30 File Offset: 0x0027CD30
		private void BeginLinkingRitual(Pawn pawn)
		{
			LocalTargetInfo localTargetInfo;
			if (!this.TryFindLinkSpot(pawn, out localTargetInfo) || !this.CanPsylink(pawn, new LocalTargetInfo?(localTargetInfo)).Accepted)
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.LinkPsylinkable, this.parent, localTargetInfo);
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		// Token: 0x06008855 RID: 34901 RVA: 0x0027EB88 File Offset: 0x0027CD88
		public void FinishLinkingRitual(Pawn pawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psylinkables are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 5464564, false);
				return;
			}
			MoteMaker.MakeStaticMote(this.parent.Position, pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, 10f);
			SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(this.parent));
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			List<Thing> list = (from p in compSpawnSubplant.SubplantsForReading
			orderby p.Position.DistanceTo(this.parent.Position) descending
			select p).ToList<Thing>();
			int num = 0;
			while (num < requiredPlantCount && num < list.Count)
			{
				list[num].Destroy(DestroyMode.Vanish);
				num++;
			}
			compSpawnSubplant.Cleanup();
			pawn.ChangePsylinkLevel(1, true);
			string str = "LetterTextLinkingRitualCompleted".Translate(pawn.Named("PAWN"), this.parent.Named("LINKABLE"));
			Find.LetterStack.ReceiveLetter("LetterLabelLinkingRitualCompleted".Translate(), str, LetterDefOf.PositiveEvent, new LookTargets(new TargetInfo[]
			{
				pawn,
				this.parent
			}), null, null, null, null);
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x0027ECC8 File Offset: 0x0027CEC8
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsThatCanPsylinkLastGrassGrow, "pawnsThatCanPsylinkLastGrassGrow", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnsThatCanPsylinkLastGrassGrow.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04005777 RID: 22391
		private List<Pawn> pawnsThatCanPsylinkLastGrassGrow = new List<Pawn>();

		// Token: 0x04005778 RID: 22392
		public const float MaxDistance = 3.9f;
	}
}
