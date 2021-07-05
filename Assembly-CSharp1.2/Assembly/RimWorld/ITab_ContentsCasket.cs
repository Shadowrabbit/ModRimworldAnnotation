using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B00 RID: 6912
	public class ITab_ContentsCasket : ITab_ContentsBase
	{
		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x0600982A RID: 38954 RVA: 0x002CAB34 File Offset: 0x002C8D34
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

		// Token: 0x0600982B RID: 38955 RVA: 0x0006562C File Offset: 0x0006382C
		public ITab_ContentsCasket()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "ContainedItems";
			this.canRemoveThings = false;
		}

		// Token: 0x0400613F RID: 24895
		private List<Thing> listInt = new List<Thing>();
	}
}
