using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A50 RID: 2640
	public class BiomeAnimalRecord
	{
		// Token: 0x06003F9C RID: 16284 RVA: 0x001595AA File Offset: 0x001577AA
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04002329 RID: 9001
		public PawnKindDef animal;

		// Token: 0x0400232A RID: 9002
		public float commonality;
	}
}
