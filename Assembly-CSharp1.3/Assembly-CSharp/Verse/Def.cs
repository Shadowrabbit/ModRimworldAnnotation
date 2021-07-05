using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x02000080 RID: 128
	public class Def : Editable
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000196DF File Offset: 0x000178DF
		public virtual TaggedString LabelCap
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

		// Token: 0x060004BD RID: 1213 RVA: 0x0001975A File Offset: 0x0001795A
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00019763 File Offset: 0x00017963
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
					Log.Warning("Some descriptionHyperlinks in " + this.defName + " had null def.");
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

		// Token: 0x060004BF RID: 1215 RVA: 0x00019773 File Offset: 0x00017973
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00019781 File Offset: 0x00017981
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00019789 File Offset: 0x00017989
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00019798 File Offset: 0x00017998
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

		// Token: 0x060004C3 RID: 1219 RVA: 0x00019800 File Offset: 0x00017A00
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x0400019C RID: 412
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x0400019D RID: 413
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label;

		// Token: 0x0400019E RID: 414
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description;

		// Token: 0x0400019F RID: 415
		[XmlInheritanceAllowDuplicateNodes]
		public List<DefHyperlink> descriptionHyperlinks;

		// Token: 0x040001A0 RID: 416
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors;

		// Token: 0x040001A1 RID: 417
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x040001A2 RID: 418
		[Unsaved(false)]
		public ushort shortHash;

		// Token: 0x040001A3 RID: 419
		[Unsaved(false)]
		public ushort index = ushort.MaxValue;

		// Token: 0x040001A4 RID: 420
		[Unsaved(false)]
		public ModContentPack modContentPack;

		// Token: 0x040001A5 RID: 421
		[Unsaved(false)]
		public string fileName;

		// Token: 0x040001A6 RID: 422
		[Unsaved(false)]
		protected TaggedString cachedLabelCap = null;

		// Token: 0x040001A7 RID: 423
		[Unsaved(false)]
		public bool generated;

		// Token: 0x040001A8 RID: 424
		[Unsaved(false)]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x040001A9 RID: 425
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x040001AA RID: 426
		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");
	}
}
