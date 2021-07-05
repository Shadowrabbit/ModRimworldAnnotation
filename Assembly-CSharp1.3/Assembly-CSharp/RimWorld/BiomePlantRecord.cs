using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A4E RID: 2638
	public class BiomePlantRecord
	{
		// Token: 0x06003F98 RID: 16280 RVA: 0x0015955E File Offset: 0x0015775E
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04002325 RID: 8997
		public ThingDef plant;

		// Token: 0x04002326 RID: 8998
		public float commonality;
	}
}
