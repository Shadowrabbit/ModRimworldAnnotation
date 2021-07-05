using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1F RID: 3615
	public class TraitEntry
	{
		// Token: 0x0600537F RID: 21375 RVA: 0x000033AC File Offset: 0x000015AC
		public TraitEntry()
		{
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x001C4764 File Offset: 0x001C2964
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x001C477A File Offset: 0x001C297A
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.def = DefDatabase<TraitDef>.GetNamed(xmlRoot.Name, true);
			if (xmlRoot.HasChildNodes)
			{
				this.degree = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
				return;
			}
			this.degree = 0;
		}

		// Token: 0x04003103 RID: 12547
		public TraitDef def;

		// Token: 0x04003104 RID: 12548
		public int degree;
	}
}
