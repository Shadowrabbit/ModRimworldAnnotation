using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000453 RID: 1107
	public class DiaNodeList
	{
		// Token: 0x06002186 RID: 8582 RVA: 0x000D17A4 File Offset: 0x000CF9A4
		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.NodeNames)
			{
				list.Add(DialogDatabase.GetNodeNamed(nodeName));
			}
			foreach (DiaNodeMold diaNodeMold in list)
			{
				if (diaNodeMold.unique && diaNodeMold.used)
				{
					list.Remove(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x040014E6 RID: 5350
		public string Name = "NeedsName";

		// Token: 0x040014E7 RID: 5351
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x040014E8 RID: 5352
		public List<string> NodeNames = new List<string>();
	}
}
