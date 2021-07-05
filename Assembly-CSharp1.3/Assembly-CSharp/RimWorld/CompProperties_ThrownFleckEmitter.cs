using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BE RID: 4542
	public class CompProperties_ThrownFleckEmitter : CompProperties
	{
		// Token: 0x06006D66 RID: 28006 RVA: 0x0024A73D File Offset: 0x0024893D
		public CompProperties_ThrownFleckEmitter()
		{
			this.compClass = typeof(CompThrownFleckEmitter);
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x0024A779 File Offset: 0x00248979
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.fleck == null)
			{
				yield return "CompThrownFleckEmitter must have a fleck assigned.";
			}
			yield break;
		}

		// Token: 0x04003CB9 RID: 15545
		public FleckDef fleck;

		// Token: 0x04003CBA RID: 15546
		public Vector3 offsetMin;

		// Token: 0x04003CBB RID: 15547
		public Vector3 offsetMax;

		// Token: 0x04003CBC RID: 15548
		public int emissionInterval = -1;

		// Token: 0x04003CBD RID: 15549
		public int burstCount = 1;

		// Token: 0x04003CBE RID: 15550
		public Color colorA = Color.white;

		// Token: 0x04003CBF RID: 15551
		public Color colorB = Color.white;

		// Token: 0x04003CC0 RID: 15552
		public FloatRange scale;

		// Token: 0x04003CC1 RID: 15553
		public FloatRange rotationRate;

		// Token: 0x04003CC2 RID: 15554
		public FloatRange velocityX;

		// Token: 0x04003CC3 RID: 15555
		public FloatRange velocityY;
	}
}
