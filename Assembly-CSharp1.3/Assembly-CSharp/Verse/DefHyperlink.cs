using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200007F RID: 127
	public class DefHyperlink
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x000033AC File Offset: 0x000015AC
		public DefHyperlink()
		{
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00019635 File Offset: 0x00017835
		public DefHyperlink(Def def)
		{
			this.def = def;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00019644 File Offset: 0x00017844
		public DefHyperlink(RoyalTitleDef def, Faction faction)
		{
			this.def = def;
			this.faction = faction;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001965C File Offset: 0x0001785C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured DefHyperlink: " + xmlRoot.OuterXml);
				return;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlRoot.Name, null);
			if (typeInAnyAssembly == null)
			{
				Log.Error("Misconfigured DefHyperlink. Could not find def of type " + xmlRoot.Name);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, typeInAnyAssembly);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000196D1 File Offset: 0x000178D1
		public static implicit operator DefHyperlink(Def def)
		{
			return new DefHyperlink
			{
				def = def
			};
		}

		// Token: 0x0400019A RID: 410
		public Def def;

		// Token: 0x0400019B RID: 411
		public Faction faction;
	}
}
