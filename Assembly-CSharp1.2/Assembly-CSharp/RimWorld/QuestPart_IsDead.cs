using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001071 RID: 4209
	public class QuestPart_IsDead : QuestPartActivable
	{
		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005B8F RID: 23439 RVA: 0x0003F78B File Offset: 0x0003D98B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x0003F79B File Offset: 0x0003D99B
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.Destroyed)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x0003F7CE File Offset: 0x0003D9CE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x0003F7E7 File Offset: 0x0003D9E7
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x0003F810 File Offset: 0x0003DA10
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04003D76 RID: 15734
		public Pawn pawn;
	}
}
