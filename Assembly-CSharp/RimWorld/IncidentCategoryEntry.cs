using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001233 RID: 4659
	public class IncidentCategoryEntry
	{
		// Token: 0x060065CD RID: 26061 RVA: 0x0004598B File Offset: 0x00043B8B
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040043C6 RID: 17350
		public IncidentCategoryDef category;

		// Token: 0x040043C7 RID: 17351
		public float weight;
	}
}
