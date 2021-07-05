using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA6 RID: 2726
	public class PreceptThingChanceClass
	{
		// Token: 0x060040C9 RID: 16585 RVA: 0x0015DE14 File Offset: 0x0015C014
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040025B6 RID: 9654
		public ThingDef def;

		// Token: 0x040025B7 RID: 9655
		public float chance;
	}
}
