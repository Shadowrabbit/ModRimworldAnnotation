using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F88 RID: 3976
	public class PawnGenOption
	{
		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x0600573B RID: 22331 RVA: 0x0003C7EC File Offset: 0x0003A9EC
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x001CCB04 File Offset: 0x001CAD04
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.kind != null) ? this.kind.ToString() : "null",
				" w=",
				this.selectionWeight.ToString("F2"),
				" c=",
				(this.kind != null) ? this.Cost.ToString("F2") : "null",
				")"
			});
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x0003C7F9 File Offset: 0x0003A9F9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040038B3 RID: 14515
		public PawnKindDef kind;

		// Token: 0x040038B4 RID: 14516
		public float selectionWeight;
	}
}
