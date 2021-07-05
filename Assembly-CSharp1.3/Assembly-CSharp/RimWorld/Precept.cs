using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EF0 RID: 3824
	public class Precept : IExposable, ILoadReferenceable
	{
		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06005ACD RID: 23245 RVA: 0x001F64F8 File Offset: 0x001F46F8
		public virtual string TipLabel
		{
			get
			{
				return this.def.tipLabelOverride ?? (this.def.issue.LabelCap + ": " + this.def.LabelCap);
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06005ACE RID: 23246 RVA: 0x001F6538 File Offset: 0x001F4738
		public virtual string UIInfoFirstLine
		{
			get
			{
				return this.def.issue.LabelCap;
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x001F654F File Offset: 0x001F474F
		public virtual string UIInfoSecondLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06005AD0 RID: 23248 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CanRegenerate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool SortByImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06005AD2 RID: 23250 RVA: 0x001F6561 File Offset: 0x001F4761
		public virtual Color LabelColor
		{
			get
			{
				return new Color(0.8f, 0.8f, 0.8f);
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x001F6577 File Offset: 0x001F4777
		public virtual string Label
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06005AD4 RID: 23252 RVA: 0x001F657F File Offset: 0x001F477F
		public string LabelCap
		{
			get
			{
				if (this.labelCapCache == null)
				{
					this.labelCapCache = this.name.CapitalizeFirst();
				}
				return this.labelCapCache;
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x001F65A0 File Offset: 0x001F47A0
		public string Description
		{
			get
			{
				if (!this.descOverride.NullOrEmpty())
				{
					return this.descOverride;
				}
				return this.def.description;
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x001F65C1 File Offset: 0x001F47C1
		public virtual string DescriptionForTip
		{
			get
			{
				return this.Description;
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool UsesGeneratedName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06005AD8 RID: 23256 RVA: 0x001F65C9 File Offset: 0x001F47C9
		public virtual Texture2D Icon
		{
			get
			{
				return this.def.Icon ?? this.ideo.Icon;
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x00002688 File Offset: 0x00000888
		// (set) Token: 0x06005ADA RID: 23258 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual List<PreceptApparelRequirement> ApparelRequirements
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x001F65E5 File Offset: 0x001F47E5
		public int Id
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostMake()
		{
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x001F65ED File Offset: 0x001F47ED
		public virtual void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			this.ideo = ideo;
			this.ID = Find.UniqueIDsManager.GetNextPreceptID();
			this.randomSeed = Rand.Int;
			this.name = this.def.issue.label;
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x001F6627 File Offset: 0x001F4827
		public virtual void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			this.Init(ideo, generatingFor);
			this.ClearTipCache();
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x001F6637 File Offset: 0x001F4837
		public virtual void ClearTipCache()
		{
			this.tipCached = null;
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x001F6640 File Offset: 0x001F4840
		protected string ColorizeDescTitle(TaggedString title)
		{
			return title.Resolve().Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x001F6653 File Offset: 0x001F4853
		protected string ColorizeWarning(TaggedString title)
		{
			return title.Resolve().Colorize(ColoredText.ThreatColor);
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x001F6668 File Offset: 0x001F4868
		public virtual string GetTip()
		{
			Precept.tmpCompsDesc.Clear();
			if (!this.DescriptionForTip.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Append(this.DescriptionForTip);
			}
			bool flag = true;
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_UnwillingToDo preceptComp_UnwillingToDo;
				if ((preceptComp_UnwillingToDo = (this.def.comps[i] as PreceptComp_UnwillingToDo)) != null)
				{
					string prohibitionText = preceptComp_UnwillingToDo.GetProhibitionText();
					if (!prohibitionText.NullOrEmpty())
					{
						if (flag)
						{
							flag = false;
							Precept.tmpCompsDesc.AppendLine();
							Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Prohibitions".Translate() + ":"));
						}
						Precept.tmpCompsDesc.AppendInNewLine("  - " + prohibitionText);
					}
				}
			}
			bool flag2 = true;
			int j = 0;
			while (j < this.def.comps.Count)
			{
				PreceptComp_SelfTookMemoryThought preceptComp_SelfTookMemoryThought;
				PreceptComp preceptComp;
				if ((preceptComp_SelfTookMemoryThought = (this.def.comps[j] as PreceptComp_SelfTookMemoryThought)) != null)
				{
					preceptComp = preceptComp_SelfTookMemoryThought;
					goto IL_176;
				}
				PreceptComp_SituationalThought preceptComp_SituationalThought;
				if ((preceptComp_SituationalThought = (this.def.comps[j] as PreceptComp_SituationalThought)) != null && preceptComp_SituationalThought.AffectsMood)
				{
					preceptComp = preceptComp_SituationalThought;
					goto IL_176;
				}
				PreceptComp_KnowsMemoryThought preceptComp_KnowsMemoryThought;
				if ((preceptComp_KnowsMemoryThought = (this.def.comps[j] as PreceptComp_KnowsMemoryThought)) != null && preceptComp_KnowsMemoryThought.AffectsMood)
				{
					preceptComp = preceptComp_KnowsMemoryThought;
					goto IL_176;
				}
				PreceptComp_BedThought preceptComp_BedThought;
				if ((preceptComp_BedThought = (this.def.comps[j] as PreceptComp_BedThought)) != null && preceptComp_BedThought.AffectsMood)
				{
					preceptComp = preceptComp_BedThought;
					goto IL_176;
				}
				IL_1F0:
				j++;
				continue;
				IL_176:
				if (flag2)
				{
					flag2 = false;
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Mood".Translate() + ":"));
				}
				foreach (string str in preceptComp.GetDescriptions())
				{
					Precept.tmpCompsDesc.AppendInNewLine("  - " + str);
				}
				goto IL_1F0;
			}
			bool flag3 = true;
			int k = 0;
			while (k < this.def.comps.Count)
			{
				PreceptComp_SituationalThought preceptComp_SituationalThought2;
				PreceptComp preceptComp2;
				if ((preceptComp_SituationalThought2 = (this.def.comps[k] as PreceptComp_SituationalThought)) != null && !preceptComp_SituationalThought2.AffectsMood)
				{
					preceptComp2 = preceptComp_SituationalThought2;
					goto IL_26E;
				}
				PreceptComp_KnowsMemoryThought preceptComp_KnowsMemoryThought2;
				if ((preceptComp_KnowsMemoryThought2 = (this.def.comps[k] as PreceptComp_KnowsMemoryThought)) != null && !preceptComp_KnowsMemoryThought2.AffectsMood)
				{
					preceptComp2 = preceptComp_KnowsMemoryThought2;
					goto IL_26E;
				}
				IL_2E8:
				k++;
				continue;
				IL_26E:
				if (flag3)
				{
					flag3 = false;
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Opinions".Translate() + ":"));
				}
				foreach (string str2 in preceptComp2.GetDescriptions())
				{
					Precept.tmpCompsDesc.AppendInNewLine("  - " + str2);
				}
				goto IL_2E8;
			}
			bool flag4 = true;
			if (this.def.statOffsets != null)
			{
				for (int l = 0; l < this.def.statOffsets.Count; l++)
				{
					if (flag4)
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("PreceptStats".Translate() + ":"));
						flag4 = false;
					}
					Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.statOffsets[l].stat.LabelCap + ": " + this.def.statOffsets[l].ValueToStringAsOffset);
				}
			}
			if (this.def.statFactors != null)
			{
				for (int m = 0; m < this.def.statFactors.Count; m++)
				{
					if (flag4)
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("PreceptStats".Translate() + ":"));
						flag4 = false;
					}
					Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.statFactors[m].stat.LabelCap + ": " + this.def.statFactors[m].ToStringAsFactor);
				}
			}
			if (this.def.abilityStatFactors != null)
			{
				for (int n = 0; n < this.def.abilityStatFactors.Count; n++)
				{
					foreach (StatModifier statModifier in this.def.abilityStatFactors[n].modifiers)
					{
						if (flag4)
						{
							Precept.tmpCompsDesc.AppendLine();
							Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("PreceptStats".Translate() + ":"));
							flag4 = false;
						}
						Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.abilityStatFactors[n].ability.LabelCap + ": " + statModifier.stat.LabelCap + ": " + statModifier.ToStringAsFactor);
					}
				}
			}
			if (this.def.TraitsAffecting.Count != 0)
			{
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("AffectedByTraits".Translate() + ":"));
				foreach (TraitDegreeData traitDegreeData in from x in this.def.TraitsAffecting
				select x.def.DataAtDegree((x.degree != null) ? x.degree.Value : 0))
				{
					Precept.tmpCompsDesc.AppendInNewLine("  - " + traitDegreeData.GetLabelFor(Gender.Male).CapitalizeFirst());
				}
			}
			return Precept.tmpCompsDesc.ToString();
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x001F6D18 File Offset: 0x001F4F18
		public virtual IEnumerable<Alert> GetAlerts()
		{
			yield break;
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Tick()
		{
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x0002974C File Offset: 0x0002794C
		public virtual string GenerateNameRaw()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x00002688 File Offset: 0x00000888
		public virtual List<PreceptApparelRequirement> GenerateNewApparelRequirements(FactionDef generatingFor = null)
		{
			return null;
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x001F6D24 File Offset: 0x001F4F24
		public string GenerateNewName()
		{
			Precept.tmpUsedWords.Clear();
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				if (precept != this && precept.GetType() == base.GetType() && precept.name != null)
				{
					Precept.tmpUsedWords.Add(precept.name);
					foreach (string item in precept.name.Split(new char[]
					{
						' '
					}))
					{
						Precept.tmpUsedWords.Add(item);
					}
				}
			}
			float num = 0f;
			string text = null;
			if (this.def.takeNameFrom != null)
			{
				return this.ideo.PreceptsListForReading.First((Precept p) => p.def == this.def.takeNameFrom).name;
			}
			this.labelCapCache = null;
			if (this.def.ignoreNameUniqueness)
			{
				text = this.GenerateNameRaw();
			}
			else
			{
				int num2 = 0;
				string text2;
				for (;;)
				{
					text2 = this.GenerateNameRaw();
					float num3 = Precept.<GenerateNewName>g__NameUniqueness|54_0(text2);
					if (num3 >= 1f)
					{
						break;
					}
					if (num3 > num)
					{
						text = text2;
						num = num3;
					}
					if (num2++ > 50)
					{
						goto Block_6;
					}
				}
				return text2;
				Block_6:
				if (this.def.visible)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Failed to generate a unique precept name: ",
						text,
						" at ",
						num.ToStringPercent(),
						" uniqueness"
					}));
				}
			}
			return text;
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x001F6EC4 File Offset: 0x001F50C4
		public void RegenerateName()
		{
			this.SetName(this.GenerateNewName());
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x001F6ED4 File Offset: 0x001F50D4
		public void SetName(string newName)
		{
			if (newName == this.name)
			{
				return;
			}
			this.labelCapCache = null;
			this.name = newName;
			if (this.ideo != null)
			{
				foreach (Precept precept in this.ideo.PreceptsListForReading)
				{
					if (precept.def.takeNameFrom == this.def)
					{
						precept.RegenerateName();
					}
				}
			}
		}

		// Token: 0x06005AEA RID: 23274 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			return null;
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x001F6F64 File Offset: 0x001F5164
		public virtual IEnumerable<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			yield break;
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x001F6F70 File Offset: 0x001F5170
		protected void AddIdeoRulesTo(ref GrammarRequest request)
		{
			string ideoName = null;
			IdeoSymbolPack ideoSymbolPack;
			if (this.TryGuessChosenSymbolPack(out ideoSymbolPack))
			{
				if (!ideoSymbolPack.ideoName.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("chosenIdeoName", ideoSymbolPack.ideoName));
					ideoName = ideoSymbolPack.ideoName;
				}
				if (!ideoSymbolPack.theme.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("chosenTheme", this.MatchIdeoNameCapitalization(ideoSymbolPack.theme, ideoName)));
				}
			}
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (memeDef.generalRules != null)
				{
					request.IncludesBare.Add(memeDef.generalRules);
				}
				if (memeDef.symbolPacks != null)
				{
					foreach (IdeoSymbolPack ideoSymbolPack2 in memeDef.symbolPacks)
					{
						if (!ideoSymbolPack2.theme.NullOrEmpty())
						{
							request.Rules.Add(new Rule_String("memePackTheme", this.MatchIdeoNameCapitalization(ideoSymbolPack2.theme, ideoName)));
						}
						if (!ideoSymbolPack2.adjective.NullOrEmpty())
						{
							request.Rules.Add(new Rule_String("memePackAdjective", this.MatchIdeoNameCapitalization(ideoSymbolPack2.adjective, ideoName)));
						}
					}
				}
			}
			if (!this.ideo.adjective.NullOrEmpty())
			{
				request.Rules.Add(new Rule_String("chosenAdjective", this.ideo.adjective));
			}
			if (this.ideo.KeyDeityName != null)
			{
				request.Rules.Add(new Rule_String("keyDeity", this.ideo.KeyDeityName));
			}
			if (!this.ideo.leaderTitleMale.NullOrEmpty())
			{
				request.Rules.Add(new Rule_String("leaderTitle", this.ideo.leaderTitleMale));
			}
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x001F7188 File Offset: 0x001F5388
		private bool TryGuessChosenSymbolPack(out IdeoSymbolPack result)
		{
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (memeDef.symbolPacks != null)
				{
					foreach (IdeoSymbolPack ideoSymbolPack in memeDef.symbolPacks)
					{
						if (ideoSymbolPack.adjective == this.ideo.adjective || ideoSymbolPack.member == this.ideo.memberName || (ideoSymbolPack.ideoName != null && this.ideo.name.Contains(ideoSymbolPack.ideoName)))
						{
							result = ideoSymbolPack;
							return true;
						}
					}
				}
			}
			result = new IdeoSymbolPack();
			return false;
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x001F7288 File Offset: 0x001F5488
		private string MatchIdeoNameCapitalization(string value, string ideoName)
		{
			if (value.Equals(ideoName, StringComparison.InvariantCultureIgnoreCase))
			{
				return ideoName;
			}
			return value;
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x001F7298 File Offset: 0x001F5498
		public virtual void DrawPreceptBox(Rect preceptBox, IdeoEditMode editMode, bool forceHighlight = false)
		{
			GUI.color = Color.white;
			Widgets.DrawRectFast(preceptBox, IdeoUIUtility.GetBackgroundColor(this.def.impact), null);
			if (Mouse.IsOver(preceptBox) || forceHighlight)
			{
				Widgets.DrawHighlight(preceptBox);
			}
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(preceptBox, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				MemeDef memeThatRequiresPrecept = this.ideo.GetMemeThatRequiresPrecept(this.def);
				bool flag = false;
				IEnumerable<FloatMenuOption> enumerable = this.EditFloatMenuOptions();
				if (enumerable != null)
				{
					foreach (FloatMenuOption floatMenuOption in enumerable)
					{
						flag = true;
						floatMenuOption.orderInPriority = 2000;
						list.Add(floatMenuOption);
					}
				}
				FloatMenuOption floatMenuOption2 = null;
				if (this.def.canRemoveInUI)
				{
					floatMenuOption2 = new FloatMenuOption("Remove".Translate().CapitalizeFirst(), delegate()
					{
						this.ideo.RemovePrecept(this, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 1900);
					MemeDef memeThatRequiresPrecept2 = this.ideo.GetMemeThatRequiresPrecept(this.def);
					if (editMode != IdeoEditMode.Dev && memeThatRequiresPrecept2 != null)
					{
						floatMenuOption2.action = null;
						floatMenuOption2.Label = "CannotRemove".Translate() + ": " + "RequiredByMeme".Translate(memeThatRequiresPrecept2.label);
					}
					else if (editMode != IdeoEditMode.Dev && this.def.issue.HasDefaultPrecept)
					{
						floatMenuOption2.action = null;
						floatMenuOption2.Label = "CannotRemove".Translate() + ": " + "Required".Translate();
					}
					else
					{
						flag = true;
					}
				}
				if (this.CanRegenerate)
				{
					FactionDef faction = (Find.World == null) ? Find.Scenario.playerFaction.factionDef : null;
					list.Add(new FloatMenuOption("Regenerate".Translate().CapitalizeFirst(), delegate()
					{
						this.Regenerate(this.ideo, faction);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 1800));
				}
				else if (base.GetType() == typeof(Precept))
				{
					using (IEnumerator<PreceptDef> enumerator2 = (from x in DefDatabase<PreceptDef>.AllDefs
					where x != this.def && x.issue == this.def.issue
					select x).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PreceptDef p = enumerator2.Current;
							AcceptanceReport report = IdeoUIUtility.CanListPrecept(this.ideo, p, editMode);
							if (report || !string.IsNullOrWhiteSpace(report.Reason))
							{
								TaggedString taggedString = p.LabelCap;
								Action action = delegate()
								{
									this.ideo.AddPrecept(PreceptMaker.MakePrecept(p), true, null, null);
									this.ideo.RemovePrecept(this, true);
								};
								if (!report)
								{
									action = null;
									taggedString += " (" + report.Reason + ")";
								}
								FloatMenuOption floatMenuOption3 = new FloatMenuOption(taggedString, action, p.issue.Icon, IdeoUIUtility.GetIconAndLabelColor(p.impact), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
								floatMenuOption3.orderInPriority = p.displayOrderInIssue;
								flag = true;
								list.Add(floatMenuOption3);
							}
						}
					}
				}
				if (flag)
				{
					if (floatMenuOption2 != null)
					{
						list.Add(floatMenuOption2);
					}
				}
				else if (memeThatRequiresPrecept == null)
				{
					list.Add(new FloatMenuOption("CannotEdit".Translate() + ": " + "Required".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					list.Add(new FloatMenuOption("CannotEdit".Translate() + ": " + "RequiredByMeme".Translate(memeThatRequiresPrecept.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EditingPrecepts, KnowledgeAmount.Total);
				Find.WindowStack.Add(new FloatMenu(list));
			}
			GUI.color = IdeoUIUtility.GetIconAndLabelColor(this.def.impact);
			Rect rect = new Rect(preceptBox.x + (preceptBox.height - 50f) / 2f, preceptBox.y + (preceptBox.height - 50f) / 2f, 50f, 50f);
			this.DrawIcon(rect);
			Rect rect2 = new Rect(rect.xMax + 10f, preceptBox.y + 3f, preceptBox.xMax - rect.xMax - 10f, preceptBox.height / 2f);
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = this.LabelColor;
			Widgets.Label(rect2, this.UIInfoFirstLine);
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
			GUI.color = IdeoUIUtility.GetIconAndLabelColor(this.def.impact);
			Rect rect3 = new Rect(rect.xMax + 10f, preceptBox.y + preceptBox.height / 2f - 3f, preceptBox.xMax - rect.xMax - 10f, preceptBox.height / 2f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.LabelFit(rect3, this.UIInfoSecondLine);
			GenUI.ResetLabelAlign();
			bool flag2;
			this.PostDrawBox(preceptBox, out flag2);
			if (!flag2 && Mouse.IsOver(preceptBox) && Find.WindowStack.WindowOfType<FloatMenu>() == null)
			{
				TooltipHandler.TipRegion(preceptBox, this.TipLabel.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.GetTip() + ((editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
			}
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x001F78C4 File Offset: 0x001F5AC4
		public virtual void DrawIcon(Rect rect)
		{
			GUI.DrawTexture(rect, this.def.issue.Icon);
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CompatibleWith(Precept other)
		{
			return true;
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x0009511A File Offset: 0x0009331A
		protected virtual void PostDrawBox(Rect rect, out bool anyIconTooltipActive)
		{
			anyIconTooltipActive = false;
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x001F78DC File Offset: 0x001F5ADC
		public FloatMenuOption EditFloatMenuOption()
		{
			return new FloatMenuOption("Edit".Translate() + "...", delegate()
			{
				Find.WindowStack.Add(new Dialog_EditPrecept(this));
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_IdeoNotPrimaryAnymore(Ideo newIdeo)
		{
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberSpawned(Pawn pawn)
		{
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberLost(Pawn pawn)
		{
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_GameStarted()
		{
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x001F7920 File Offset: 0x001F5B20
		public virtual void Notify_MemberGenerated(Pawn pawn)
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				this.def.comps[i].Notify_MemberGenerated(pawn, this);
			}
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_RecachedPrecepts()
		{
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x001F7960 File Offset: 0x001F5B60
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Defs.Look<PreceptDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.randomSeed, "randomSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.usesDefiniteArticle, "usesDefiniteArticle", false, false);
			Scribe_Values.Look<string>(ref this.descOverride, "descOverride", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ID == -1)
			{
				this.ID = Find.UniqueIDsManager.GetNextPreceptID();
			}
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberDied(Pawn p)
		{
		}

		// Token: 0x06005AFD RID: 23293 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberCorpseDestroyed(Pawn p)
		{
		}

		// Token: 0x06005AFE RID: 23294 RVA: 0x001F79F8 File Offset: 0x001F5BF8
		public string GetUniqueLoadID()
		{
			return "Precept_" + this.ID;
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x001F7A3C File Offset: 0x001F5C3C
		[CompilerGenerated]
		internal static float <GenerateNewName>g__NameUniqueness|54_0(string newName)
		{
			string[] array = newName.Split(new char[]
			{
				' '
			});
			int num = 0;
			int num2 = 0;
			foreach (string text in array)
			{
				if (text.Length >= 4)
				{
					num2++;
					if (Precept.tmpUsedWords.Contains(text))
					{
						num++;
					}
				}
			}
			if (num2 != 0)
			{
				return 1f - (float)num / (float)num2;
			}
			return 1f;
		}

		// Token: 0x0400351C RID: 13596
		public Ideo ideo;

		// Token: 0x0400351D RID: 13597
		public PreceptDef def;

		// Token: 0x0400351E RID: 13598
		protected string name;

		// Token: 0x0400351F RID: 13599
		private int ID = -1;

		// Token: 0x04003520 RID: 13600
		public bool usesDefiniteArticle = true;

		// Token: 0x04003521 RID: 13601
		public string descOverride;

		// Token: 0x04003522 RID: 13602
		public int randomSeed;

		// Token: 0x04003523 RID: 13603
		protected string labelCapCache;

		// Token: 0x04003524 RID: 13604
		protected string tipCached;

		// Token: 0x04003525 RID: 13605
		public const int FloatMenuEditOrder = 2000;

		// Token: 0x04003526 RID: 13606
		public const int FloatMenuRemoveOrder = 1900;

		// Token: 0x04003527 RID: 13607
		public const int FloatMenuRegenerateOrder = 1800;

		// Token: 0x04003528 RID: 13608
		protected static StringBuilder tmpCompsDesc = new StringBuilder();

		// Token: 0x04003529 RID: 13609
		private static HashSet<string> tmpUsedWords = new HashSet<string>();
	}
}
