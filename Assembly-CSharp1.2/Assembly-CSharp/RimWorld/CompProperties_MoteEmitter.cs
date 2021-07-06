using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F7 RID: 6135
	public class CompProperties_MoteEmitter : CompProperties
	{
		// Token: 0x060087C9 RID: 34761 RVA: 0x0005B192 File Offset: 0x00059392
		public CompProperties_MoteEmitter()
		{
			this.compClass = typeof(CompMoteEmitter);
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x0005B1B1 File Offset: 0x000593B1
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x04005710 RID: 22288
		public ThingDef mote;

		// Token: 0x04005711 RID: 22289
		public Vector3 offset;

		// Token: 0x04005712 RID: 22290
		public int emissionInterval = -1;

		// Token: 0x04005713 RID: 22291
		public SoundDef soundOnEmission;

		// Token: 0x04005714 RID: 22292
		public bool maintain;

		// Token: 0x04005715 RID: 22293
		public string saveKeysPrefix;
	}
}
