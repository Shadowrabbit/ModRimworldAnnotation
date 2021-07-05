using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A4F RID: 2639
	public class PlantBiomeRecord
	{
		// Token: 0x06003F9A RID: 16282 RVA: 0x00159584 File Offset: 0x00157784
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04002327 RID: 8999
		public BiomeDef biome;

		// Token: 0x04002328 RID: 9000
		public float commonality;
	}
}
