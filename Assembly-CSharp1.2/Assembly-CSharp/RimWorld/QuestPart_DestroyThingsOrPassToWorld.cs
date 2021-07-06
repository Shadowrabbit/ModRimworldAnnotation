using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B2 RID: 4274
	public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
	{
		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06005D34 RID: 23860 RVA: 0x00040A84 File Offset: 0x0003EC84
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

		// Token: 0x06005D35 RID: 23861 RVA: 0x00040A94 File Offset: 0x0003EC94
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
			}
		}

		// Token: 0x06005D36 RID: 23862 RVA: 0x001DC434 File Offset: 0x001DA634
		public static void Destroy(List<Thing> things)
		{
			for (int i = things.Count - 1; i >= 0; i--)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(things[i]);
			}
		}

		// Token: 0x06005D37 RID: 23863 RVA: 0x001DC460 File Offset: 0x001DA660
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

		// Token: 0x06005D38 RID: 23864 RVA: 0x001DC49C File Offset: 0x001DA69C
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

		// Token: 0x06005D39 RID: 23865 RVA: 0x001DC51C File Offset: 0x001DA71C
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

		// Token: 0x04003E5E RID: 15966
		public string inSignal;

		// Token: 0x04003E5F RID: 15967
		public List<Thing> things = new List<Thing>();

		// Token: 0x04003E60 RID: 15968
		public bool questLookTargets = true;
	}
}
