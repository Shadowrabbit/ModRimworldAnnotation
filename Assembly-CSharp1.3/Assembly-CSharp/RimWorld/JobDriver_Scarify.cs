using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006F0 RID: 1776
	public class JobDriver_Scarify : JobDriver
	{
		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x0011FFFC File Offset: 0x0011E1FC
		protected Pawn Target
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x00120024 File Offset: 0x0011E224
		public static bool AvailableOnNow(Pawn pawn, BodyPartRecord part = null)
		{
			return pawn.RaceProps.Humanlike && (Faction.OfPlayerSilentFail == null || Faction.OfPlayer.ideos.AnyPreceptWithRequiredScars()) && (part == null || (!pawn.health.WouldDieAfterAddingHediff(HediffDefOf.Scarification, part, 1f) && !pawn.health.WouldLosePartAfterAddingHediff(HediffDefOf.Scarification, part, 1f)));
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x0012008E File Offset: 0x0011E28E
		public static IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn)
		{
			foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.canScarify)
				{
					yield return bodyPartRecord;
				}
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x001200A0 File Offset: 0x0011E2A0
		public static void Scarify(Pawn pawn, BodyPartRecord part)
		{
			if (!ModLister.CheckIdeology("Scarification"))
			{
				return;
			}
			Lord lord = pawn.GetLord();
			LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation;
			if (lord != null && (lordJob_Ritual_Mutilation = (lord.LordJob as LordJob_Ritual_Mutilation)) != null)
			{
				lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Scarification, pawn, part);
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff.TryGetComp<HediffComp_GetsPermanent>();
			hediffComp_GetsPermanent.IsPermanent = true;
			hediffComp_GetsPermanent.SetPainCategory(JobDriver_Scarify.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted e) => e.weight).category);
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x00120143 File Offset: 0x0011E343
		public static void CreateHistoryEventDef(Pawn pawn)
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotScarified, pawn.Named(HistoryEventArgsNames.Doer)), true);
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x00120165 File Offset: 0x0011E365
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x00120187 File Offset: 0x0011E387
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Scarify"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Pawn target = this.Target;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_General.Wait(35, TargetIndex.None);
			Func<BodyPartRecord, bool> <>9__1;
			yield return new Toil
			{
				initAction = delegate()
				{
					IEnumerable<BodyPartRecord> partsToApplyOn = JobDriver_Scarify.GetPartsToApplyOn(target);
					Func<BodyPartRecord, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((BodyPartRecord part) => JobDriver_Scarify.AvailableOnNow(target, part)));
					}
					BodyPartRecord part2;
					if (!partsToApplyOn.Where(predicate).TryRandomElement(out part2) && !JobDriver_Scarify.GetPartsToApplyOn(target).TryRandomElement(out part2))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Errored, true, true);
						Log.Error("Failed to find body part to scarify");
					}
					JobDriver_Scarify.Scarify(target, part2);
					JobDriver_Scarify.CreateHistoryEventDef(target);
					SoundDefOf.Execute_Cut.PlayOneShot(target);
					if (target.RaceProps.BloodDef != null)
					{
						CellRect cellRect = new CellRect(target.PositionHeld.x - 1, target.PositionHeld.z - 1, 3, 3);
						for (int i = 0; i < 3; i++)
						{
							IntVec3 randomCell = cellRect.RandomCell;
							if (randomCell.InBounds(this.Map) && GenSight.LineOfSight(randomCell, target.PositionHeld, this.Map, false, null, 0, 0))
							{
								FilthMaker.TryMakeFilth(randomCell, target.MapHeld, target.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
							}
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x04001D82 RID: 7554
		private const TargetIndex TargetPawnIndex = TargetIndex.A;

		// Token: 0x04001D83 RID: 7555
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.8f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f)
		};
	}
}
