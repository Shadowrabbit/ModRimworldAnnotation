using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150A RID: 5386
	public class ThreatsGeneratorParams : IExposable
	{
		// Token: 0x06008058 RID: 32856 RVA: 0x002D78D4 File Offset: 0x002D5AD4
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

		// Token: 0x06008059 RID: 32857 RVA: 0x002D79BC File Offset: 0x002D5BBC
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

		// Token: 0x04004FED RID: 20461
		public AllowedThreatsGeneratorThreats allowedThreats;

		// Token: 0x04004FEE RID: 20462
		public int randSeed;

		// Token: 0x04004FEF RID: 20463
		public float onDays;

		// Token: 0x04004FF0 RID: 20464
		public float offDays;

		// Token: 0x04004FF1 RID: 20465
		public float minSpacingDays;

		// Token: 0x04004FF2 RID: 20466
		public FloatRange numIncidentsRange;

		// Token: 0x04004FF3 RID: 20467
		public float? threatPoints;

		// Token: 0x04004FF4 RID: 20468
		public float? minThreatPoints;

		// Token: 0x04004FF5 RID: 20469
		public float currentThreatPointsFactor = 1f;

		// Token: 0x04004FF6 RID: 20470
		public Faction faction;
	}
}
