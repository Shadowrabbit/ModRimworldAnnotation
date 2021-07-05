using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAA RID: 2986
	public class QuestPart_SituationalThought : QuestPartActivable
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060045A8 RID: 17832 RVA: 0x00170FAC File Offset: 0x0016F1AC
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

		// Token: 0x060045A9 RID: 17833 RVA: 0x00170FBC File Offset: 0x0016F1BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.stage, "stage", 0, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x00171014 File Offset: 0x0016F214
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThoughtDefOf.DecreeUnmet;
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x00171037 File Offset: 0x0016F237
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04002A69 RID: 10857
		public ThoughtDef def;

		// Token: 0x04002A6A RID: 10858
		public Pawn pawn;

		// Token: 0x04002A6B RID: 10859
		public int stage;

		// Token: 0x04002A6C RID: 10860
		public int delayTicks;
	}
}
