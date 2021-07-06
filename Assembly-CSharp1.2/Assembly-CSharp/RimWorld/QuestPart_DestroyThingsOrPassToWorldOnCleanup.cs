using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B5 RID: 4277
	public class QuestPart_DestroyThingsOrPassToWorldOnCleanup : QuestPart
	{
		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005D46 RID: 23878 RVA: 0x00040B18 File Offset: 0x0003ED18
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

		// Token: 0x06005D47 RID: 23879 RVA: 0x00040B28 File Offset: 0x0003ED28
		public override void Cleanup()
		{
			base.Cleanup();
			QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x001DC650 File Offset: 0x001DA850
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04003E68 RID: 15976
		public List<Thing> things = new List<Thing>();

		// Token: 0x04003E69 RID: 15977
		public bool questLookTargets = true;
	}
}
