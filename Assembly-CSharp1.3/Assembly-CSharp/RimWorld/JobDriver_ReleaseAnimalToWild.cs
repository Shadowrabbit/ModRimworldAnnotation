using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006C6 RID: 1734
	public class JobDriver_ReleaseAnimalToWild : JobDriver
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x0011D55B File Offset: 0x0011B75B
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x0011D760 File Offset: 0x0011B960
		public static bool TryFindClosestOutsideCell(IntVec3 root, Map map, TraverseParms traverseParms, out IntVec3 cell)
		{
			cell = default(IntVec3);
			Region region;
			if (CellFinder.TryFindClosestRegionWith(root.GetRegion(map, RegionType.Set_Passable), traverseParms, (Region r) => r.District.TouchesMapEdge, 99999, out region, RegionType.Set_Passable))
			{
				float num = float.PositiveInfinity;
				foreach (IntVec3 intVec in region.Cells)
				{
					if (intVec.Standable(map))
					{
						float num2 = (float)intVec.DistanceToSquared(root);
						if (num2 < num)
						{
							num = num2;
							cell = intVec;
						}
					}
				}
				return num != float.PositiveInfinity;
			}
			return false;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x0011D81C File Offset: 0x0011BA1C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Animal.Dead);
			this.FailOn(() => this.Animal.Faction != Faction.OfPlayer);
			this.FailOn(() => this.Animal.InAggroMentalState);
			this.FailOn(() => this.Animal.MapHeld.designationManager.DesignationOn(this.Animal, DesignationDefOf.ReleaseAnimalToWild) == null);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
			Toil waitToil = Toils_General.WaitWith(TargetIndex.A, 180, false, false).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Jump.JumpIf(waitToil, () => this.Animal.Position.GetRegion(base.Map, RegionType.Set_Passable).District.TouchesMapEdge);
			JobDriver_ReleaseAnimalToWild.<>c__DisplayClass6_0 CS$<>8__locals1 = new JobDriver_ReleaseAnimalToWild.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			CS$<>8__locals1.toil = new Toil();
			CS$<>8__locals1.toil.initAction = delegate()
			{
				Pawn actor = CS$<>8__locals1.toil.actor;
				IntVec3 c;
				if (!JobDriver_ReleaseAnimalToWild.TryFindClosestOutsideCell(actor.Position, actor.Map, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false, false, false), out c))
				{
					CS$<>8__locals1.<>4__this.EndJobWith(JobCondition.Incompletable);
					return;
				}
				actor.pather.StartPath(c, PathEndMode.OnCell);
			};
			CS$<>8__locals1.toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			yield return CS$<>8__locals1.toil;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.A, null, false, false);
			CS$<>8__locals1 = null;
			yield return waitToil;
			yield return Toils_General.Do(delegate
			{
				ReleaseAnimalToWildUtility.DoReleaseAnimal(this.Animal, this.pawn);
			});
			yield break;
		}

		// Token: 0x04001D48 RID: 7496
		private const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04001D49 RID: 7497
		private const int WaitForTicks = 180;
	}
}
