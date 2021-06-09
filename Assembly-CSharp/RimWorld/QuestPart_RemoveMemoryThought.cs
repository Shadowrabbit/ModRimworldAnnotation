using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F8 RID: 4344
	public class QuestPart_RemoveMemoryThought : QuestPart
	{
		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06005EE6 RID: 24294 RVA: 0x00041A84 File Offset: 0x0003FC84
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

		// Token: 0x06005EE7 RID: 24295 RVA: 0x001E13C0 File Offset: 0x001DF5C0
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

		// Token: 0x06005EE8 RID: 24296 RVA: 0x001E14DC File Offset: 0x001DF6DC
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

		// Token: 0x06005EE9 RID: 24297 RVA: 0x00041A94 File Offset: 0x0003FC94
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x00041AC0 File Offset: 0x0003FCC0
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

		// Token: 0x04003F8C RID: 16268
		public string inSignal;

		// Token: 0x04003F8D RID: 16269
		public ThoughtDef def;

		// Token: 0x04003F8E RID: 16270
		public Pawn pawn;

		// Token: 0x04003F8F RID: 16271
		public Pawn otherPawn;

		// Token: 0x04003F90 RID: 16272
		public int? count;

		// Token: 0x04003F91 RID: 16273
		public bool addToLookTargets = true;
	}
}
