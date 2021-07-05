using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200186D RID: 6253
	public class CompTechprint : ThingComp
	{
		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06008AB9 RID: 35513 RVA: 0x0005D019 File Offset: 0x0005B219
		public CompProperties_Techprint Props
		{
			get
			{
				return (CompProperties_Techprint)this.props;
			}
		}

		// Token: 0x06008ABA RID: 35514 RVA: 0x0005D026 File Offset: 0x0005B226
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Techprints are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657212, false);
				yield break;
			}
			JobFailReason.Clear();
			if (selPawn.WorkTypeIsDisabled(WorkTypeDefOf.Research) || selPawn.WorkTagIsDisabled(WorkTags.Intellectual))
			{
				JobFailReason.Is("WillNever".Translate("Research".TranslateSimple().UncapitalizeFirst()), null);
			}
			else if (!selPawn.CanReach(this.parent, PathEndMode.ClosestTouch, Danger.Some, false, TraverseMode.ByPawn))
			{
				JobFailReason.Is("CannotReach".Translate(), null);
			}
			else if (!selPawn.CanReserve(this.parent, 1, -1, null, false))
			{
				Pawn pawn = selPawn.Map.reservationManager.FirstRespectedReserver(this.parent, selPawn);
				if (pawn == null)
				{
					pawn = selPawn.Map.physicalInteractionReservationManager.FirstReserverOf(selPawn);
				}
				if (pawn != null)
				{
					JobFailReason.Is("ReservedBy".Translate(pawn.LabelShort, pawn), null);
				}
				else
				{
					JobFailReason.Is("Reserved".Translate(), null);
				}
			}
			HaulAIUtility.PawnCanAutomaticallyHaul(selPawn, this.parent, true);
			Thing thing2 = GenClosest.ClosestThingReachable(selPawn.Position, selPawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(selPawn, Danger.Some, TraverseMode.ByPawn, false), 9999f, (Thing thing) => thing is Building_ResearchBench && selPawn.CanReserve(thing, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
			Job job = null;
			if (thing2 != null)
			{
				job = JobMaker.MakeJob(JobDefOf.ApplyTechprint);
				job.targetA = thing2;
				job.targetB = this.parent;
				job.targetC = thing2.Position;
			}
			if (JobFailReason.HaveReason)
			{
				yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("ApplyTechprint".Translate(this.parent.Label)) + ": " + JobFailReason.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				JobFailReason.Clear();
			}
			else
			{
				yield return new FloatMenuOption("ApplyTechprint".Translate(this.parent.Label).CapitalizeFirst(), delegate()
				{
					if (job == null)
					{
						Messages.Message("MessageNoResearchBenchForTechprint".Translate(), MessageTypeDefOf.RejectInput, true);
						return;
					}
					selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}
	}
}
