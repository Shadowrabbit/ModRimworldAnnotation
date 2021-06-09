using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F76 RID: 3958
	public class BiomeAnimalRecord
	{
		// Token: 0x060056DD RID: 22237 RVA: 0x0003C420 File Offset: 0x0003A620
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400382C RID: 14380
		public PawnKindDef animal;

		// Token: 0x0400382D RID: 14381
		public float commonality;
	}
}
