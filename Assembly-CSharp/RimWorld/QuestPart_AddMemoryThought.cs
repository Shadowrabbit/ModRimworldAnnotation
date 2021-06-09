using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109C RID: 4252
	public class QuestPart_AddMemoryThought : QuestPart
	{
		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06005CB6 RID: 23734 RVA: 0x00040506 File Offset: 0x0003E706
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

		// Token: 0x06005CB7 RID: 23735 RVA: 0x001DB234 File Offset: 0x001D9434
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def, this.otherPawn);
			}
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x001DB29C File Offset: 0x001D949C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x00040516 File Offset: 0x0003E716
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x00040542 File Offset: 0x0003E742
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

		// Token: 0x04003E13 RID: 15891
		public string inSignal;

		// Token: 0x04003E14 RID: 15892
		public ThoughtDef def;

		// Token: 0x04003E15 RID: 15893
		public Pawn pawn;

		// Token: 0x04003E16 RID: 15894
		public Pawn otherPawn;

		// Token: 0x04003E17 RID: 15895
		public bool addToLookTargets = true;
	}
}
