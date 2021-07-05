using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A62 RID: 2658
	public class PawnGenOption
	{
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0015A493 File Offset: 0x00158693
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0015A4A0 File Offset: 0x001586A0
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

		// Token: 0x06003FDB RID: 16347 RVA: 0x0015A52D File Offset: 0x0015872D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040023CE RID: 9166
		public PawnKindDef kind;

		// Token: 0x040023CF RID: 9167
		public float selectionWeight;
	}
}
