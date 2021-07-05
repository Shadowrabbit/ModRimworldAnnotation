using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200178F RID: 6031
	public class WorldInfo : IExposable
	{
		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06008B5E RID: 35678 RVA: 0x003210BE File Offset: 0x0031F2BE
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x06008B5F RID: 35679 RVA: 0x003210CB File Offset: 0x0031F2CB
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x06008B60 RID: 35680 RVA: 0x003210D8 File Offset: 0x0031F2D8
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<float>(ref this.planetCoverage, "planetCoverage", 0f, false);
			Scribe_Values.Look<string>(ref this.seedString, "seedString", null, false);
			Scribe_Values.Look<int>(ref this.persistentRandomValue, "persistentRandomValue", 0, false);
			Scribe_Values.Look<OverallRainfall>(ref this.overallRainfall, "overallRainfall", OverallRainfall.AlmostNone, false);
			Scribe_Values.Look<OverallTemperature>(ref this.overallTemperature, "overallTemperature", OverallTemperature.VeryCold, false);
			Scribe_Values.Look<IntVec3>(ref this.initialMapSize, "initialMapSize", default(IntVec3), false);
			Scribe_Collections.Look<FactionDef, int>(ref this.factionCounts, "factionCounts", LookMode.Def, LookMode.Value);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x040058D1 RID: 22737
		public string name = "DefaultWorldName";

		// Token: 0x040058D2 RID: 22738
		public float planetCoverage;

		// Token: 0x040058D3 RID: 22739
		public string seedString = "SeedError";

		// Token: 0x040058D4 RID: 22740
		public int persistentRandomValue = Rand.Int;

		// Token: 0x040058D5 RID: 22741
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x040058D6 RID: 22742
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x040058D7 RID: 22743
		public OverallPopulation overallPopulation = OverallPopulation.Normal;

		// Token: 0x040058D8 RID: 22744
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);

		// Token: 0x040058D9 RID: 22745
		public Dictionary<FactionDef, int> factionCounts;
	}
}
