using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F73 RID: 3955
	public class WeatherCommonalityRecord
	{
		// Token: 0x060056D7 RID: 22231 RVA: 0x0003C3AE File Offset: 0x0003A5AE
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04003826 RID: 14374
		public WeatherDef weather;

		// Token: 0x04003827 RID: 14375
		public float commonality;
	}
}
