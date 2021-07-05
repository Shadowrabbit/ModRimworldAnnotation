using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Verse
{
	// Token: 0x02000455 RID: 1109
	public class DiaOptionMold
	{
		// Token: 0x06002192 RID: 8594 RVA: 0x000D1B54 File Offset: 0x000CFD54
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

		// Token: 0x040014F4 RID: 5364
		public string Text = "OK".Translate();

		// Token: 0x040014F5 RID: 5365
		[XmlElement("Node")]
		public List<DiaNodeMold> ChildNodes = new List<DiaNodeMold>();

		// Token: 0x040014F6 RID: 5366
		[XmlElement("NodeName")]
		[DefaultValue("")]
		public List<string> ChildNodeNames = new List<string>();
	}
}
