using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E26 RID: 3622
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x001C5A6B File Offset: 0x001C3C6B
		public PawnBioType BioType
		{
			get
			{
				if (this.pirateKing)
				{
					return PawnBioType.PirateKing;
				}
				if (this.adulthood != null)
				{
					return PawnBioType.BackstoryInGame;
				}
				return PawnBioType.Undefined;
			}
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x001C5A82 File Offset: 0x001C3C82
		public void PostLoad()
		{
			if (this.childhood != null)
			{
				this.childhood.PostLoad();
			}
			if (this.adulthood != null)
			{
				this.adulthood.PostLoad();
			}
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x001C5AAC File Offset: 0x001C3CAC
		public void ResolveReferences()
		{
			if (this.adulthood.spawnCategories.Count == 1 && this.adulthood.spawnCategories[0] == "Trader")
			{
				this.adulthood.spawnCategories.Add("Civil");
			}
			if (this.childhood != null)
			{
				this.childhood.ResolveReferences();
			}
			if (this.adulthood != null)
			{
				this.adulthood.ResolveReferences();
			}
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x001C5B24 File Offset: 0x001C3D24
		public IEnumerable<string> ConfigErrors()
		{
			if (this.childhood != null)
			{
				foreach (string text in this.childhood.ConfigErrors(true))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.childhood.title,
						": ",
						text
					});
				}
				IEnumerator<string> enumerator = null;
			}
			if (this.adulthood != null)
			{
				foreach (string text2 in this.adulthood.ConfigErrors(false))
				{
					yield return string.Concat(new object[]
					{
						this.name,
						", ",
						this.adulthood.title,
						": ",
						text2
					});
				}
				IEnumerator<string> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x001C5B34 File Offset: 0x001C3D34
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}

		// Token: 0x04003148 RID: 12616
		public GenderPossibility gender;

		// Token: 0x04003149 RID: 12617
		public NameTriple name;

		// Token: 0x0400314A RID: 12618
		public Backstory childhood;

		// Token: 0x0400314B RID: 12619
		public Backstory adulthood;

		// Token: 0x0400314C RID: 12620
		public bool pirateKing;
	}
}
