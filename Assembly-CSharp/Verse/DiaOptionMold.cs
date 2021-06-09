using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	// Token: 0x020007A9 RID: 1961
	public class DiaOptionMold
	{
		// Token: 0x0600315A RID: 12634 RVA: 0x00145978 File Offset: 0x00143B78
		public DiaNodeMold RandomLinkNode()
		{
			List<DiaNodeMold> list = this.ChildNodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.ChildNodeNames)
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
			if (list.Count == 0)
			{
				return null;
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x0400221E RID: 8734
		public string Text = "OK".Translate();

		// Token: 0x0400221F RID: 8735
		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		// Token: 0x04002220 RID: 8736
		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();
	}
}
