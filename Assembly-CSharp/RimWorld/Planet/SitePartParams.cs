using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002166 RID: 8550
	public class SitePartParams : IExposable
	{
		// Token: 0x0600B639 RID: 46649 RVA: 0x0034AF50 File Offset: 0x00349150
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.randomValue, "randomValue", 0, false);
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousLumpResources, "preciousLumpResources");
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.turretsCount, "turretsCount", 0, false);
			Scribe_Values.Look<int>(ref this.mortarsCount, "mortarsCount", 0, false);
		}

		// Token: 0x04007CD0 RID: 31952
		public int randomValue;

		// Token: 0x04007CD1 RID: 31953
		public float threatPoints;

		// Token: 0x04007CD2 RID: 31954
		public ThingDef preciousLumpResources;

		// Token: 0x04007CD3 RID: 31955
		public PawnKindDef animalKind;

		// Token: 0x04007CD4 RID: 31956
		public int turretsCount;

		// Token: 0x04007CD5 RID: 31957
		public int mortarsCount;
	}
}
