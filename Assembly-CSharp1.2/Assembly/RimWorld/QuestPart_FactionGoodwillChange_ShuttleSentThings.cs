using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113C RID: 4412
	public class QuestPart_FactionGoodwillChange_ShuttleSentThings : QuestPartActivable
	{
		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x060060B4 RID: 24756 RVA: 0x00042B10 File Offset: 0x00040D10
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

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x060060B5 RID: 24757 RVA: 0x00042B20 File Offset: 0x00040D20
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

		// Token: 0x060060B6 RID: 24758 RVA: 0x001E594C File Offset: 0x001E3B4C
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

		// Token: 0x060060B7 RID: 24759 RVA: 0x00042B30 File Offset: 0x00040D30
		public override void Cleanup()
		{
			if (base.State == QuestPartState.Enabled)
			{
				this.TryAffectGoodwill(this.things.Count * this.changeNotOnShuttle);
			}
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x001E5A00 File Offset: 0x001E3C00
		private void TryAffectGoodwill(int goodwillChange)
		{
			if (goodwillChange != 0)
			{
				this.faction.TryAffectGoodwillWith(Faction.OfPlayer, goodwillChange, this.canSendMessage, this.canSendHostilityLetter, this.reason, null);
			}
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x001E5A40 File Offset: 0x001E3C40
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
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400409F RID: 16543
		public List<string> inSignalsShuttleSent = new List<string>();

		// Token: 0x040040A0 RID: 16544
		public string inSignalShuttleDestroyed;

		// Token: 0x040040A1 RID: 16545
		public int changeNotOnShuttle;

		// Token: 0x040040A2 RID: 16546
		public Faction faction;

		// Token: 0x040040A3 RID: 16547
		public bool canSendMessage = true;

		// Token: 0x040040A4 RID: 16548
		public bool canSendHostilityLetter = true;

		// Token: 0x040040A5 RID: 16549
		public string reason;

		// Token: 0x040040A6 RID: 16550
		public List<Thing> things = new List<Thing>();
	}
}
