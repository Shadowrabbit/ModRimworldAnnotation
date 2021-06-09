using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D2 RID: 210
	public class DefHyperlink
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x00006B8B File Offset: 0x00004D8B
		public DefHyperlink()
		{
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0000B3B8 File Offset: 0x000095B8
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0000B3C7 File Offset: 0x000095C7
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0008EF8C File Offset: 0x0008D18C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured DefHyperlink: " + xmlRoot.OuterXml, false);
				return;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlRoot.Name, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Misconfigured DefHyperlink. Could not find def of type " + xmlRoot.Name, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, typeInAnyAssembly);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000B3DD File Offset: 0x000095DD
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		// Token: 0x04000327 RID: 807
		public Def def;

		// Token: 0x04000328 RID: 808
		public Faction faction;
	}
}
