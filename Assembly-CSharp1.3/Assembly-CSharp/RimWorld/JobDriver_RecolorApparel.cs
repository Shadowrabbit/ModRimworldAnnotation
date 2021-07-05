using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200072C RID: 1836
	public class JobDriver_RecolorApparel : JobDriver
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x00123E54 File Offset: 0x00122054
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			Thing thing = this.job.GetTarget(TargetIndex.C).Thing;
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			int num = this.job.GetTargetQueue(TargetIndex.B).Count;
			foreach (LocalTargetInfo target in this.job.GetTargetQueue(TargetIndex.A))
			{
				int num2 = Mathf.Min(num, target.Thing.stackCount);
				if (!this.pawn.Reserve(target, this.job, 1, num2, null, errorOnFailed))
				{
					return false;
				}
				num -= num2;
				if (num2 <= 0)
				{
					break;
				}
			}
			return true;
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x00123F64 File Offset: 0x00122164
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Apparel recoloring"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.C);
			Thing thing = this.job.GetTarget(TargetIndex.C).Thing;
			foreach (Toil toil in JobDriver_DoBill.CollectIngredientsToils(TargetIndex.A, TargetIndex.C, TargetIndex.B, true, false))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.InteractionCell);
			Toil extract = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return extract;
			Toil toil2 = Toils_General.Wait(1000, TargetIndex.None);
			toil2.PlaySustainerOrSound(SoundDefOf.Interact_RecolorApparel, 1f);
			toil2.WithProgressBarToilDelay(TargetIndex.C, false, -0.5f);
			yield return toil2;
			yield return Toils_General.Do(delegate
			{
				ThingCountClass thingCountClass = this.job.placedThings[0];
				if (thingCountClass.thing.stackCount == 1)
				{
					thingCountClass.thing.Destroy(DestroyMode.Vanish);
					this.job.placedThings.RemoveAt(0);
				}
				else
				{
					thingCountClass.thing.SplitOff(1).Destroy(DestroyMode.Vanish);
				}
				this.job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompColorable>().Recolor();
			});
			yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, extract);
			extract = null;
			yield break;
			yield break;
		}

		// Token: 0x04001DE2 RID: 7650
		public const TargetIndex DyeInd = TargetIndex.A;

		// Token: 0x04001DE3 RID: 7651
		public const TargetIndex ApparelInd = TargetIndex.B;

		// Token: 0x04001DE4 RID: 7652
		public const TargetIndex StylingStationInd = TargetIndex.C;

		// Token: 0x04001DE5 RID: 7653
		public const int WorkTimeTicks = 1000;
	}
}
