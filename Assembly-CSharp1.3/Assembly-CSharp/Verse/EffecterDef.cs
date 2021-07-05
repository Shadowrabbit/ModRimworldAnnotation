using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C3 RID: 195
	public class EffecterDef : Def
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x0001E217 File Offset: 0x0001C417
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001E220 File Offset: 0x0001C420
		public Effecter Spawn(IntVec3 target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.Trigger(targetInfo, targetInfo);
			return effecter;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001E24C File Offset: 0x0001C44C
		public Effecter Spawn(IntVec3 target, Map map, Vector3 offset, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			TargetInfo targetInfo = new TargetInfo(target, map, false);
			effecter.scale = scale;
			effecter.offset = offset;
			effecter.Trigger(targetInfo, targetInfo);
			return effecter;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001E280 File Offset: 0x0001C480
		public Effecter Spawn(Thing target, Map map, float scale = 1f)
		{
			Effecter effecter = new Effecter(this);
			effecter.offset = target.TrueCenter() - target.Position.ToVector3Shifted();
			effecter.scale = scale;
			TargetInfo targetInfo = new TargetInfo(target.Position, map, false);
			effecter.Trigger(targetInfo, targetInfo);
			return effecter;
		}

		// Token: 0x040003F5 RID: 1013
		public List<SubEffecterDef> children;

		// Token: 0x040003F6 RID: 1014
		public float positionRadius;

		// Token: 0x040003F7 RID: 1015
		public FloatRange offsetTowardsTarget;
	}
}
