using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001139 RID: 4409
	[StaticConstructorOnStartup]
	public class QuestPart_FactionGoodwillChange : QuestPart
	{
		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x0600609A RID: 24730 RVA: 0x000429FC File Offset: 0x00040BFC
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

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x0600609B RID: 24731 RVA: 0x00042A0C File Offset: 0x00040C0C
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

		// Token: 0x0600609C RID: 24732 RVA: 0x001E54D8 File Offset: 0x001E36D8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				GlobalTargetInfo value;
				if (this.lookTarget.IsValid)
				{
					value = this.lookTarget;
				}
				else if (this.getLookTargetFromSignal)
				{
					LookTargets lookTargets;
					if (SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
					{
						value = lookTargets.TryGetPrimaryTarget();
					}
					else
					{
						value = GlobalTargetInfo.Invalid;
					}
				}
				else
				{
					value = GlobalTargetInfo.Invalid;
				}
				FactionRelationKind playerRelationKind = this.faction.PlayerRelationKind;
				int goodwillChange = 0;
				if (!signal.args.TryGetArg<int>("GOODWILL", out goodwillChange))
				{
					goodwillChange = this.change;
				}
				this.faction.TryAffectGoodwillWith(Faction.OfPlayer, goodwillChange, this.canSendMessage, this.canSendHostilityLetter, signal.args.GetFormattedText(this.reason), new GlobalTargetInfo?(value));
				TaggedString t = "";
				this.faction.TryAppendRelationKindChangedInfo(ref t, playerRelationKind, this.faction.PlayerRelationKind, null);
				if (!t.NullOrEmpty())
				{
					t = "\n\n" + t;
				}
			}
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x001E560C File Offset: 0x001E380C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.change, "change", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetFromSignal, "getLookTargetFromSignal", true, false);
			Scribe_TargetInfo.Look(ref this.lookTarget, "lookTarget");
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x00042A1C File Offset: 0x00040C1C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.change = -15;
			this.faction = Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined);
		}

		// Token: 0x0400408D RID: 16525
		public string inSignal;

		// Token: 0x0400408E RID: 16526
		public int change;

		// Token: 0x0400408F RID: 16527
		public Faction faction;

		// Token: 0x04004090 RID: 16528
		public bool canSendMessage = true;

		// Token: 0x04004091 RID: 16529
		public bool canSendHostilityLetter = true;

		// Token: 0x04004092 RID: 16530
		public string reason;

		// Token: 0x04004093 RID: 16531
		public bool getLookTargetFromSignal = true;

		// Token: 0x04004094 RID: 16532
		public GlobalTargetInfo lookTarget;
	}
}
