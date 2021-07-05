using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001133 RID: 4403
	public class CompProperties_FleckEmitter : CompProperties
	{
		// Token: 0x060069D1 RID: 27089 RVA: 0x0023A5BB File Offset: 0x002387BB
		public CompProperties_FleckEmitter()
		{
			this.compClass = typeof(CompFleckEmitter);
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x0023A5DA File Offset: 0x002387DA
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.fleck == null)
			{
				yield return "CompFleckEmitter must have a fleck assigned.";
			}
			yield break;
		}

		// Token: 0x04003B12 RID: 15122
		public FleckDef fleck;

		// Token: 0x04003B13 RID: 15123
		public Vector3 offset;

		// Token: 0x04003B14 RID: 15124
		public int emissionInterval = -1;

		// Token: 0x04003B15 RID: 15125
		public SoundDef soundOnEmission;

		// Token: 0x04003B16 RID: 15126
		public string saveKeysPrefix;
	}
}
