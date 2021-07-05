using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020007A7 RID: 1959
	public class DiaNodeList
	{
		// Token: 0x0600314E RID: 12622 RVA: 0x00145674 File Offset: 0x00143874
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

		// Token: 0x04002210 RID: 8720
		public string Name = "NeedsName";

		// Token: 0x04002211 RID: 8721
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04002212 RID: 8722
		public List<string> NodeNames = new List<string>();
	}
}
