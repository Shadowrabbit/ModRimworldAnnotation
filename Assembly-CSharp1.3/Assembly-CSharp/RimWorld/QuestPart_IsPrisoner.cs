using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B47 RID: 2887
	public class QuestPart_IsPrisoner : QuestPartActivable
	{
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06004387 RID: 17287 RVA: 0x00167EDA File Offset: 0x001660DA
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

		// Token: 0x06004388 RID: 17288 RVA: 0x00167EEA File Offset: 0x001660EA
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.IsPrisoner)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x00167F1D File Offset: 0x0016611D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x00167F36 File Offset: 0x00166136
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x00167F5F File Offset: 0x0016615F
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002909 RID: 10505
		public Pawn pawn;
	}
}
