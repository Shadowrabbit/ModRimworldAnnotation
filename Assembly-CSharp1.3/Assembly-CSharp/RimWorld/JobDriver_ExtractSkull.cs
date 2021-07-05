using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000712 RID: 1810
	public class JobDriver_ExtractSkull : JobDriver
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x001221F5 File Offset: 0x001203F5
		protected Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x0012220C File Offset: 0x0012040C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Skull extraction"))
			{
				yield break;
			}
			this.FailOn(() => this.Corpse.Destroyed || !this.Corpse.Spawned);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = Toils_General.Wait(180, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.PlaySustainerOrSound(SoundDefOf.Recipe_Surgery, 1f);
			Toil toil2 = toil;
			toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
			{
				if (Rand.Chance(0.016666668f))
				{
					CellRect cellRect = new CellRect(this.Corpse.PositionHeld.x - 1, this.Corpse.PositionHeld.z - 1, 3, 3);
					IntVec3 randomCell = cellRect.RandomCell;
					if (randomCell.InBounds(base.Map) && GenSight.LineOfSight(randomCell, this.Corpse.PositionHeld, base.Map, false, null, 0, 0))
					{
						FilthMaker.TryMakeFilth(randomCell, this.Corpse.MapHeld, this.Corpse.InnerPawn.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
					}
				}
			}));
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.Corpse.InnerPawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).First((BodyPartRecord p) => p.def == BodyPartDefOf.Head), null, null);
				this.Corpse.Map.designationManager.RemoveDesignation(this.Corpse.Map.designationManager.DesignationOn(this.Corpse, DesignationDefOf.ExtractSkull));
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Skull, null), this.Corpse.PositionHeld, this.Corpse.Map, ThingPlaceMode.Near, null, null, default(Rot4));
			});
			yield break;
		}

		// Token: 0x04001DB8 RID: 7608
		public const int ExtractionTimeTicks = 180;

		// Token: 0x04001DB9 RID: 7609
		private const TargetIndex CorpseInd = TargetIndex.A;
	}
}
