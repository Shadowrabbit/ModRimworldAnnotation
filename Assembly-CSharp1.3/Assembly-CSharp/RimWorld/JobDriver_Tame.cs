using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006CF RID: 1743
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003098 RID: 12440 RVA: 0x0011E0F6 File Offset: 0x0011C2F6
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x0011E106 File Offset: 0x0011C306
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Func<bool> noLongerDesignated = () => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null;
			foreach (Toil toil in base.MakeNewToils())
			{
				toil.FailOn(noLongerDesignated);
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
					if (pawn == null || !AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						return;
					}
					if (pawn.Faction != Faction.OfPlayer)
					{
						return;
					}
					if (AnimalPenUtility.GetCurrentPenOf(pawn, false) == null)
					{
						RopingPriority ropingPriority = RopingPriority.Closest;
						string text;
						CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, pawn, out text, false, true, true, false, ropingPriority);
						Job job = null;
						if (penAnimalShouldBeTakenTo != null)
						{
							job = WorkGiver_TakeToPen.MakeJob(this.pawn, pawn, penAnimalShouldBeTakenTo, true, ropingPriority, out text);
						}
						if (job != null)
						{
							this.pawn.jobs.StartJob(job, JobCondition.Succeeded, null, false, true, null, null, false, false);
							return;
						}
						Messages.Message("MessageTameNoSuitablePens".Translate(pawn.Named("ANIMAL")), pawn, MessageTypeDefOf.NeutralEvent, true);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
			yield break;
		}
	}
}
