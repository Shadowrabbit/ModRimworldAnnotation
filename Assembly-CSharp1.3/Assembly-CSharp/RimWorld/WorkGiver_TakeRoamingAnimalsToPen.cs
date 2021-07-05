using System;

namespace RimWorld
{
	// Token: 0x0200080C RID: 2060
	public class WorkGiver_TakeRoamingAnimalsToPen : WorkGiver_TakeToPen
	{
		// Token: 0x060036F4 RID: 14068 RVA: 0x001373B6 File Offset: 0x001355B6
		public WorkGiver_TakeRoamingAnimalsToPen()
		{
			this.targetRoamingAnimals = true;
			this.allowUnenclosedPens = true;
		}
	}
}
