using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F6 RID: 6134
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06008F1A RID: 36634 RVA: 0x00334BDD File Offset: 0x00332DDD
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06008F1B RID: 36635 RVA: 0x00334BF1 File Offset: 0x00332DF1
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06008F1C RID: 36636 RVA: 0x00334C13 File Offset: 0x00332E13
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06008F1D RID: 36637 RVA: 0x00334C21 File Offset: 0x00332E21
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06008F1E RID: 36638 RVA: 0x00334C29 File Offset: 0x00332E29
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x04005A0D RID: 23053
		public ThingOwner contents;
	}
}
