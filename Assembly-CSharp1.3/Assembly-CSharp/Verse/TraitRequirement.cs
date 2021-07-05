using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000E6 RID: 230
	public class TraitRequirement
	{
		// Token: 0x06000646 RID: 1606 RVA: 0x0001F1AA File Offset: 0x0001D3AA
		public bool Matches(Trait trait)
		{
			return trait.def == this.def && (this.degree == null || trait.Degree == this.degree.Value);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
		public bool HasTrait(Pawn p)
		{
			if (p.story == null)
			{
				return false;
			}
			if (this.degree == null)
			{
				return p.story.traits.HasTrait(this.def);
			}
			return p.story.traits.HasTrait(this.def, this.degree.Value);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001F23C File Offset: 0x0001D43C
		public Trait GetTrait(Pawn p)
		{
			if (p.story == null)
			{
				return null;
			}
			if (this.degree == null)
			{
				return p.story.traits.GetTrait(this.def);
			}
			return p.story.traits.GetTrait(this.def, this.degree.Value);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001F298 File Offset: 0x0001D498
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.Name == "li")
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.FirstChild.Value, null, null);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.Name, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.degree = new int?(ParseHelper.FromString<int>(xmlRoot.FirstChild.Value));
			}
		}

		// Token: 0x0400051A RID: 1306
		public TraitDef def;

		// Token: 0x0400051B RID: 1307
		public int? degree;
	}
}
