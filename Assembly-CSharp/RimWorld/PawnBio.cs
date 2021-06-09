using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BF RID: 5311
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x0600725A RID: 29274 RVA: 0x0004CE53 File Offset: 0x0004B053
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

		// Token: 0x0600725B RID: 29275 RVA: 0x0004CE6A File Offset: 0x0004B06A
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

		// Token: 0x0600725C RID: 29276 RVA: 0x0022F6E8 File Offset: 0x0022D8E8
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

		// Token: 0x0600725D RID: 29277 RVA: 0x0004CE92 File Offset: 0x0004B092
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

		// Token: 0x0600725E RID: 29278 RVA: 0x0004CEA2 File Offset: 0x0004B0A2
		public override string ToString()
		{
			return "PawnBio(" + this.name + ")";
		}

		// Token: 0x04004B56 RID: 19286
		public GenderPossibility gender;

		// Token: 0x04004B57 RID: 19287
		public NameTriple name;

		// Token: 0x04004B58 RID: 19288
		public Backstory childhood;

		// Token: 0x04004B59 RID: 19289
		public Backstory adulthood;

		// Token: 0x04004B5A RID: 19290
		public bool pirateKing;
	}
}
