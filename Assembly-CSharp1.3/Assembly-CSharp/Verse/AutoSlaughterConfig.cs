using System;

namespace Verse
{
	// Token: 0x02000182 RID: 386
	public class AutoSlaughterConfig : IExposable
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0003BCE0 File Offset: 0x00039EE0
		public bool AnyLimit
		{
			get
			{
				return this.maxTotal != -1 || this.maxMales != -1 || this.maxFemales != -1;
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0003BD04 File Offset: 0x00039F04
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.animal, "animal");
			Scribe_Values.Look<int>(ref this.maxTotal, "maxTotal", -1, false);
			Scribe_Values.Look<int>(ref this.maxMales, "maxMales", -1, false);
			Scribe_Values.Look<int>(ref this.maxFemales, "maxFemales", -1, false);
		}

		// Token: 0x0400092B RID: 2347
		public ThingDef animal;

		// Token: 0x0400092C RID: 2348
		public int maxTotal = -1;

		// Token: 0x0400092D RID: 2349
		public int maxMales = -1;

		// Token: 0x0400092E RID: 2350
		public int maxFemales = -1;

		// Token: 0x0400092F RID: 2351
		public string uiMaxTotalBuffer;

		// Token: 0x04000930 RID: 2352
		public string uiMaxMalesBuffer;

		// Token: 0x04000931 RID: 2353
		public string uiMaxFemalesBuffer;

		// Token: 0x04000932 RID: 2354
		public const int NoLimit = -1;
	}
}
