using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B94 RID: 2964
	public class QuestPart_RemoveMemoryThought : QuestPart
	{
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06004547 RID: 17735 RVA: 0x0016FB19 File Offset: 0x0016DD19
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

		// Token: 0x06004548 RID: 17736 RVA: 0x0016FB2C File Offset: 0x0016DD2C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				if (this.count != null)
				{
					for (int i = 0; i < this.count.Value; i++)
					{
						Thought_Memory thought_Memory = this.pawn.needs.mood.thoughts.memories.Memories.FirstOrDefault((Thought_Memory m) => this.def == m.def && (this.otherPawn == null || m.otherPawn == this.otherPawn));
						if (thought_Memory == null)
						{
							return;
						}
						this.pawn.needs.mood.thoughts.memories.RemoveMemory(thought_Memory);
					}
					return;
				}
				if (this.otherPawn == null)
				{
					this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.def);
					return;
				}
				this.pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(this.def, this.otherPawn);
			}
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0016FC48 File Offset: 0x0016DE48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Values.Look<int?>(ref this.count, "count", null, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x0016FCCB File Offset: 0x0016DECB
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x0016FCF7 File Offset: 0x0016DEF7
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

		// Token: 0x04002A3A RID: 10810
		public string inSignal;

		// Token: 0x04002A3B RID: 10811
		public ThoughtDef def;

		// Token: 0x04002A3C RID: 10812
		public Pawn pawn;

		// Token: 0x04002A3D RID: 10813
		public Pawn otherPawn;

		// Token: 0x04002A3E RID: 10814
		public int? count;

		// Token: 0x04002A3F RID: 10815
		public bool addToLookTargets = true;
	}
}
