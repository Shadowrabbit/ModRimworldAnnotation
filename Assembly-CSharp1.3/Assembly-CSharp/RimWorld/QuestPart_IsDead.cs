using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B46 RID: 2886
	public class QuestPart_IsDead : QuestPartActivable
	{
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06004380 RID: 17280 RVA: 0x00167E43 File Offset: 0x00166043
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

		// Token: 0x06004381 RID: 17281 RVA: 0x00167E53 File Offset: 0x00166053
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.Destroyed)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x00167E86 File Offset: 0x00166086
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x00167E9F File Offset: 0x0016609F
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x00167EC8 File Offset: 0x001660C8
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002908 RID: 10504
		public Pawn pawn;
	}
}
