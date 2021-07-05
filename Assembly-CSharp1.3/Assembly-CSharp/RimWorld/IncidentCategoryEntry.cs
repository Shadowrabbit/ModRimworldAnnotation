using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C47 RID: 3143
	public class IncidentCategoryEntry
	{
		// Token: 0x060049A7 RID: 18855 RVA: 0x00185764 File Offset: 0x00183964
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04002CC0 RID: 11456
		public IncidentCategoryDef category;

		// Token: 0x04002CC1 RID: 11457
		public float weight;
	}
}
