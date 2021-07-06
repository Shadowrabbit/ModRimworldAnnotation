using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D3 RID: 211
	public class Def : Editable
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0000B3EB File Offset: 0x000095EB
		public TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0000B466 File Offset: 0x00009666
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0000B46F File Offset: 0x0000966F
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				yield return base.GetType() + " lacks defName. Label=" + this.label;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
			}
			if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
			}
			if (this.modExtensions != null)
			{
				int num;
				for (int i = 0; i < this.modExtensions.Count; i = num)
				{
					foreach (string text in this.modExtensions[i].ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
					num = i + 1;
				}
			}
			if (this.description != null)
			{
				if (this.description == "")
				{
					yield return "empty description";
				}
				if (char.IsWhiteSpace(this.description[0]))
				{
					yield return "description has leading whitespace";
				}
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					yield return "description has trailing whitespace";
				}
			}
			if (this.descriptionHyperlinks != null && this.descriptionHyperlinks.Count > 0)
			{
				if (this.descriptionHyperlinks.RemoveAll((DefHyperlink x) => x.def == null) != 0)
				{
					Log.Warning("Some descriptionHyperlinks in " + this.defName + " had null def.", false);
				}
				Def.<>c__DisplayClass19_0 CS$<>8__locals1 = new Def.<>c__DisplayClass19_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.i = this.descriptionHyperlinks.Count - 1;
				while (CS$<>8__locals1.i > 0)
				{
					if (this.descriptionHyperlinks.FirstIndexOf((DefHyperlink h) => h.def == CS$<>8__locals1.<>4__this.descriptionHyperlinks[CS$<>8__locals1.i].def) < CS$<>8__locals1.i)
					{
						yield return string.Concat(new string[]
						{
							"Hyperlink to ",
							this.descriptionHyperlinks[CS$<>8__locals1.i].def.defName,
							" more than once on ",
							this.defName,
							" description"
						});
					}
					int num = CS$<>8__locals1.i;
					CS$<>8__locals1.i = num - 1;
				}
				CS$<>8__locals1 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0000B47F File Offset: 0x0000967F
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0000B48D File Offset: 0x0000968D
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0000B495 File Offset: 0x00009695
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0008F004 File Offset: 0x0008D204
		public T GetModExtension<T>() where T : DefModExtension
		{
			if (this.modExtensions == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.modExtensions.Count; i++)
			{
				if (this.modExtensions[i] is T)
				{
					return this.modExtensions[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0000B4A2 File Offset: 0x000096A2
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x04000329 RID: 809
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x0400032A RID: 810
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label;

		// Token: 0x0400032B RID: 811
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description;

		// Token: 0x0400032C RID: 812
		[XmlInheritanceAllowDuplicateNodes]
		public List<DefHyperlink> descriptionHyperlinks;

		// Token: 0x0400032D RID: 813
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors;

		// Token: 0x0400032E RID: 814
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x0400032F RID: 815
		[Unsaved(false)]
		public ushort shortHash;

		// Token: 0x04000330 RID: 816
		[Unsaved(false)]
		public ushort index = ushort.MaxValue;

		// Token: 0x04000331 RID: 817
		[Unsaved(false)]
		public ModContentPack modContentPack;

		// Token: 0x04000332 RID: 818
		[Unsaved(false)]
		public string fileName;

		// Token: 0x04000333 RID: 819
		[Unsaved(false)]
		private TaggedString cachedLabelCap = null;

		// Token: 0x04000334 RID: 820
		[Unsaved(false)]
		public bool generated;

		// Token: 0x04000335 RID: 821
		[Unsaved(false)]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x04000336 RID: 822
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x04000337 RID: 823
		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");
	}
}
