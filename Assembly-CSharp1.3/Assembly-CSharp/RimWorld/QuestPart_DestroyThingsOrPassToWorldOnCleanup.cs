using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6E RID: 2926
	public class QuestPart_DestroyThingsOrPassToWorldOnCleanup : QuestPart
	{
		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x06004473 RID: 17523 RVA: 0x0016B46C File Offset: 0x0016966C
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

		// Token: 0x06004474 RID: 17524 RVA: 0x0016B47C File Offset: 0x0016967C
		public override void Cleanup()
		{
			base.Cleanup();
			QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0016B490 File Offset: 0x00169690
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

		// Token: 0x04002989 RID: 10633
		public List<Thing> things = new List<Thing>();

		// Token: 0x0400298A RID: 10634
		public bool questLookTargets = true;
	}
}
