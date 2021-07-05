using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017D9 RID: 6105
	public class SitePartParams : IExposable
	{
		// Token: 0x06008E35 RID: 36405 RVA: 0x00331374 File Offset: 0x0032F574
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.randomValue, "randomValue", 0, false);
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.lootMarketValue, "lootMarketValue", 0f, false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousLumpResources, "preciousLumpResources");
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.turretsCount, "turretsCount", 0, false);
			Scribe_Values.Look<int>(ref this.mortarsCount, "mortarsCount", 0, false);
			Scribe_Deep.Look<ComplexSketch>(ref this.ancientComplexSketch, "ancientComplexSketch", Array.Empty<object>());
			Scribe_Defs.Look<ThingSetMakerDef>(ref this.ancientComplexRewardMaker, "ancientComplexRewardMaker");
			Scribe_Values.Look<string>(ref this.triggerSecuritySignal, "triggerSecuritySignal", null, false);
			Scribe_References.Look<Thing>(ref this.relicThing, "relicThing", false);
			Scribe_Values.Look<string>(ref this.relicLostSignal, "relicLostSignal", null, false);
			Scribe_Values.Look<float>(ref this.interiorThreatPoints, "interiorThreatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.exteriorThreatPoints, "exteriorThreatPoints", 0f, false);
		}

		// Token: 0x040059C8 RID: 22984
		public int randomValue;

		// Token: 0x040059C9 RID: 22985
		public float threatPoints;

		// Token: 0x040059CA RID: 22986
		public float lootMarketValue;

		// Token: 0x040059CB RID: 22987
		public float points;

		// Token: 0x040059CC RID: 22988
		public Precept_Relic relic;

		// Token: 0x040059CD RID: 22989
		public ThingDef preciousLumpResources;

		// Token: 0x040059CE RID: 22990
		public PawnKindDef animalKind;

		// Token: 0x040059CF RID: 22991
		public int turretsCount;

		// Token: 0x040059D0 RID: 22992
		public int mortarsCount;

		// Token: 0x040059D1 RID: 22993
		public ComplexSketch ancientComplexSketch;

		// Token: 0x040059D2 RID: 22994
		public ThingSetMakerDef ancientComplexRewardMaker;

		// Token: 0x040059D3 RID: 22995
		public string triggerSecuritySignal;

		// Token: 0x040059D4 RID: 22996
		public string relicLostSignal;

		// Token: 0x040059D5 RID: 22997
		public Thing relicThing;

		// Token: 0x040059D6 RID: 22998
		public float interiorThreatPoints;

		// Token: 0x040059D7 RID: 22999
		public float exteriorThreatPoints;
	}
}
