using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB2 RID: 4018
	public class CompProperties_RitualVisualEffect
	{
		// Token: 0x06005ED9 RID: 24281 RVA: 0x002077F0 File Offset: 0x002059F0
		public CompProperties_RitualVisualEffect()
		{
		}

		// Token: 0x06005EDA RID: 24282 RVA: 0x0020786C File Offset: 0x00205A6C
		public CompProperties_RitualVisualEffect(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06005EDB RID: 24283 RVA: 0x002078EE File Offset: 0x00205AEE
		public RitualVisualEffectComp GetInstance()
		{
			return (RitualVisualEffectComp)Activator.CreateInstance(this.compClass);
		}

		// Token: 0x040036A1 RID: 13985
		public Type compClass = typeof(RitualVisualEffectComp);

		// Token: 0x040036A2 RID: 13986
		public ThingDef moteDef;

		// Token: 0x040036A3 RID: 13987
		public FleckDef fleckDef;

		// Token: 0x040036A4 RID: 13988
		public EffecterDef effecterDef;

		// Token: 0x040036A5 RID: 13989
		public FloatRange velocity = FloatRange.Zero;

		// Token: 0x040036A6 RID: 13990
		public Vector3 velocityDir = Vector3.zero;

		// Token: 0x040036A7 RID: 13991
		public FloatRange rotation = FloatRange.Zero;

		// Token: 0x040036A8 RID: 13992
		public FloatRange rotationRate = FloatRange.Zero;

		// Token: 0x040036A9 RID: 13993
		public FloatRange scale = FloatRange.One;

		// Token: 0x040036AA RID: 13994
		public Vector3 offset = Vector3.zero;

		// Token: 0x040036AB RID: 13995
		public IntVec3 roomCheckOffset = IntVec3.Zero;

		// Token: 0x040036AC RID: 13996
		public bool scaleWithRoom;

		// Token: 0x040036AD RID: 13997
		public bool scalePositionWithRoom;

		// Token: 0x040036AE RID: 13998
		public bool onlySpawnInSameRoom;

		// Token: 0x040036AF RID: 13999
		public Color? colorOverride;

		// Token: 0x040036B0 RID: 14000
		public List<Color> overrideColors = new List<Color>();
	}
}
