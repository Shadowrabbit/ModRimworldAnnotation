using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001340 RID: 4928
	public class ITab_ContentsCasket : ITab_ContentsBase
	{
		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x0600774D RID: 30541 RVA: 0x0029E498 File Offset: 0x0029C698
		public override IList<Thing> container
		{
			get
			{
				Building_Casket building_Casket = base.SelThing as Building_Casket;
				this.listInt.Clear();
				if (building_Casket != null && building_Casket.ContainedThing != null)
				{
					this.listInt.Add(building_Casket.ContainedThing);
				}
				return this.listInt;
			}
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x0029E4DE File Offset: 0x0029C6DE
		public ITab_ContentsCasket()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "ContainedItems";
			this.canRemoveThings = false;
		}

		// Token: 0x04004254 RID: 16980
		private List<Thing> listInt = new List<Thing>();
	}
}
