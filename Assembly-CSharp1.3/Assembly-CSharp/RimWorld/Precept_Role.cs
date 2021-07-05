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
	// Token: 0x02000FCD RID: 4045
	public abstract class Precept_Role : Precept
	{
		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06005F2B RID: 24363 RVA: 0x00208D08 File Offset: 0x00206F08
		public bool Active
		{
			get
			{
				return this.active || DebugSettings.activateAllIdeoRoles;
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06005F2C RID: 24364 RVA: 0x001F654F File Offset: 0x001F474F
		public override string UIInfoFirstLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06005F2D RID: 24365 RVA: 0x001F9183 File Offset: 0x001F7383
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06005F2E RID: 24366 RVA: 0x00208D19 File Offset: 0x00206F19
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.LabelCap;
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06005F2F RID: 24367 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06005F30 RID: 24368 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06005F31 RID: 24369 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06005F32 RID: 24370 RVA: 0x00208D45 File Offset: 0x00206F45
		// (set) Token: 0x06005F33 RID: 24371 RVA: 0x00208D4D File Offset: 0x00206F4D
		public override List<PreceptApparelRequirement> ApparelRequirements
		{
			get
			{
				return this.apparelRequirements;
			}
			set
			{
				this.tipCached = null;
				this.allApparelRequirementLabelsCached.Clear();
				this.apparelRequirements = value;
			}
		}

		// Token: 0x06005F34 RID: 24372 RVA: 0x00208D68 File Offset: 0x00206F68
		public List<string> AllApparelRequirementLabels(Gender gender, Pawn forPawn = null)
		{
			List<string> list;
			if (!this.allApparelRequirementLabelsCached.TryGetValue(gender, out list))
			{
				list = new List<string>();
				if (this.apparelRequirements != null)
				{
					for (int i = 0; i < this.apparelRequirements.Count; i++)
					{
						ApparelRequirement requirement = this.apparelRequirements[i].requirement;
						if (!requirement.groupLabel.NullOrEmpty())
						{
							list.Add(requirement.groupLabel);
						}
						else
						{
							list.Add(requirement.AllRequiredApparel(Gender.Male).First<ThingDef>().LabelCap.Resolve());
						}
					}
				}
				this.allApparelRequirementLabelsCached[gender] = list;
			}
			if (this.apparelRequirements != null)
			{
				bool flag = false;
				for (int j = 0; j < this.apparelRequirements.Count; j++)
				{
					ApparelRequirement requirement2 = this.apparelRequirements[j].requirement;
					string t;
					if (forPawn != null && !ApparelUtility.IsRequirementActive(requirement2, ApparelRequirementSource.Role, forPawn, out t))
					{
						if (!flag)
						{
							list = new List<string>(list);
							flag = true;
						}
						string text = list[j];
						text += " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
						list[j] = text;
					}
				}
			}
			return list;
		}

		// Token: 0x06005F35 RID: 24373
		public abstract IEnumerable<Pawn> ChosenPawns();

		// Token: 0x06005F36 RID: 24374
		public abstract Pawn ChosenPawnSingle();

		// Token: 0x06005F37 RID: 24375 RVA: 0x00208EB4 File Offset: 0x002070B4
		public string LabelForPawn(Pawn p)
		{
			if (!this.def.leaderRole)
			{
				return this.name;
			}
			if (p.gender != Gender.Female || this.ideo.leaderTitleFemale.NullOrEmpty())
			{
				return this.ideo.leaderTitleMale;
			}
			return this.ideo.leaderTitleFemale;
		}

		// Token: 0x06005F38 RID: 24376
		public abstract bool IsAssigned(Pawn p);

		// Token: 0x06005F39 RID: 24377
		public abstract void Unassign(Pawn p, bool generateThoughts);

		// Token: 0x06005F3A RID: 24378 RVA: 0x00208F07 File Offset: 0x00207107
		public void RegenerateApparelRequirements(FactionDef generatingFor = null)
		{
			this.apparelRequirements = this.GenerateNewApparelRequirements(generatingFor);
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x00208F18 File Offset: 0x00207118
		public override List<PreceptApparelRequirement> GenerateNewApparelRequirements(FactionDef generatingFor = null)
		{
			Precept_Role.<>c__DisplayClass29_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.generatingFor = generatingFor;
			CS$<>8__locals1.apparelRequirementCount = ((this.def.roleApparelRequirementCountCurve != null) ? Mathf.CeilToInt(this.def.roleApparelRequirementCountCurve.Evaluate(Rand.Value)) : 0);
			if (CS$<>8__locals1.apparelRequirementCount == 0)
			{
				return null;
			}
			CS$<>8__locals1.bodypartsAlreadyCovered = new List<BodyPartGroupDef>();
			CS$<>8__locals1.apparelRequirements = new List<PreceptApparelRequirement>();
			List<PreceptApparelRequirement> list = new List<PreceptApparelRequirement>();
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (!memeDef.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					list.AddRange(memeDef.apparelRequirements);
				}
			}
			IEnumerable<PreceptApparelRequirement> collection = from apparelPrecept in this.ideo.PreceptsListForReading.OfType<Precept_Apparel>()
			select new PreceptApparelRequirement
			{
				requirement = new ApparelRequirement
				{
					bodyPartGroupsMatchAny = apparelPrecept.apparelDef.apparel.bodyPartGroups,
					requiredDefs = new List<ThingDef>
					{
						apparelPrecept.apparelDef
					}
				}
			};
			list.AddRange(collection);
			if (this.def.roleApparelRequirements != null)
			{
				list.AddRange(this.def.roleApparelRequirements);
			}
			List<PreceptApparelRequirement> list2 = new List<PreceptApparelRequirement>(list);
			using (List<PreceptApparelRequirement>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					PreceptApparelRequirement req = enumerator2.Current;
					Predicate<PreceptApparelRequirement> <>9__2;
					foreach (Precept_Role precept_Role in this.ideo.RolesListForReading)
					{
						if (precept_Role.apparelRequirements != null)
						{
							List<PreceptApparelRequirement> list3 = precept_Role.apparelRequirements;
							Predicate<PreceptApparelRequirement> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((PreceptApparelRequirement o) => o.requirement.SameApparelAs(req.requirement)));
							}
							if (list3.Any(predicate))
							{
								list2.Remove(req);
							}
						}
					}
				}
			}
			this.<GenerateNewApparelRequirements>g__ChooseApparelRequirements|29_0(list2, ref CS$<>8__locals1);
			if (CS$<>8__locals1.apparelRequirements.Count < CS$<>8__locals1.apparelRequirementCount)
			{
				this.<GenerateNewApparelRequirements>g__ChooseApparelRequirements|29_0(CS$<>8__locals1.apparelRequirements, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.apparelRequirements;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x00209158 File Offset: 0x00207358
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			if (!ModLister.CheckIdeology("Ideology role"))
			{
				return;
			}
			base.RegenerateName();
			this.apparelRequirements = this.GenerateNewApparelRequirements(generatingFor);
			this.allApparelRequirementLabelsCached.Clear();
			this.restrictToSupremeGender = (Rand.Value < this.def.restrictToSupremeGenderChance);
			this.FillOrUpdateAbilities();
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x002091B8 File Offset: 0x002073B8
		public override string GenerateNameRaw()
		{
			if (!this.def.leaderRole)
			{
				GrammarRequest request = default(GrammarRequest);
				request.Includes.Add(this.def.nameMaker);
				base.AddIdeoRulesTo(ref request);
				return GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_roleName", request, null, false, null, null, null, false));
			}
			this.ideo.foundation.GenerateLeaderTitle();
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				precept.ClearTipCache();
			}
			return this.ideo.leaderTitleMale;
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x00209274 File Offset: 0x00207474
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Clear();
				if (!this.def.description.NullOrEmpty())
				{
					Precept.tmpCompsDesc.Append(this.def.description);
				}
				if (!this.def.leaderRole && this.def.activationBelieverCount != -1)
				{
					Precept.tmpCompsDesc.AppendLine();
					string value = (this.def.activationBelieverCount > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, -1) : this.ideo.memberName;
					string value2 = (this.def.deactivationBelieverCount > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, -1) : this.ideo.memberName;
					Precept.tmpCompsDesc.AppendInNewLine("RoleBelieverCountDesc".Translate(value, this.def.activationBelieverCount, value2, this.def.deactivationBelieverCount).Resolve());
				}
				if (!this.def.grantedAbilities.NullOrEmpty<AbilityDef>())
				{
					List<string> list = new List<string>();
					int i = 0;
					while (i < this.def.grantedAbilities.Count)
					{
						CompProperties_AbilityStartRitual compProperties_AbilityStartRitual = this.def.grantedAbilities[i].comps.FirstOrDefault((AbilityCompProperties comp) => comp is CompProperties_AbilityStartRitual) as CompProperties_AbilityStartRitual;
						if (compProperties_AbilityStartRitual != null)
						{
							using (List<Precept>.Enumerator enumerator = this.ideo.PreceptsListForReading.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Precept_Ritual precept_Ritual;
									if ((precept_Ritual = (enumerator.Current as Precept_Ritual)) != null && precept_Ritual.def == compProperties_AbilityStartRitual.ritualDef)
									{
										list.Add(precept_Ritual.LabelCap);
										break;
									}
								}
								goto IL_20B;
							}
							goto IL_1E4;
						}
						goto IL_1E4;
						IL_20B:
						i++;
						continue;
						IL_1E4:
						list.Add(this.def.grantedAbilities[i].LabelCap.Resolve());
						goto IL_20B;
					}
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleGrantedAbilitiesLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list.ToLineList("  - "));
				}
				List<string> list2 = new List<string>();
				foreach (Precept precept in this.ideo.PreceptsListForReading)
				{
					Precept_Ritual precept_Ritual2;
					if ((precept_Ritual2 = (precept as Precept_Ritual)) != null && precept.def.listedForRoles)
					{
						bool flag;
						if (precept_Ritual2 == null)
						{
							flag = (null != null);
						}
						else
						{
							RitualBehaviorWorker behavior = precept_Ritual2.behavior;
							flag = (((behavior != null) ? behavior.def.stages : null) != null);
						}
						if (flag && precept_Ritual2.behavior.def.roles != null && !list2.Contains(precept_Ritual2.LabelCap) && precept_Ritual2.behavior.def.roles.Any(delegate(RitualRole r)
						{
							string text2;
							return r.AppliesToRole(this, out text2, null, null);
						}))
						{
							list2.Add(precept_Ritual2.LabelCap);
						}
					}
				}
				if (list2.Count > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRitualsLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list2.ToLineList("  - "));
				}
				if (!this.def.roleRequirements.NullOrEmpty<RoleRequirement>())
				{
					List<string> list3 = new List<string>();
					for (int j = 0; j < this.def.roleRequirements.Count; j++)
					{
						string text = this.def.roleRequirements[j].GetLabelCap(this).ResolveTags();
						if (!text.NullOrEmpty())
						{
							list3.Add(text);
						}
					}
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRequirementsLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list3.ToLineList("  - "));
				}
				if (!this.def.roleEffects.NullOrEmpty<RoleEffect>())
				{
					List<string> list4 = new List<string>();
					foreach (RoleEffect roleEffect in this.def.roleEffects.OrderBy(delegate(RoleEffect r)
					{
						if (!r.IsBad)
						{
							return 0;
						}
						return 1;
					}))
					{
						list4.Add(roleEffect.Label(null, this));
					}
					if (list4.Count > 0)
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleEffectsLabel".Translate().Resolve() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine(list4.ToLineList("  - "));
					}
				}
				List<string> list5 = this.AllApparelRequirementLabels((this.ChosenPawnSingle() != null) ? this.ChosenPawnSingle().gender : Gender.Male, this.ChosenPawnSingle());
				if (list5.Count > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRequiredApparelLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list5.ToLineList("  - "));
				}
				if (this.def.expectationsOffset != 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Expectations".Translate().CapitalizeFirst().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine("  - " + "RoleExpectationOffset".Translate(this.def.expectationsOffset).Resolve());
				}
				if (this.def.roleDisabledWorkTags != WorkTags.None)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleDisabledWorkLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine("  - " + (from w in this.DisabledWorkTypes
					select w.labelShort.UncapitalizeFirst()).ToCommaList(false, false).CapitalizeFirst());
				}
				if (!this.def.requiredMemes.NullOrEmpty<MemeDef>())
				{
					List<MemeDef> requiredMemes = this.def.requiredMemes;
					IEnumerable<MemeDef> source = (requiredMemes != null) ? (from m in requiredMemes
					where this.ideo.memes.Contains(m)
					select m) : null;
					if (source.Any<MemeDef>())
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("UnlockedByMeme".Translate().Resolve() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine((from m in source
						select m.LabelCap.Resolve()).ToLineList("  - ", false));
					}
				}
				this.tipCached = Precept.tmpCompsDesc.ToString();
			}
			return this.tipCached;
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x00209A64 File Offset: 0x00207C64
		public override void Tick()
		{
			base.Tick();
			this.RecacheActivity();
		}

		// Token: 0x06005F40 RID: 24384
		public abstract void RecacheActivity();

		// Token: 0x06005F41 RID: 24385
		public abstract void Assign(Pawn p, bool addThoughts);

		// Token: 0x06005F42 RID: 24386 RVA: 0x00209A72 File Offset: 0x00207C72
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x06005F43 RID: 24387
		public abstract void FillOrUpdateAbilities();

		// Token: 0x06005F44 RID: 24388 RVA: 0x00209A84 File Offset: 0x00207C84
		protected List<Ability> FillOrUpdateAbilityList(Pawn forPawn, List<Ability> abilities)
		{
			if (this.def.grantedAbilities == null)
			{
				return null;
			}
			if (abilities != null)
			{
				abilities.RemoveAll((Ability a) => a == null || a.def == null);
				int num = 0;
				for (int i = 0; i < abilities.Count; i++)
				{
					if (this.def.grantedAbilities.Contains(abilities[i].def))
					{
						num++;
					}
				}
				if (num != this.def.grantedAbilities.Count)
				{
					abilities = null;
				}
			}
			if (abilities == null)
			{
				abilities = new List<Ability>();
				using (List<AbilityDef>.Enumerator enumerator = this.def.grantedAbilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityDef def = enumerator.Current;
						abilities.Add(AbilityUtility.MakeAbility(def, forPawn, this));
					}
					return abilities;
				}
			}
			foreach (Ability ability in abilities)
			{
				ability.pawn = forPawn;
				ability.verb.caster = forPawn;
				ability.sourcePrecept = this;
			}
			return abilities;
		}

		// Token: 0x06005F45 RID: 24389
		public abstract List<Ability> AbilitiesFor(Pawn p);

		// Token: 0x06005F46 RID: 24390 RVA: 0x00209BC0 File Offset: 0x00207DC0
		public void Notify_PawnUnassigned(Pawn oldPawn)
		{
			if (oldPawn != null)
			{
				Pawn_AbilityTracker abilities = oldPawn.abilities;
				if (abilities != null)
				{
					abilities.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (oldPawn != null)
			{
				oldPawn.Notify_DisabledWorkTypesChanged();
			}
			if (oldPawn != null)
			{
				Pawn_ApparelTracker apparel = oldPawn.apparel;
				if (apparel != null)
				{
					apparel.Notify_RoleChanged();
				}
			}
			if (oldPawn != null && oldPawn.IsPrisoner)
			{
				Messages.Message("MessageRoleUnassignedPrisoner".Translate(oldPawn, this.LabelForPawn(oldPawn)), oldPawn, MessageTypeDefOf.SilentInput, false);
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x00209C48 File Offset: 0x00207E48
		public void Notify_PawnAssigned(Pawn newPawn)
		{
			if (newPawn != null)
			{
				Pawn_AbilityTracker abilities = newPawn.abilities;
				if (abilities != null)
				{
					abilities.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (newPawn != null)
			{
				newPawn.Notify_DisabledWorkTypesChanged();
			}
			if (newPawn != null)
			{
				Pawn_ApparelTracker apparel = newPawn.apparel;
				if (apparel != null)
				{
					apparel.Notify_RoleChanged();
				}
			}
			if (newPawn != null)
			{
				Messages.Message("MessageRoleAssigned".Translate(newPawn, this.LabelForPawn(newPawn)), newPawn, MessageTypeDefOf.SilentInput, false);
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
			}
			this.FillOrUpdateAbilities();
		}

		// Token: 0x06005F48 RID: 24392 RVA: 0x00209CCC File Offset: 0x00207ECC
		public RoleRequirement GetFirstUnmetRequirement(Pawn p)
		{
			foreach (RoleRequirement roleRequirement in this.def.roleRequirements)
			{
				if (!roleRequirement.Met(p, this))
				{
					return roleRequirement;
				}
			}
			return null;
		}

		// Token: 0x06005F49 RID: 24393 RVA: 0x00209D30 File Offset: 0x00207F30
		public bool RequirementsMet(Pawn p)
		{
			using (List<RoleRequirement>.Enumerator enumerator = this.def.roleRequirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Met(p, this))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005F4A RID: 24394 RVA: 0x001FA8F1 File Offset: 0x001F8AF1
		public override void DrawIcon(Rect rect)
		{
			GUI.color = this.ideo.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = Color.white;
		}

		// Token: 0x06005F4B RID: 24395 RVA: 0x00209D90 File Offset: 0x00207F90
		protected override void PostDrawBox(Rect rect, out bool anyTooltipActive)
		{
			bool localAnyTooltipActive = false;
			if (!this.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
			{
				List<ThingDef> list = this.apparelRequirements.SelectMany((PreceptApparelRequirement ar) => ar.requirement.AllRequiredApparel(Gender.None)).Distinct<ThingDef>().ToList<ThingDef>();
				int num = list.Count * 24;
				num += (list.Count - 1) * 2;
				Rect rect2 = new Rect(rect.xMax - (float)num, rect.yMin, (float)num, 24f);
				float xMin = rect2.xMin;
				GenUI.DrawElementStack<ThingDef>(rect2, 24f, list, delegate(Rect r, ThingDef apparel)
				{
					if (Mouse.IsOver(r))
					{
						localAnyTooltipActive = true;
						TooltipHandler.TipRegion(r, "RoleRequiredApparelLabel".Translate() + ": " + apparel.LabelCap + "\n\n" + apparel.DescriptionDetailed);
					}
					GUI.DrawTexture(r, apparel.uiIcon, ScaleMode.ScaleToFit, true, 0f, apparel.uiIconColor, 0f, 0f);
				}, (ThingDef apparel) => 24f, 0f, 2f, true);
			}
			anyTooltipActive = localAnyTooltipActive;
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x00209E78 File Offset: 0x00208078
		public override IEnumerable<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			if (!this.Active || !pawn.IsFreeNonSlaveColonist || pawn.IsQuestLodger())
			{
				yield break;
			}
			if (!this.def.leaderRole && this.def.createsRoleEmptyThought && !this.ChosenPawns().Any<Pawn>() && !activeThoughts.Any(delegate(Thought_Situational t)
			{
				Thought_IdeoRoleEmpty thought_IdeoRoleEmpty2;
				return (thought_IdeoRoleEmpty2 = (t as Thought_IdeoRoleEmpty)) != null && thought_IdeoRoleEmpty2.Role == this;
			}))
			{
				Thought_IdeoRoleEmpty thought_IdeoRoleEmpty = (Thought_IdeoRoleEmpty)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleEmpty);
				if (thought_IdeoRoleEmpty != null)
				{
					thought_IdeoRoleEmpty.pawn = pawn;
					thought_IdeoRoleEmpty.sourcePrecept = this;
					yield return thought_IdeoRoleEmpty;
				}
			}
			if (this.IsAssigned(pawn) && this.apparelRequirements != null && this.apparelRequirements.Count > 0 && !activeThoughts.Any(delegate(Thought_Situational t)
			{
				Thought_IdeoRoleApparelRequirementNotMet thought_IdeoRoleApparelRequirementNotMet2;
				return (thought_IdeoRoleApparelRequirementNotMet2 = (t as Thought_IdeoRoleApparelRequirementNotMet)) != null && thought_IdeoRoleApparelRequirementNotMet2.Role == this;
			}))
			{
				Thought_IdeoRoleApparelRequirementNotMet thought_IdeoRoleApparelRequirementNotMet = (Thought_IdeoRoleApparelRequirementNotMet)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleApparelRequirementNotMet);
				if (thought_IdeoRoleApparelRequirementNotMet != null)
				{
					thought_IdeoRoleApparelRequirementNotMet.pawn = pawn;
					thought_IdeoRoleApparelRequirementNotMet.sourcePrecept = this;
					yield return thought_IdeoRoleApparelRequirementNotMet;
				}
			}
			yield break;
		}

		// Token: 0x06005F4D RID: 24397 RVA: 0x00209E96 File Offset: 0x00208096
		protected bool ValidatePawn(Pawn p)
		{
			return p.Faction != null && (!p.Faction.IsPlayer || p.IsFreeNonSlaveColonist) && !p.Destroyed && !p.Dead && this.RequirementsMet(p);
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06005F4E RID: 24398 RVA: 0x00209ED5 File Offset: 0x002080D5
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.def.roleDisabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x00209EE8 File Offset: 0x002080E8
		public bool CanEquip(Pawn pawn, Thing thing, out string reason)
		{
			if (this.def.roleEffects != null)
			{
				foreach (RoleEffect roleEffect in this.def.roleEffects)
				{
					if (!roleEffect.CanEquip(pawn, thing))
					{
						reason = roleEffect.Label(pawn, this);
						return false;
					}
				}
			}
			reason = null;
			return true;
		}

		// Token: 0x06005F50 RID: 24400 RVA: 0x00209F64 File Offset: 0x00208164
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Collections.Look<PreceptApparelRequirement>(ref this.apparelRequirements, "apparelRequirements", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.restrictToSupremeGender, "restrictToSupremeGender", false, false);
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x00209FC4 File Offset: 0x002081C4
		[CompilerGenerated]
		private void <GenerateNewApparelRequirements>g__ChooseApparelRequirements|29_0(List<PreceptApparelRequirement> source, ref Precept_Role.<>c__DisplayClass29_0 A_2)
		{
			if (A_2.apparelRequirements.Count >= A_2.apparelRequirementCount)
			{
				return;
			}
			List<PreceptApparelRequirement> list = new List<PreceptApparelRequirement>(source);
			list.Shuffle<PreceptApparelRequirement>();
			for (int i = 0; i < list.Count; i++)
			{
				string text;
				if (list[i].CanAddRequirement(this, A_2.apparelRequirements, out text, A_2.generatingFor))
				{
					A_2.apparelRequirements.Add(list[i]);
					A_2.bodypartsAlreadyCovered.AddRange(list[i].requirement.bodyPartGroupsMatchAny);
					if (A_2.apparelRequirements.Count >= A_2.apparelRequirementCount)
					{
						break;
					}
				}
			}
		}

		// Token: 0x040036D4 RID: 14036
		protected bool active;

		// Token: 0x040036D5 RID: 14037
		public bool restrictToSupremeGender;

		// Token: 0x040036D6 RID: 14038
		protected const string BulletPointString = "  - ";

		// Token: 0x040036D7 RID: 14039
		public List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x040036D8 RID: 14040
		private Dictionary<Gender, List<string>> allApparelRequirementLabelsCached = new Dictionary<Gender, List<string>>();
	}
}
