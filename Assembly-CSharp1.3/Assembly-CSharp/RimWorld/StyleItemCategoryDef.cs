using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ACD RID: 2765
	public class StyleItemCategoryDef : Def
	{
		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600414B RID: 16715 RVA: 0x0015F2B5 File Offset: 0x0015D4B5
		public List<StyleItemDef> ItemsInCategory
		{
			get
			{
				if (this.cachedStyleItems == null)
				{
					this.cachedStyleItems = new List<StyleItemDef>();
					this.cachedStyleItems.AddRange(from x in StyleItemDef.AllStyleItemDefs
					where x.StyleItemCategory == this
					select x);
				}
				return this.cachedStyleItems;
			}
		}

		// Token: 0x04002720 RID: 10016
		private List<StyleItemDef> cachedStyleItems;
	}
}
