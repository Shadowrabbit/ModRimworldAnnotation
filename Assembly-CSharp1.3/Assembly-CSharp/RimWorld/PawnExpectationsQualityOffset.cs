using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F69 RID: 3945
	public class PawnExpectationsQualityOffset
	{
		// Token: 0x06005D8A RID: 23946 RVA: 0x0020121C File Offset: 0x001FF41C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "expectation", xmlRoot.Name, null, null);
			this.offset = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400360B RID: 13835
		public ExpectationDef expectation;

		// Token: 0x0400360C RID: 13836
		public float offset;
	}
}
