using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA5 RID: 4005
	public class MTBByBiome
	{
		// Token: 0x060057A8 RID: 22440 RVA: 0x001CDC3C File Offset: 0x001CBE3C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name, null, null);
			this.mtbDays = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003961 RID: 14689
		public BiomeDef biome;

		// Token: 0x04003962 RID: 14690
		public float mtbDays;
	}
}
