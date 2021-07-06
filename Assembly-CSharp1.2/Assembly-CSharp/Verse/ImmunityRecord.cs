using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000424 RID: 1060
	public class ImmunityRecord : IExposable
	{
		// Token: 0x060019D2 RID: 6610 RVA: 0x00018130 File Offset: 0x00016330
		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00018168 File Offset: 0x00016368
		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x000E3B60 File Offset: 0x000E1D60
		public float ImmunityChangePerTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.hediffDef.CompProps<HediffCompProperties_Immunizable>();
			if (sick)
			{
				float num = hediffCompProperties_Immunizable.immunityPerDaySick;
				num *= pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true);
				if (diseaseInstance != null)
				{
					Rand.PushState();
					Rand.Seed = Gen.HashCombineInt(diseaseInstance.loadID ^ Find.World.info.persistentRandomValue, 156482735);
					num *= Mathf.Lerp(0.8f, 1.2f, Rand.Value);
					Rand.PopState();
				}
				return num / 60000f;
			}
			return hediffCompProperties_Immunizable.immunityPerDayNotSick / 60000f;
		}

		// Token: 0x0400133A RID: 4922
		public HediffDef hediffDef;

		// Token: 0x0400133B RID: 4923
		public HediffDef source;

		// Token: 0x0400133C RID: 4924
		public float immunity;
	}
}
