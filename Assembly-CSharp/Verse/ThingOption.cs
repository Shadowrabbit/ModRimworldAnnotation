using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020007F2 RID: 2034
	public sealed class ThingOption
	{
		// Token: 0x0600338B RID: 13195 RVA: 0x0002878F File Offset: 0x0002698F
		public ThingOption()
		{
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x000287A2 File Offset: 0x000269A2
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x0014FAF4 File Offset: 0x0014DCF4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingOption: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x0014FB50 File Offset: 0x0014DD50
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

		// Token: 0x04002349 RID: 9033
		public ThingDef thingDef;

		// Token: 0x0400234A RID: 9034
		public float weight = 1f;
	}
}
