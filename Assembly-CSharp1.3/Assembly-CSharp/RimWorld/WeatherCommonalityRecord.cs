using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A4D RID: 2637
	public class WeatherCommonalityRecord
	{
		// Token: 0x06003F96 RID: 16278 RVA: 0x00159538 File Offset: 0x00157738
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04002323 RID: 8995
		public WeatherDef weather;

		// Token: 0x04002324 RID: 8996
		public float commonality;
	}
}
