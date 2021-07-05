using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B0 RID: 5296
	public class TraitEntry
	{
		// Token: 0x060071F9 RID: 29177 RVA: 0x00006B8B File Offset: 0x00004D8B
		public TraitEntry()
		{
		}

		// Token: 0x060071FA RID: 29178 RVA: 0x0004C964 File Offset: 0x0004AB64
		public TraitEntry(TraitDef def, int degree)
		{
			this.def = def;
			this.degree = degree;
		}

		// Token: 0x060071FB RID: 29179 RVA: 0x0004C97A File Offset: 0x0004AB7A
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

		// Token: 0x04004AF9 RID: 19193
		public TraitDef def;

		// Token: 0x04004AFA RID: 19194
		public int degree;
	}
}
