using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B60 RID: 2912
	public class QuestPart_AddMemoryThought : QuestPart
	{
		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x00169F40 File Offset: 0x00168140
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null && this.addToLookTargets)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x00169F50 File Offset: 0x00168150
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def, this.otherPawn, null);
			}
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x00169FB8 File Offset: 0x001681B8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0016A021 File Offset: 0x00168221
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0016A04D File Offset: 0x0016824D
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
			if (this.otherPawn == replace)
			{
				this.otherPawn = with;
			}
		}

		// Token: 0x04002953 RID: 10579
		public string inSignal;

		// Token: 0x04002954 RID: 10580
		public ThoughtDef def;

		// Token: 0x04002955 RID: 10581
		public Pawn pawn;

		// Token: 0x04002956 RID: 10582
		public Pawn otherPawn;

		// Token: 0x04002957 RID: 10583
		public bool addToLookTargets = true;
	}
}
