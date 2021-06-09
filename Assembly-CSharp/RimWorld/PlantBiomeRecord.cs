using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F75 RID: 3957
	public class PlantBiomeRecord
	{
		// Token: 0x060056DB RID: 22235 RVA: 0x0003C3FA File Offset: 0x0003A5FA
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400382A RID: 14378
		public BiomeDef biome;

		// Token: 0x0400382B RID: 14379
		public float commonality;
	}
}
