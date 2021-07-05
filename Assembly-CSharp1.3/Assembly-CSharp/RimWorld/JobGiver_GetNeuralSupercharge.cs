using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007C1 RID: 1985
	public class JobGiver_GetNeuralSupercharge : ThinkNode_JobGiver
	{
		// Token: 0x060035A1 RID: 13729 RVA: 0x0012F298 File Offset: 0x0012D498
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0f;
			}
			int lastReceivedNeuralSuperchargeTick = pawn.health.lastReceivedNeuralSuperchargeTick;
			if (lastReceivedNeuralSuperchargeTick != -1 && Find.TickManager.TicksGame - lastReceivedNeuralSuperchargeTick < 30000)
			{
				return 0f;
			}
			if (this.ClosestSupercharger(pawn) == null)
			{
				return 0f;
			}
			return 9.25f;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x0012F2F0 File Offset: 0x0012D4F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.ClosestSupercharger(pawn);
			if (thing == null || !pawn.CanReserve(thing, 1, -1, null, false))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.GetNeuralSupercharge, thing);
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x0012F32C File Offset: 0x0012D52C
		private Thing ClosestSupercharger(Pawn pawn)
		{
			JobGiver_GetNeuralSupercharge.<>c__DisplayClass2_0 CS$<>8__locals1 = new JobGiver_GetNeuralSupercharge.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			return GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForDef(ThingDefOf.NeuralSupercharger), PathEndMode.InteractionCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, new Predicate<Thing>(CS$<>8__locals1.<ClosestSupercharger>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false);
		}
	}
}
