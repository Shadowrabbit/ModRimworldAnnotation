using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A51 RID: 2641
	public class AnimalBiomeRecord
	{
		// Token: 0x06003F9E RID: 16286 RVA: 0x001595D0 File Offset: 0x001577D0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400232B RID: 9003
		public BiomeDef biome;

		// Token: 0x0400232C RID: 9004
		public float commonality;
	}
}
