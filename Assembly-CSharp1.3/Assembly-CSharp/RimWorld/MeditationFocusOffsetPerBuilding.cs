using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200115A RID: 4442
	public class MeditationFocusOffsetPerBuilding
	{
		// Token: 0x06006ABE RID: 27326 RVA: 0x000033AC File Offset: 0x000015AC
		public MeditationFocusOffsetPerBuilding()
		{
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x0023D8EC File Offset: 0x0023BAEC
		public MeditationFocusOffsetPerBuilding(ThingDef building, float offset)
		{
			this.building = building;
			this.offset = offset;
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x0023D904 File Offset: 0x0023BB04
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.Name != "li")
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "building", xmlRoot, null, null);
				this.offset = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "building", xmlRoot.InnerText, null, null);
			this.offset = float.MinValue;
		}

		// Token: 0x04003B5A RID: 15194
		public ThingDef building;

		// Token: 0x04003B5B RID: 15195
		public float offset;
	}
}
