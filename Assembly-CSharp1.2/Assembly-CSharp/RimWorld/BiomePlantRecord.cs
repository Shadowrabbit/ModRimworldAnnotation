using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F74 RID: 3956
	public class BiomePlantRecord
	{
		// Token: 0x060056D9 RID: 22233 RVA: 0x0003C3D4 File Offset: 0x0003A5D4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003828 RID: 14376
		public ThingDef plant;

		// Token: 0x04003829 RID: 14377
		public float commonality;
	}
}
