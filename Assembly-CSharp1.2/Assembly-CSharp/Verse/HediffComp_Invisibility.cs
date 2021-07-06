using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E5 RID: 997
	public class HediffComp_Invisibility : HediffComp
	{
		// Token: 0x06001871 RID: 6257 RVA: 0x00017331 File Offset: 0x00015531
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.UpdateTarget();
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00017340 File Offset: 0x00015540
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000DF6A0 File Offset: 0x000DD8A0
		private void UpdateTarget()
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			PortraitsCache.SetDirty(pawn);
		}
	}
}
