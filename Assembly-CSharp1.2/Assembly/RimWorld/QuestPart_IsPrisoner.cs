using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001073 RID: 4211
	public class QuestPart_IsPrisoner : QuestPartActivable
	{
		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06005B9F RID: 23455 RVA: 0x0003F875 File Offset: 0x0003DA75
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

		// Token: 0x06005BA0 RID: 23456 RVA: 0x0003F885 File Offset: 0x0003DA85
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.IsPrisoner)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x0003F8B8 File Offset: 0x0003DAB8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x06005BA2 RID: 23458 RVA: 0x0003F8D1 File Offset: 0x0003DAD1
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06005BA3 RID: 23459 RVA: 0x0003F8FA File Offset: 0x0003DAFA
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04003D7C RID: 15740
		public Pawn pawn;
	}
}
