using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001651 RID: 5713
	public class QuestNode_GenerateThing : QuestNode
	{
		// Token: 0x0600855B RID: 34139 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600855C RID: 34140 RVA: 0x002FE1D0 File Offset: 0x002FC3D0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(this.def.GetValue(slate), null);
			thing.stackCount = (this.stackCount.GetValue(slate) ?? 1);
			if (this.contents.GetValue(slate) != null)
			{
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null)
				{
					thingOwner.TryAddRangeOrTransfer(this.contents.GetValue(slate), true, false);
				}
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(slate, this.addToList.GetValue(slate), thing);
			}
		}

		// Token: 0x04005337 RID: 21303
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005338 RID: 21304
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04005339 RID: 21305
		public SlateRef<ThingDef> def;

		// Token: 0x0400533A RID: 21306
		public SlateRef<int?> stackCount;

		// Token: 0x0400533B RID: 21307
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
