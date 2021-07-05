using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187D RID: 6269
	public class CompProperties_ThrownMoteEmitter : CompProperties
	{
		// Token: 0x06008B10 RID: 35600 RVA: 0x0005D4A2 File Offset: 0x0005B6A2
		public CompProperties_ThrownMoteEmitter()
		{
			this.compClass = typeof(CompThrownMoteEmitter);
		}

		// Token: 0x06008B11 RID: 35601 RVA: 0x0005D4DE File Offset: 0x0005B6DE
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompThrownMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x04005921 RID: 22817
		public ThingDef mote;

		// Token: 0x04005922 RID: 22818
		public Vector3 offsetMin;

		// Token: 0x04005923 RID: 22819
		public Vector3 offsetMax;

		// Token: 0x04005924 RID: 22820
		public int emissionInterval = -1;

		// Token: 0x04005925 RID: 22821
		public int burstCount = 1;

		// Token: 0x04005926 RID: 22822
		public Color colorA = Color.white;

		// Token: 0x04005927 RID: 22823
		public Color colorB = Color.white;

		// Token: 0x04005928 RID: 22824
		public FloatRange scale;

		// Token: 0x04005929 RID: 22825
		public FloatRange rotationRate;

		// Token: 0x0400592A RID: 22826
		public FloatRange velocityX;

		// Token: 0x0400592B RID: 22827
		public FloatRange velocityY;
	}
}
