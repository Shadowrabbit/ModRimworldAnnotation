using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001117 RID: 4375
	public class QuestPart_SituationalThought : QuestPartActivable
	{
		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06005F95 RID: 24469 RVA: 0x000421F3 File Offset: 0x000403F3
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

		// Token: 0x06005F96 RID: 24470 RVA: 0x001E274C File Offset: 0x001E094C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.stage, "stage", 0, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x00042203 File Offset: 0x00040403
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThoughtDefOf.DecreeUnmet;
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06005F98 RID: 24472 RVA: 0x00042226 File Offset: 0x00040426
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04003FE4 RID: 16356
		public ThoughtDef def;

		// Token: 0x04003FE5 RID: 16357
		public Pawn pawn;

		// Token: 0x04003FE6 RID: 16358
		public int stage;

		// Token: 0x04003FE7 RID: 16359
		public int delayTicks;
	}
}
