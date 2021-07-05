using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6D RID: 2925
	public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
	{
		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x0016B2D1 File Offset: 0x001694D1
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				if (this.questLookTargets)
				{
					int num;
					for (int i = 0; i < this.things.Count; i = num + 1)
					{
						yield return this.things[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0016B2E1 File Offset: 0x001694E1
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
			}
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0016B308 File Offset: 0x00169508
		public static void Destroy(List<Thing> things)
		{
			for (int i = things.Count - 1; i >= 0; i--)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(things[i]);
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0016B334 File Offset: 0x00169534
		public static void Destroy(Thing thing)
		{
			Thing thing2;
			if (thing.ParentHolder is MinifiedThing)
			{
				thing2 = (Thing)thing.ParentHolder;
			}
			else
			{
				thing2 = thing;
			}
			if (!thing2.Destroyed)
			{
				thing2.DestroyOrPassToWorld(DestroyMode.QuestLogic);
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0016B370 File Offset: 0x00169570
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0016B3F0 File Offset: 0x001695F0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				List<Thing> source = Find.RandomPlayerHomeMap.listerThings.ThingsInGroup(ThingRequestGroup.Plant);
				this.things.Clear();
				this.things.Add(source.FirstOrDefault<Thing>());
			}
		}

		// Token: 0x04002986 RID: 10630
		public string inSignal;

		// Token: 0x04002987 RID: 10631
		public List<Thing> things = new List<Thing>();

		// Token: 0x04002988 RID: 10632
		public bool questLookTargets = true;
	}
}
