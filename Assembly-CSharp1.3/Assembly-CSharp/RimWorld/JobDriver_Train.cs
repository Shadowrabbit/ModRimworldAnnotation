using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D0 RID: 1744
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x0011E217 File Offset: 0x0011C417
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x0011E227 File Offset: 0x0011C427
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Func<bool> noLongerTrainable = () => base.Animal.training.NextTrainableToTrain() == null;
			foreach (Toil toil in base.MakeNewToils())
			{
				toil.FailOn(noLongerTrainable);
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Interpersonal.TryTrain(TargetIndex.A);
			yield break;
			yield break;
		}
	}
}
