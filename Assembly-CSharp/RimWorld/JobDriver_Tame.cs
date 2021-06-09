using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B3E RID: 2878
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004393 RID: 17299 RVA: 0x00032107 File Offset: 0x00030307
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00032117 File Offset: 0x00030317
		protected override IEnumerable<Toil> MakeNewToils()
		{
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null && !base.OnLastToil);
			yield break;
			yield break;
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x00032127 File Offset: 0x00030327
		protected override Toil FinalInteractToil()
		{
			return Toils_Interpersonal.TryRecruit(TargetIndex.A);
		}
	}
}
