using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001727 RID: 5927
	public class Spark : Projectile
	{
		// Token: 0x060082BB RID: 33467 RVA: 0x0026C48C File Offset: 0x0026A68C
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
