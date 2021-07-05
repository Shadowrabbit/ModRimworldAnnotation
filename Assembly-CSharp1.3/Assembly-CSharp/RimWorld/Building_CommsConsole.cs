using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001072 RID: 4210
	public class Building_CommsConsole : Building
	{
		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x060063EB RID: 25579 RVA: 0x0021BAEA File Offset: 0x00219CEA
		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ElectricityDisabled) && (this.powerComp == null || this.powerComp.PowerOn);
			}
		}

		// Token: 0x060063EC RID: 25580 RVA: 0x0021BB1D File Offset: 0x00219D1D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
		}

		// Token: 0x060063ED RID: 25581 RVA: 0x0021BB4C File Offset: 0x00219D4C
		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x060063EE RID: 25582 RVA: 0x0021BB90 File Offset: 0x00219D90
		private FloatMenuOption GetFailureReason(Pawn myPawn)
		{
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled)
			{
				return new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (this.powerComp != null && !this.powerComp.PowerOn)
			{
				return new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				return new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label, myPawn.Named("PAWN"))), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!this.GetCommTargets(myPawn).Any<ICommunicable>())
			{
				return new FloatMenuOption("CannotUseReason".Translate("NoCommsTarget".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!this.CanUseCommsNow)
			{
				Log.Error(myPawn + " could not use comm console for unknown reason.");
				return new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x060063EF RID: 25583 RVA: 0x0021BD18 File Offset: 0x00219F18
		public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
		{
			return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat((from f in Find.FactionManager.AllFactionsVisibleInViewOrder
			where !f.temporary && !f.IsPlayer
			select f).Cast<ICommunicable>());
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x0021BD72 File Offset: 0x00219F72
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			FloatMenuOption failureReason = this.GetFailureReason(myPawn);
			if (failureReason != null)
			{
				yield return failureReason;
				yield break;
			}
			foreach (ICommunicable communicable in this.GetCommTargets(myPawn))
			{
				FloatMenuOption floatMenuOption = communicable.CommFloatMenuOption(this, myPawn);
				if (floatMenuOption != null)
				{
					yield return floatMenuOption;
				}
			}
			IEnumerator<ICommunicable> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060063F1 RID: 25585 RVA: 0x0021BD8C File Offset: 0x00219F8C
		public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = target;
			negotiator.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x0400387C RID: 14460
		private CompPowerTrader powerComp;
	}
}
