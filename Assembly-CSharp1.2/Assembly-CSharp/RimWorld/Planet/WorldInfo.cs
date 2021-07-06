using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020B4 RID: 8372
	public class WorldInfo : IExposable
	{
		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x0600B183 RID: 45443 RVA: 0x0007359E File Offset: 0x0007179E
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x0600B184 RID: 45444 RVA: 0x000735AB File Offset: 0x000717AB
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x0600B185 RID: 45445 RVA: 0x00338614 File Offset: 0x00336814
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<float>(ref this.planetCoverage, "planetCoverage", 0f, false);
			Scribe_Values.Look<string>(ref this.seedString, "seedString", null, false);
			Scribe_Values.Look<int>(ref this.persistentRandomValue, "persistentRandomValue", 0, false);
			Scribe_Values.Look<OverallRainfall>(ref this.overallRainfall, "overallRainfall", OverallRainfall.AlmostNone, false);
			Scribe_Values.Look<OverallTemperature>(ref this.overallTemperature, "overallTemperature", OverallTemperature.VeryCold, false);
			Scribe_Values.Look<IntVec3>(ref this.initialMapSize, "initialMapSize", default(IntVec3), false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04007A4C RID: 31308
		public string name = "DefaultWorldName";

		// Token: 0x04007A4D RID: 31309
		public float planetCoverage;

		// Token: 0x04007A4E RID: 31310
		public string seedString = "SeedError";

		// Token: 0x04007A4F RID: 31311
		public int persistentRandomValue = Rand.Int;

		// Token: 0x04007A50 RID: 31312
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x04007A51 RID: 31313
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x04007A52 RID: 31314
		public OverallPopulation overallPopulation = OverallPopulation.Normal;

		// Token: 0x04007A53 RID: 31315
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);
	}
}
