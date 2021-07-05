using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001161 RID: 4449
	public class CompProperties_MoteEmitter : CompProperties
	{
		// Token: 0x06006AE7 RID: 27367 RVA: 0x0023E2DB File Offset: 0x0023C4DB
		public CompProperties_MoteEmitter()
		{
			this.compClass = typeof(CompMoteEmitter);
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06006AE8 RID: 27368 RVA: 0x0023E310 File Offset: 0x0023C510
		public Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.offsetMin.x, this.offsetMax.x), Rand.Range(this.offsetMin.y, this.offsetMax.y), Rand.Range(this.offsetMin.z, this.offsetMax.z));
			}
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x0023E373 File Offset: 0x0023C573
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.mote == null)
			{
				yield return "CompMoteEmitter must have a mote assigned.";
			}
			yield break;
		}

		// Token: 0x04003B67 RID: 15207
		public ThingDef mote;

		// Token: 0x04003B68 RID: 15208
		public Vector3 offset;

		// Token: 0x04003B69 RID: 15209
		public Vector3 offsetMin = Vector3.zero;

		// Token: 0x04003B6A RID: 15210
		public Vector3 offsetMax = Vector3.zero;

		// Token: 0x04003B6B RID: 15211
		public SoundDef soundOnEmission;

		// Token: 0x04003B6C RID: 15212
		public int emissionInterval = -1;

		// Token: 0x04003B6D RID: 15213
		public int ticksSinceLastEmittedMaxOffset;

		// Token: 0x04003B6E RID: 15214
		public bool maintain;

		// Token: 0x04003B6F RID: 15215
		public string saveKeysPrefix;
	}
}
