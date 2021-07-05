using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A64 RID: 2660
	public class MemeWeight
	{
		// Token: 0x06003FDE RID: 16350 RVA: 0x0015A558 File Offset: 0x00158758
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "meme", xmlRoot.Name, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040023D3 RID: 9171
		public MemeDef meme;

		// Token: 0x040023D4 RID: 9172
		public float selectionWeight;
	}
}
