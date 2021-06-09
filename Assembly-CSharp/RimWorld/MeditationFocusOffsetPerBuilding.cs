using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EF RID: 6127
	public class MeditationFocusOffsetPerBuilding
	{
		// Token: 0x0600879E RID: 34718 RVA: 0x00006B8B File Offset: 0x00004D8B
		public MeditationFocusOffsetPerBuilding()
		{
		}

		// Token: 0x0600879F RID: 34719 RVA: 0x0005B030 File Offset: 0x00059230
		public MeditationFocusOffsetPerBuilding(ThingDef building, float offset)
		{
			this.building = building;
			this.offset = offset;
		}

		// Token: 0x060087A0 RID: 34720 RVA: 0x0027BF80 File Offset: 0x0027A180
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

		// Token: 0x04005702 RID: 22274
		public ThingDef building;

		// Token: 0x04005703 RID: 22275
		public float offset;
	}
}
