using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000ECF RID: 3791
	public class IdeoFoundation_Deity : IdeoFoundation
	{
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x0600597B RID: 22907 RVA: 0x001E951F File Offset: 0x001E771F
		public List<IdeoFoundation_Deity.Deity> DeitiesListForReading
		{
			get
			{
				return this.deities;
			}
		}

		// Token: 0x0600597C RID: 22908 RVA: 0x001E9528 File Offset: 0x001E7728
		public override void Init(IdeoGenerationParms parms)
		{
			if (!ModLister.CheckIdeology("Ideoligion"))
			{
				return;
			}
			this.RandomizeCulture(parms);
			this.RandomizePlace();
			this.RandomizeMemes(parms);
			this.GenerateDeities();
			this.GenerateTextSymbols();
			this.GenerateLeaderTitle();
			this.RandomizeIcon();
			this.RandomizePrecepts(true, parms);
			this.ideo.RegenerateDescription(true);
			this.RandomizeStyles();
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x001E9588 File Offset: 0x001E7788
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IdeoFoundation_Deity.Deity>(ref this.deities, "deities", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x001E95A8 File Offset: 0x001E77A8
		public override void DoInfo(ref float curY, float width, IdeoEditMode editMode)
		{
			if (this.deities.Count == 0 && this.ideo.DeityCountRange.max <= 0)
			{
				return;
			}
			curY += 4f;
			Widgets.Label((width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f, ref curY, width, "Deities".Translate(), default(TipSignal));
			if (editMode != IdeoEditMode.None)
			{
				float num = width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
				Rect rect = new Rect(num - IdeoUIUtility.ButtonSize.x, curY - IdeoUIUtility.ButtonSize.y, IdeoUIUtility.ButtonSize.x, IdeoUIUtility.ButtonSize.y);
				Rect rect2 = rect;
				rect2.x = rect.xMin - rect.width - 10f;
				bool flag = this.deities.Count < this.ideo.DeityCountRange.max;
				if (Widgets.ButtonText(flag ? rect2 : rect, "RandomizeDeities".Translate(), true, true, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					this.GenerateDeities();
					this.ideo.RegenerateAllPreceptNames();
					this.ideo.RegenerateDescription(false);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (flag && Widgets.ButtonText(rect, "AddDeity".Translate(), true, true, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					this.deities.Add(this.GenerateNewDeity());
					this.ideo.RegenerateAllPreceptNames();
					this.ideo.RegenerateDescription(false);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			curY += 4f;
			for (int i = 0; i < this.deities.Count; i++)
			{
				IdeoFoundation_Deity.Deity curDeity = this.deities[i];
				int num2 = i / 3;
				int num3 = i % 3;
				int num4 = (i >= this.deities.Count - this.deities.Count % 3) ? (this.deities.Count % 3) : 3;
				float num5 = (width - (float)num4 * IdeoFoundation_Deity.DeityBoxSize.x - (float)((num4 - 1) * 8)) / 2f;
				Rect rect3 = new Rect(num5 + (float)num3 * IdeoFoundation_Deity.DeityBoxSize.x + (float)(num3 * 8), curY + (float)num2 * IdeoFoundation_Deity.DeityBoxSize.y + (float)(num2 * 8), IdeoFoundation_Deity.DeityBoxSize.x, IdeoFoundation_Deity.DeityBoxSize.y);
				Widgets.DrawLightHighlight(rect3);
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					string text = curDeity.name.Colorize(ColoredText.TipSectionTitleColor) + "\n" + curDeity.type;
					if (curDeity.relatedMeme != null)
					{
						text = string.Concat(new string[]
						{
							text,
							"\n\n",
							"RelatedToMeme".Translate().Colorize(ColoredText.TipSectionTitleColor),
							": ",
							curDeity.relatedMeme.LabelCap.Resolve()
						});
					}
					if (editMode != IdeoEditMode.None)
					{
						text = text + "\n\n" + IdeoUIUtility.ClickToEdit;
					}
					TooltipHandler.TipRegion(rect3, text);
				}
				if (Widgets.ButtonInvisible(rect3, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					Action action = null;
					TaggedString taggedString = "Remove".Translate().CapitalizeFirst();
					int min = this.ideo.DeityCountRange.min;
					if (this.deities.Count > min)
					{
						action = delegate()
						{
							this.deities.Remove(curDeity);
							this.ideo.RegenerateDescription(false);
						};
					}
					else
					{
						string arg = (min <= 1) ? "Deity".Translate() : Find.ActiveLanguageWorker.Pluralize("Deity".Translate(), min);
						taggedString += " (" + "DeitiesRequired".Translate(min, arg.Named("DEITYNOUN")) + ")";
					}
					list.Add(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list.Add(new FloatMenuOption("Regenerate".Translate().CapitalizeFirst(), delegate()
					{
						this.FillDeity(curDeity);
						this.ideo.RegenerateDescription(false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list.Add(new FloatMenuOption("EditDeity".Translate().CapitalizeFirst(), delegate()
					{
						Find.WindowStack.Add(new Dialog_EditDeity(curDeity, this.ideo));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					Find.WindowStack.Add(new FloatMenu(list));
				}
				Rect position = new Rect(rect3.x + (rect3.height - 50f) / 2f, rect3.y + (rect3.height - 50f) / 2f, 50f, 50f);
				GUI.DrawTexture(position, this.deities[i].Icon);
				Rect rect4 = new Rect(position.xMax + 10f, rect3.y + 3f, rect3.xMax - position.xMax - 10f, rect3.height / 2f);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect4, this.deities[i].name);
				GenUI.ResetLabelAlign();
				Rect rect5 = new Rect(position.xMax + 10f, rect3.y + rect3.height / 2f - 3f, rect3.xMax - position.xMax - 10f, rect3.height / 2f);
				Text.Anchor = TextAnchor.MiddleLeft;
				GUI.color = new Color(0.8f, 0.8f, 0.8f);
				Widgets.Label(rect5, this.deities[i].type);
				GUI.color = Color.white;
				GenUI.ResetLabelAlign();
				GUI.DrawTexture(new Rect(rect3.xMax - 20f - 4f, rect3.y + 4f, 20f, 20f), this.deities[i].gender.GetIcon());
			}
			int num6 = Mathf.CeilToInt((float)this.deities.Count / 3f);
			curY += (float)num6 * IdeoFoundation_Deity.DeityBoxSize.y + (float)((num6 - 1) * 8);
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x001E9C74 File Offset: 0x001E7E74
		public override void GenerateTextSymbols()
		{
			IdeoFoundation_Deity.<>c__DisplayClass11_0 CS$<>8__locals1 = new IdeoFoundation_Deity.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.ideo.culture == null)
			{
				return;
			}
			this.ideo.usedSymbols.Clear();
			this.ideo.usedSymbolPacks.Clear();
			CS$<>8__locals1.usedMemes = new List<MemeDef>();
			CS$<>8__locals1.request = default(GrammarRequest);
			CS$<>8__locals1.request.Includes.Add(this.ideo.culture.ideoNameMaker);
			base.AddPlaceRules(ref CS$<>8__locals1.request);
			this.AddDeityRules(ref CS$<>8__locals1.request);
			List<IdeoFoundation_Deity.SymbolSource> list = new List<IdeoFoundation_Deity.SymbolSource>();
			if (this.ideo.memes.Any((MemeDef m) => !m.symbolPacks.NullOrEmpty<IdeoSymbolPack>()))
			{
				list.Add(IdeoFoundation_Deity.SymbolSource.Pack);
			}
			if (this.deities.Count >= 1)
			{
				if (!this.ideo.memes.Any((MemeDef m) => !m.allowSymbolsFromDeity))
				{
					list.Add(IdeoFoundation_Deity.SymbolSource.Deity);
				}
			}
			if (list.Count == 0)
			{
				Log.Error("No way to generate ideo symbols. Memes: " + (from m in this.ideo.memes
				select m.defName).ToCommaList(false, false));
				this.ideo.name = "Errorism";
				this.ideo.adjective = "Errorist";
				this.ideo.memberName = "Errorist";
				return;
			}
			IdeoFoundation_Deity.SymbolSource symbolSource = list.RandomElementByWeight(delegate(IdeoFoundation_Deity.SymbolSource s)
			{
				if (s == IdeoFoundation_Deity.SymbolSource.Pack)
				{
					return 1f;
				}
				if (s == IdeoFoundation_Deity.SymbolSource.Deity)
				{
					return 0.5f;
				}
				throw new NotImplementedException();
			});
			if (symbolSource != IdeoFoundation_Deity.SymbolSource.Pack)
			{
				if (symbolSource == IdeoFoundation_Deity.SymbolSource.Deity)
				{
					CS$<>8__locals1.<GenerateTextSymbols>g__SetupFromDeity|2();
				}
			}
			else
			{
				CS$<>8__locals1.<GenerateTextSymbols>g__SetupFromSymbolPack|1();
			}
			this.ideo.name = this.GetResolvedText("r_ideoName", CS$<>8__locals1.request, true);
			this.ideo.name = GenText.CapitalizeAsTitle(this.ideo.name);
			this.ideo.adjective = this.GetResolvedText("r_ideoAdjective", CS$<>8__locals1.request, false);
			this.ideo.memberName = this.GetResolvedText("r_memberName", CS$<>8__locals1.request, true);
		}

		// Token: 0x06005980 RID: 22912 RVA: 0x001E9EBC File Offset: 0x001E80BC
		public void AddDeityRules(ref GrammarRequest request)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.AddDeityRules(dictionary);
			request.Rules.AddRange(from kv in dictionary
			select new Rule_String(kv.Key, kv.Value));
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x001E9F08 File Offset: 0x001E8108
		public void AddDeityRules(Dictionary<string, string> tokens)
		{
			for (int i = 0; i < this.deities.Count; i++)
			{
				IdeoFoundation_Deity.Deity deity = this.deities[i];
				string str = string.Format("deity{0}_", i);
				tokens.AddDistinct(str + "name", deity.name.ApplyTag(TagType.Name, null).Resolve());
				tokens.AddDistinct(str + "pronoun", deity.gender.GetPronoun());
				tokens.AddDistinct(str + "objective", deity.gender.GetObjective());
				tokens.AddDistinct(str + "possessive", deity.gender.GetPossessive());
				tokens.AddDistinct(str + "type", deity.type);
			}
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x001E9FE0 File Offset: 0x001E81E0
		private bool CanUseSymbolPack(IdeoSymbolPack pack)
		{
			if (Find.World == null || Find.IdeoManager == null)
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.usedSymbolPacks.Contains(pack.PrimarySymbol))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x001EA058 File Offset: 0x001E8258
		private bool CanUseSymbol(string symbol)
		{
			if (Find.World == null || Find.IdeoManager == null)
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.usedSymbols.Contains(symbol))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x001EA0CC File Offset: 0x001E82CC
		private string GetResolvedText(string key, GrammarRequest request, bool capitalizeFirstSentence = true)
		{
			string text = GrammarResolver.Resolve(key, request, null, false, null, null, null, capitalizeFirstSentence);
			for (int i = 0; i < 10; i++)
			{
				if (this.CanUseSymbol(text))
				{
					this.ideo.usedSymbols.Add(text);
					return text;
				}
				text = GrammarResolver.Resolve(key, request, null, false, null, null, null, capitalizeFirstSentence);
			}
			this.ideo.usedSymbols.Add(text);
			return text;
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x001EA130 File Offset: 0x001E8330
		public void GenerateDeities()
		{
			this.deities.Clear();
			int num;
			if (Rand.Chance(0.5f))
			{
				num = 0;
			}
			else if (Rand.Chance(0.5f))
			{
				num = 1;
			}
			else
			{
				num = Rand.RangeInclusive(2, 4);
			}
			IntRange deityCountRange = this.ideo.DeityCountRange;
			num = Mathf.Clamp(num, deityCountRange.min, deityCountRange.max);
			for (int i = 0; i < num; i++)
			{
				IdeoFoundation_Deity.Deity item = this.GenerateNewDeity();
				this.deities.Add(item);
			}
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x001EA1B0 File Offset: 0x001E83B0
		private void FillDeity(IdeoFoundation_Deity.Deity deity)
		{
			Gender supremeGender = this.ideo.SupremeGender;
			if (supremeGender != Gender.None)
			{
				deity.gender = supremeGender;
			}
			else
			{
				deity.gender = Gen.RandomEnumValue<Gender>(true);
			}
			MemeDef relatedMeme;
			MemeDef relatedMeme2;
			if ((from x in this.ideo.memes
			where !this.deities.Any((IdeoFoundation_Deity.Deity y) => y.relatedMeme == x)
			select x).TryRandomElement(out relatedMeme))
			{
				deity.relatedMeme = relatedMeme;
			}
			else if (this.ideo.memes.TryRandomElement(out relatedMeme2))
			{
				deity.relatedMeme = relatedMeme2;
			}
			if (this.ideo.StructureMeme.fixedDeityNameTypes != null)
			{
				int num = 0;
				DeityNameType deityNameType;
				do
				{
					deityNameType = this.ideo.StructureMeme.fixedDeityNameTypes.RandomElement<DeityNameType>();
					if (!this.<FillDeity>g__AllExistingDeities|18_0().Contains(deityNameType.name))
					{
						goto IL_BF;
					}
					num++;
				}
				while (num <= 20);
				Log.Error("Could not get a unique fixed deity name and type after a reasonable number of tries.");
				IL_BF:
				deity.name = deityNameType.name;
				deity.type = deityNameType.type;
			}
			else
			{
				GrammarRequest request = default(GrammarRequest);
				RulePackDef item = this.ideo.StructureMeme.deityNameMakerOverride ?? this.ideo.culture.deityNameMaker;
				request.Includes.Add(item);
				deity.name = NameGenerator.GenerateName(request, (string x) => !this.<FillDeity>g__AllExistingDeities|18_0().Contains(x), false, "r_deityName", null);
				GrammarRequest request2 = default(GrammarRequest);
				RulePackDef item2 = this.ideo.StructureMeme.deityTypeMakerOverride ?? this.ideo.culture.deityTypeMaker;
				request2.Includes.Add(item2);
				if (deity.relatedMeme != null && deity.relatedMeme.generalRules != null)
				{
					request2.IncludesBare.Add(deity.relatedMeme.generalRules);
				}
				deity.type = NameGenerator.GenerateName(request2, null, false, "r_deityType", null);
			}
			deity.iconPath = "UI/Deities/DeityGeneric";
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x001EA380 File Offset: 0x001E8580
		private IdeoFoundation_Deity.Deity GenerateNewDeity()
		{
			IdeoFoundation_Deity.Deity deity = new IdeoFoundation_Deity.Deity();
			this.FillDeity(deity);
			return deity;
		}

		// Token: 0x0600598C RID: 22924 RVA: 0x001EA401 File Offset: 0x001E8601
		[CompilerGenerated]
		private IEnumerable<string> <FillDeity>g__AllExistingDeities|18_0()
		{
			int num;
			for (int i = 0; i < this.deities.Count; i = num + 1)
			{
				yield return this.deities[i].name;
				num = i;
			}
			if (Find.World != null)
			{
				List<Ideo> ideos = Find.IdeoManager.IdeosListForReading;
				for (int i = 0; i < ideos.Count; i = num + 1)
				{
					IdeoFoundation_Deity deityFoundation;
					if ((deityFoundation = (ideos[i].foundation as IdeoFoundation_Deity)) != null)
					{
						for (int j = 0; j < deityFoundation.deities.Count; j = num + 1)
						{
							yield return deityFoundation.deities[j].name;
							num = j;
						}
					}
					deityFoundation = null;
					num = i;
				}
				ideos = null;
			}
			yield break;
		}

		// Token: 0x0400347C RID: 13436
		private List<IdeoFoundation_Deity.Deity> deities = new List<IdeoFoundation_Deity.Deity>();

		// Token: 0x0400347D RID: 13437
		private static readonly Vector2 DeityBoxSize = IdeoUIUtility.PreceptBoxSize;

		// Token: 0x0400347E RID: 13438
		private const float IconSize = 50f;

		// Token: 0x0400347F RID: 13439
		private const float GenderIconSize = 20f;

		// Token: 0x02002330 RID: 9008
		public class Deity : IExposable
		{
			// Token: 0x17001E43 RID: 7747
			// (get) Token: 0x0600C60C RID: 50700 RVA: 0x003DEFD2 File Offset: 0x003DD1D2
			public Texture2D Icon
			{
				get
				{
					if (this.icon == null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
					return this.icon;
				}
			}

			// Token: 0x0600C60D RID: 50701 RVA: 0x003DEFFC File Offset: 0x003DD1FC
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.name, "name", null, false);
				Scribe_Values.Look<string>(ref this.type, "type", null, false);
				Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
				Scribe_Values.Look<string>(ref this.iconPath, "iconPath", null, false);
				Scribe_Defs.Look<MemeDef>(ref this.relatedMeme, "relatedMeme");
			}

			// Token: 0x04008621 RID: 34337
			public string name;

			// Token: 0x04008622 RID: 34338
			public string type;

			// Token: 0x04008623 RID: 34339
			public Gender gender;

			// Token: 0x04008624 RID: 34340
			public string iconPath;

			// Token: 0x04008625 RID: 34341
			public MemeDef relatedMeme;

			// Token: 0x04008626 RID: 34342
			private Texture2D icon;
		}

		// Token: 0x02002331 RID: 9009
		private enum SymbolSource
		{
			// Token: 0x04008628 RID: 34344
			Pack,
			// Token: 0x04008629 RID: 34345
			Deity
		}
	}
}
