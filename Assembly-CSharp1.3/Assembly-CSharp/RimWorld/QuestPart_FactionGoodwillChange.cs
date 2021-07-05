using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBE RID: 3006
	[StaticConstructorOnStartup]
	public class QuestPart_FactionGoodwillChange : QuestPart
	{
		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x00173B46 File Offset: 0x00171D46
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.lookTarget;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x00173B56 File Offset: 0x00171D56
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

		// Token: 0x06004654 RID: 18004 RVA: 0x00173B68 File Offset: 0x00171D68
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				if (this.lookTarget.IsValid)
				{
					GlobalTargetInfo globalTargetInfo = this.lookTarget;
				}
				else if (this.getLookTargetFromSignal)
				{
					LookTargets lookTargets;
					if (SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
					{
						lookTargets.TryGetPrimaryTarget();
					}
					else
					{
						GlobalTargetInfo invalid = GlobalTargetInfo.Invalid;
					}
				}
				else
				{
					GlobalTargetInfo invalid2 = GlobalTargetInfo.Invalid;
				}
				FactionRelationKind playerRelationKind = this.faction.PlayerRelationKind;
				int num = 0;
				if (!signal.args.TryGetArg<int>("GOODWILL", out num))
				{
					num = this.change;
				}
				if (this.ensureMakesHostile)
				{
					num = Mathf.Min(num, Faction.OfPlayer.GoodwillToMakeHostile(this.faction));
				}
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, num, this.canSendMessage, this.canSendHostilityLetter, (num >= 0) ? (this.historyEvent ?? HistoryEventDefOf.QuestGoodwillReward) : this.historyEvent, null);
				TaggedString t = "";
				this.faction.TryAppendRelationKindChangedInfo(ref t, playerRelationKind, this.faction.PlayerRelationKind, null);
				if (!t.NullOrEmpty())
				{
					t = "\n\n" + t;
				}
			}
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x00173CBC File Offset: 0x00171EBC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.change, "change", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetFromSignal, "getLookTargetFromSignal", true, false);
			Scribe_TargetInfo.Look(ref this.lookTarget, "lookTarget");
			Scribe_Values.Look<bool>(ref this.ensureMakesHostile, "ensureMakesHostile", false, false);
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00173D6C File Offset: 0x00171F6C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.change = -15;
			this.faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
		}

		// Token: 0x04002AD3 RID: 10963
		public HistoryEventDef historyEvent;

		// Token: 0x04002AD4 RID: 10964
		public string inSignal;

		// Token: 0x04002AD5 RID: 10965
		public int change;

		// Token: 0x04002AD6 RID: 10966
		public Faction faction;

		// Token: 0x04002AD7 RID: 10967
		public bool canSendMessage = true;

		// Token: 0x04002AD8 RID: 10968
		public bool canSendHostilityLetter = true;

		// Token: 0x04002AD9 RID: 10969
		public bool getLookTargetFromSignal = true;

		// Token: 0x04002ADA RID: 10970
		public GlobalTargetInfo lookTarget;

		// Token: 0x04002ADB RID: 10971
		public bool ensureMakesHostile;
	}
}
