using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200219D RID: 8605
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x0600B7C9 RID: 47049 RVA: 0x000772FA File Offset: 0x000754FA
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600B7CA RID: 47050 RVA: 0x0007730E File Offset: 0x0007550E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x0600B7CB RID: 47051 RVA: 0x00077330 File Offset: 0x00075530
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600B7CC RID: 47052 RVA: 0x0007733E File Offset: 0x0007553E
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x00077346 File Offset: 0x00075546
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x04007DA9 RID: 32169
		public ThingOwner contents;
	}
}
