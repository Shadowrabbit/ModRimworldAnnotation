using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200012F RID: 303
	public class EffecterDef : Def
	{
		// Token: 0x06000823 RID: 2083 RVA: 0x0000C83F File Offset: 0x0000AA3F
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00094E38 File Offset: 0x00093038
		public Effecter Spawn(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.Trigger(targetInfo, targetInfo);
			return effecter;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00094E64 File Offset: 0x00093064
		public Effecter Spawn(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = target.TrueCenter() - target.Position.ToVector3Shifted();
			effecter.scale = scale;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo);
			return effecter;
		}

		// Token: 0x040005E3 RID: 1507
		public List<SubEffecterDef> children;

		// Token: 0x040005E4 RID: 1508
		public float positionRadius;

		// Token: 0x040005E5 RID: 1509
		public FloatRange offsetTowardsTarget;
	}
}
