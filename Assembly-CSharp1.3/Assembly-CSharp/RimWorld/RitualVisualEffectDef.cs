using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB0 RID: 4016
	public class RitualVisualEffectDef : Def
	{
		// Token: 0x06005ED0 RID: 24272 RVA: 0x00207532 File Offset: 0x00205732
		public RitualVisualEffect GetInstance()
		{
			RitualVisualEffect ritualVisualEffect = (RitualVisualEffect)Activator.CreateInstance(this.workerClass);
			ritualVisualEffect.def = this;
			return ritualVisualEffect;
		}

		// Token: 0x04003699 RID: 13977
		public Type workerClass = typeof(RitualVisualEffect);

		// Token: 0x0400369A RID: 13978
		public List<CompProperties_RitualVisualEffect> comps;

		// Token: 0x0400369B RID: 13979
		public Color tintColor = Color.white;
	}
}
