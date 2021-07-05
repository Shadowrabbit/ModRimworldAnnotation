using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000748 RID: 1864
	public class JobDriver_PruneGauranlenTre : JobDriver
	{
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x001257B4 File Offset: 0x001239B4
		private CompTreeConnection TreeConnection
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x001257DC File Offset: 0x001239DC
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			float num = this.TreeConnection.DesiredConnectionStrength - this.TreeConnection.ConnectionStrength;
			this.numPositions = Mathf.Min(8, Mathf.CeilToInt(num / this.TreeConnection.ConnectionStrengthGainPerHourOfPruning) + 1);
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x00125827 File Offset: 0x00123A27
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil findAdjacentCell = Toils_General.Do(delegate
			{
				this.job.targetB = this.GetAdjacentCell(this.job.GetTarget(TargetIndex.A).Thing);
			});
			Toil goToAdjacentCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell).FailOn(() => this.TreeConnection.ConnectionStrength >= this.TreeConnection.DesiredConnectionStrength);
			Toil prune = Toils_General.WaitWith(TargetIndex.A, 2500, true, false).WithEffect(EffecterDefOf.Harvest_MetaOnly, TargetIndex.A, null).WithEffect(EffecterDefOf.GauranlenDebris, TargetIndex.A, null).PlaySustainerOrSound(SoundDefOf.Interact_Prune, 1f);
			prune.AddPreTickAction(delegate
			{
				this.TreeConnection.Prune();
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Plants, 0.085f, false);
				}
				if (this.TreeConnection.ConnectionStrength >= this.TreeConnection.DesiredConnectionStrength)
				{
					base.ReadyForNextToil();
				}
			});
			int num;
			for (int i = 0; i < this.numPositions; i = num + 1)
			{
				yield return findAdjacentCell;
				yield return goToAdjacentCell;
				yield return prune;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x00125838 File Offset: 0x00123A38
		private IntVec3 GetAdjacentCell(Thing treeThing)
		{
			IntVec3 result;
			if ((from x in GenAdj.CellsAdjacent8Way(treeThing)
			where x.InBounds(this.pawn.Map) && !x.Fogged(this.pawn.Map) && !x.IsForbidden(this.pawn) && this.pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return treeThing.Position;
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0012586D File Offset: 0x00123A6D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numPositions, "numPositions", 1, false);
		}

		// Token: 0x04001E0F RID: 7695
		private int numPositions = 1;

		// Token: 0x04001E10 RID: 7696
		private const TargetIndex TreeIndex = TargetIndex.A;

		// Token: 0x04001E11 RID: 7697
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;

		// Token: 0x04001E12 RID: 7698
		private const int DurationTicks = 2500;

		// Token: 0x04001E13 RID: 7699
		private const int MaxPositions = 8;
	}
}
