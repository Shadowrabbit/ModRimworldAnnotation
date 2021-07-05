using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DF RID: 735
	public class ImmunityRecord : IExposable
	{
		// Token: 0x060013DF RID: 5087 RVA: 0x00070E38 File Offset: 0x0006F038
		public void ExposeData()
		{
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Defs.Look<HediffDef>(ref this.source, "source");
			Scribe_Values.Look<float>(ref this.immunity, "immunity", 0f, false);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x00070E70 File Offset: 0x0006F070
		public void ImmunityTick(Pawn pawn, bool sick, Hediff diseaseInstance)
		{
			this.immunity += this.ImmunityChangePerTick(pawn, sick, diseaseInstance);
			this.immunity = Mathf.Clamp01(this.immunity);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00070E9C File Offset: 0x0006F09C
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

		// Token: 0x04000E84 RID: 3716
		public HediffDef hediffDef;

		// Token: 0x04000E85 RID: 3717
		public HediffDef source;

		// Token: 0x04000E86 RID: 3718
		public float immunity;
	}
}
