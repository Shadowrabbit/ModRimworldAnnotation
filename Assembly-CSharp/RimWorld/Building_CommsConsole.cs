using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016AC RID: 5804
	public class Building_CommsConsole : Building
	{
		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06007F1B RID: 32539 RVA: 0x000555DD File Offset: 0x000537DD
		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ElectricityDisabled) && (this.powerComp == null || this.powerComp.PowerOn);
			}
		}

		// Token: 0x06007F1C RID: 32540 RVA: 0x00055610 File Offset: 0x00053810
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
		}

		// Token: 0x06007F1D RID: 32541 RVA: 0x0025C124 File Offset: 0x0025A324
		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x06007F1E RID: 32542 RVA: 0x0025C164 File Offset: 0x0025A364
		private FloatMenuOption GetFailureReason(Pawn myPawn)
		{
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled)
			{
				return new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (this.powerComp != null && !this.powerComp.PowerOn)
			{
				return new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				return new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label, myPawn.Named("PAWN"))), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (!this.CanUseCommsNow)
			{
				Log.Error(myPawn + " could not use comm console for unknown reason.", false);
				return new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			return null;
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x0025C2A4 File Offset: 0x0025A4A4
		public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
		{
			return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat((from f in Find.FactionManager.AllFactionsVisibleInViewOrder
			where !f.temporary
			select f).Cast<ICommunicable>());
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x0005563C File Offset: 0x0005383C
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

		// Token: 0x06007F21 RID: 32545 RVA: 0x0025C124 File Offset: 0x0025A324
		public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = target;
			negotiator.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x04005296 RID: 21142
		private CompPowerTrader powerComp;
	}
}
