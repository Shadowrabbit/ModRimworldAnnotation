using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000741 RID: 1857
	public class JobDriver_PlantSeed : JobDriver
	{
		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x0012546C File Offset: 0x0012366C
		private IntVec3 PlantCell
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Cell;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003376 RID: 13174 RVA: 0x00125490 File Offset: 0x00123690
		private CompPlantable PlantableComp
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompPlantable>();
			}
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x001254B6 File Offset: 0x001236B6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, 1, null, errorOnFailed);
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x001254D8 File Offset: 0x001236D8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => PlantUtility.AdjacentSowBlocker(this.job.plantDefToSow, this.PlantCell, base.Map) != null || !this.job.plantDefToSow.CanEverPlantAt(this.PlantCell, base.Map, true) || this.job.GetTarget(TargetIndex.B).Cell != this.PlantableComp.PlantCell);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.Touch);
			Toil toil = new Toil();
			toil.tickAction = delegate()
			{
				this.sowWorkDone += this.pawn.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f, false);
				}
				if (this.sowWorkDone >= this.job.plantDefToSow.plant.sowWork)
				{
					base.ReadyForNextToil();
					return;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(EffecterDefOf.Sow, TargetIndex.B, null);
			toil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow, 1f);
			toil.WithProgressBar(TargetIndex.B, () => this.sowWorkDone / this.job.plantDefToSow.plant.sowWork, true, -0.5f, false);
			toil.activeSkill = (() => SkillDefOf.Plants);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.PlantableComp.DoPlant(this.pawn, this.PlantCell, this.pawn.Map);
			});
			yield break;
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x001254E8 File Offset: 0x001236E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x04001E08 RID: 7688
		private float sowWorkDone;

		// Token: 0x04001E09 RID: 7689
		private const TargetIndex SeedIndex = TargetIndex.A;

		// Token: 0x04001E0A RID: 7690
		private const TargetIndex PlantCellIndex = TargetIndex.B;
	}
}
