using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200048F RID: 1167
	public sealed class ThingOption
	{
		// Token: 0x060023B2 RID: 9138 RVA: 0x000DE3A1 File Offset: 0x000DC5A1
		public ThingOption()
		{
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000DE3B4 File Offset: 0x000DC5B4
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000DE3D8 File Offset: 0x000DC5D8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingOption: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000DE434 File Offset: 0x000DC634
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				", weight=",
				this.weight.ToString("0.##"),
				")"
			});
		}

		// Token: 0x04001615 RID: 5653
		public ThingDef thingDef;

		// Token: 0x04001616 RID: 5654
		public float weight = 1f;
	}
}
