using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA7 RID: 2983
	public class QuestPart_SetFaction : QuestPart
	{
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x00170CC5 File Offset: 0x0016EEC5
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

		// Token: 0x0600459B RID: 17819 RVA: 0x00170CD8 File Offset: 0x0016EED8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.things.Count; i++)
				{
					if (this.things[i].Faction != this.faction)
					{
						this.things[i].SetFaction(this.faction, null);
					}
				}
			}
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00170D48 File Offset: 0x0016EF48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04002A5E RID: 10846
		public string inSignal;

		// Token: 0x04002A5F RID: 10847
		public Faction faction;

		// Token: 0x04002A60 RID: 10848
		public List<Thing> things = new List<Thing>();
	}
}
