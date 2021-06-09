using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B40 RID: 2880
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x060043A2 RID: 17314 RVA: 0x000321AF File Offset: 0x000303AF
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x000321BF File Offset: 0x000303BF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Animal.training.NextTrainableToTrain() == null && !base.OnLastToil);
			yield break;
			yield break;
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x000321CF File Offset: 0x000303CF
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryTrain(TargetIndex.A);
		}
	}
}
