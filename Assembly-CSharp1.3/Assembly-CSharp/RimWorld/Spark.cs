using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C5 RID: 4293
	public class Spark : Projectile
	{
		// Token: 0x060066BC RID: 26300 RVA: 0x0022B4C8 File Offset: 0x002296C8
		protected override void Impact(Thing hitThing)
		{
			Map map = base.Map;
			base.Impact(hitThing);
			FireUtility.TryStartFireIn(base.Position, map, 0.1f);
		}
	}
}
