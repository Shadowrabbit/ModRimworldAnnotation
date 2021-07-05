using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A7C RID: 2684
	public class MTBByBiome
	{
		// Token: 0x0600402F RID: 16431 RVA: 0x0015B630 File Offset: 0x00159830
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name, null, null);
			this.mtbDays = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400247D RID: 9341
		public BiomeDef biome;

		// Token: 0x0400247E RID: 9342
		public float mtbDays;
	}
}
