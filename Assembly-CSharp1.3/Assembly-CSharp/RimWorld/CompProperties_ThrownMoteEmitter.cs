using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C0 RID: 4544
	public class CompProperties_ThrownMoteEmitter : CompProperties
	{
		// Token: 0x06006D70 RID: 28016 RVA: 0x0024AA84 File Offset: 0x00248C84
		public CompProperties_ThrownMoteEmitter()
		{
			this.compClass = typeof(CompThrownMoteEmitter);
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x0024AAD6 File Offset: 0x00248CD6
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompThrownMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x04003CC6 RID: 15558
		public ThingDef mote;

		// Token: 0x04003CC7 RID: 15559
		public Vector3 offsetMin;

		// Token: 0x04003CC8 RID: 15560
		public Vector3 offsetMax;

		// Token: 0x04003CC9 RID: 15561
		public int emissionInterval = -1;

		// Token: 0x04003CCA RID: 15562
		public int burstCount = 1;

		// Token: 0x04003CCB RID: 15563
		public Color colorA = Color.white;

		// Token: 0x04003CCC RID: 15564
		public Color colorB = Color.white;

		// Token: 0x04003CCD RID: 15565
		public FloatRange scale = FloatRange.One;

		// Token: 0x04003CCE RID: 15566
		public FloatRange rotationRate;

		// Token: 0x04003CCF RID: 15567
		public FloatRange velocityX;

		// Token: 0x04003CD0 RID: 15568
		public FloatRange velocityY;
	}
}
