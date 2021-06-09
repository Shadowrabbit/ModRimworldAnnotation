using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D80 RID: 7552
	public class ThreatsGeneratorParams : IExposable
	{
		// Token: 0x0600A429 RID: 42025 RVA: 0x002FCA2C File Offset: 0x002FAC2C
		public void ExposeData()
		{
			Scribe_Values.Look<AllowedThreatsGeneratorThreats>(ref this.allowedThreats, "allowedThreats", AllowedThreatsGeneratorThreats.None, false);
			Scribe_Values.Look<int>(ref this.randSeed, "randSeed", 0, false);
			Scribe_Values.Look<float>(ref this.onDays, "onDays", 0f, false);
			Scribe_Values.Look<float>(ref this.offDays, "offDays", 0f, false);
			Scribe_Values.Look<float>(ref this.minSpacingDays, "minSpacingDays", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.numIncidentsRange, "numIncidentsRange", default(FloatRange), false);
			Scribe_Values.Look<float?>(ref this.threatPoints, "threatPoints", null, false);
			Scribe_Values.Look<float?>(ref this.minThreatPoints, "minThreatPoints", null, false);
			Scribe_Values.Look<float>(ref this.currentThreatPointsFactor, "currentThreatPointsFactor", 1f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x0600A42A RID: 42026 RVA: 0x002FCB14 File Offset: 0x002FAD14
		public override string ToString()
		{
			string text = "(";
			text = text + "onDays=" + this.onDays.ToString("0.##");
			text = text + " offDays=" + this.offDays.ToString("0.##");
			text = text + " minSpacingDays=" + this.minSpacingDays.ToString("0.##");
			text = text + " numIncidentsRange=" + this.numIncidentsRange;
			if (this.threatPoints != null)
			{
				text = text + " threatPoints=" + this.threatPoints.Value;
			}
			if (this.minThreatPoints != null)
			{
				text = text + " minThreatPoints=" + this.minThreatPoints.Value;
			}
			if (this.faction != null)
			{
				text = text + " faction=" + this.faction;
			}
			return text + ")";
		}

		// Token: 0x04006F4A RID: 28490
		public AllowedThreatsGeneratorThreats allowedThreats;

		// Token: 0x04006F4B RID: 28491
		public int randSeed;

		// Token: 0x04006F4C RID: 28492
		public float onDays;

		// Token: 0x04006F4D RID: 28493
		public float offDays;

		// Token: 0x04006F4E RID: 28494
		public float minSpacingDays;

		// Token: 0x04006F4F RID: 28495
		public FloatRange numIncidentsRange;

		// Token: 0x04006F50 RID: 28496
		public float? threatPoints;

		// Token: 0x04006F51 RID: 28497
		public float? minThreatPoints;

		// Token: 0x04006F52 RID: 28498
		public float currentThreatPointsFactor = 1f;

		// Token: 0x04006F53 RID: 28499
		public Faction faction;
	}
}
