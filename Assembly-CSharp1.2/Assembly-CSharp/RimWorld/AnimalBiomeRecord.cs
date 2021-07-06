using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F77 RID: 3959
	public class AnimalBiomeRecord
	{
		// Token: 0x060056DF RID: 22239 RVA: 0x0003C446 File Offset: 0x0003A646
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400382E RID: 14382
		public BiomeDef biome;

		// Token: 0x0400382F RID: 14383
		public float commonality;
	}
}
