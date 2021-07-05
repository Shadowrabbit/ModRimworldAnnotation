using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBC RID: 3004
	public class QuestPart_ChangeHeir : QuestPart
	{
		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x00172EC4 File Offset: 0x001710C4
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.holder;
				yield return this.heir;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600463D RID: 17981 RVA: 0x00172ED4 File Offset: 0x001710D4
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x00172EE4 File Offset: 0x001710E4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null)
			{
				this.holder.royalty.SetHeir(this.heir, this.faction);
				this.done = true;
			}
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x00172F36 File Offset: 0x00171136
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.holder == replace)
			{
				this.holder = with;
			}
			if (this.heir == replace)
			{
				this.heir = with;
			}
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x00172F58 File Offset: 0x00171158
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.holder, "holder", false);
			Scribe_References.Look<Pawn>(ref this.heir, "heir", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
		}

		// Token: 0x04002ABB RID: 10939
		public Faction faction;

		// Token: 0x04002ABC RID: 10940
		public Pawn holder;

		// Token: 0x04002ABD RID: 10941
		public Pawn heir;

		// Token: 0x04002ABE RID: 10942
		public string inSignal;

		// Token: 0x04002ABF RID: 10943
		public bool done;
	}
}
