using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BBF RID: 3007
	public class QuestPart_FactionGoodwillChange_ShuttleSentThings : QuestPartActivable
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x0600465A RID: 18010 RVA: 0x00173DC7 File Offset: 0x00171FC7
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

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x0600465B RID: 18011 RVA: 0x00173DD7 File Offset: 0x00171FD7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Thing t in this.things)
				{
					yield return t;
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00173DE8 File Offset: 0x00171FE8
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			List<Thing> list;
			if (this.inSignalsShuttleSent.Contains(signal.tag) && signal.args.TryGetArg<List<Thing>>("SENT", out list))
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					if (!list.Contains(this.things[i]))
					{
						num++;
					}
				}
				this.TryAffectGoodwill(num * this.changeNotOnShuttle);
				base.Complete();
			}
			if (signal.tag == this.inSignalShuttleDestroyed)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
				base.Complete();
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00173E99 File Offset: 0x00172099
		public override void Cleanup()
		{
			if (base.State == QuestPartState.Enabled)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00173EBC File Offset: 0x001720BC
		private void TryAffectGoodwill(int goodwillChange)
		{
			if (goodwillChange != 0)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, goodwillChange, this.canSendMessage, this.canSendHostilityLetter, this.historyEvent, null);
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00173EFC File Offset: 0x001720FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignalsShuttleSent, "inSignalsShuttleSent", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.changeNotOnShuttle, "changeNotOnShuttle", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.canSendMessage, "canSendMessage", true, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", true, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalShuttleDestroyed, "inSignalShuttleDestroyed", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04002ADC RID: 10972
		public List<string> inSignalsShuttleSent = new List<string>();

		// Token: 0x04002ADD RID: 10973
		public string inSignalShuttleDestroyed;

		// Token: 0x04002ADE RID: 10974
		public int changeNotOnShuttle;

		// Token: 0x04002ADF RID: 10975
		public Faction faction;

		// Token: 0x04002AE0 RID: 10976
		public bool canSendMessage = true;

		// Token: 0x04002AE1 RID: 10977
		public bool canSendHostilityLetter = true;

		// Token: 0x04002AE2 RID: 10978
		public string reason;

		// Token: 0x04002AE3 RID: 10979
		public List<Thing> things = new List<Thing>();

		// Token: 0x04002AE4 RID: 10980
		public HistoryEventDef historyEvent;
	}
}
