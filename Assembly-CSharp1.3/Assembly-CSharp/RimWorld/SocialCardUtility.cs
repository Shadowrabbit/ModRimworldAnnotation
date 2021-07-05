using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200130F RID: 4879
	[StaticConstructorOnStartup]
	public static class SocialCardUtility
	{
		// Token: 0x06007554 RID: 30036 RVA: 0x00285134 File Offset: 0x00283334
		public static void DrawSocialCard(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			float num = Prefs.DevMode ? 20f : 15f;
			Rect rect2 = new Rect(0f, num, rect.width, rect.height - num).ContractedBy(10f);
			Rect rect3 = new Rect(0f, 5f, rect.width, 40f);
			Rect rect4 = new Rect(0f, 40f, rect.width, 40f);
			Rect rect5 = rect2;
			Rect rect6 = rect2;
			if (ModsConfig.IdeologyActive && !pawn.Dead && pawn.Ideo != null)
			{
				rect5.yMin += 40f;
				SocialCardUtility.DrawPawnCertainty(pawn, rect3);
				rect5.yMin += 45f;
				SocialCardUtility.DrawPawnRole(pawn, rect4);
			}
			rect5.height *= 0.63f;
			rect6.y = rect5.yMax + 17f;
			rect6.yMax = rect2.yMax;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, (rect5.yMax + rect6.y) / 2f, rect.width);
			GUI.color = Color.white;
			if (Prefs.DevMode && !pawn.Dead)
			{
				SocialCardUtility.DrawDebugOptions(new Rect(5f, rect5.yMin - 20f, rect.width, 22f), pawn);
			}
			SocialCardUtility.DrawRelationsAndOpinions(rect5, pawn);
			InteractionCardUtility.DrawInteractionsLog(rect6, pawn, Find.PlayLog.AllEntries, 12);
			GUI.EndGroup();
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x002852EE File Offset: 0x002834EE
		private static void CheckRecache(Pawn selPawnForSocialInfo)
		{
			if (SocialCardUtility.cachedForPawn != selPawnForSocialInfo || Time.frameCount % 20 == 0)
			{
				SocialCardUtility.Recache(selPawnForSocialInfo);
			}
		}

		// Token: 0x06007556 RID: 30038 RVA: 0x00285308 File Offset: 0x00283508
		private static void Recache(Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.cachedForPawn = selPawnForSocialInfo;
			SocialCardUtility.tmpToCache.Clear();
			foreach (Pawn pawn in selPawnForSocialInfo.relations.RelatedPawns)
			{
				if (SocialCardUtility.ShouldShowPawnRelations(pawn, selPawnForSocialInfo))
				{
					SocialCardUtility.RecacheEntry(pawn, selPawnForSocialInfo, null, null);
					SocialCardUtility.tmpToCache.Add(pawn);
				}
			}
			List<Pawn> list = null;
			if (selPawnForSocialInfo.MapHeld != null)
			{
				list = selPawnForSocialInfo.MapHeld.mapPawns.AllPawns;
			}
			else if (selPawnForSocialInfo.IsCaravanMember())
			{
				list = selPawnForSocialInfo.GetCaravan().PawnsListForReading;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn2 = list[i];
					if (pawn2.RaceProps.Humanlike && pawn2 != selPawnForSocialInfo && SocialCardUtility.ShouldShowPawnRelations(pawn2, selPawnForSocialInfo) && !SocialCardUtility.tmpToCache.Contains(pawn2) && (pawn2.relations.OpinionOf(selPawnForSocialInfo) != 0 || selPawnForSocialInfo.relations.OpinionOf(pawn2) != 0))
					{
						SocialCardUtility.RecacheEntry(pawn2, selPawnForSocialInfo, null, null);
						SocialCardUtility.tmpToCache.Add(pawn2);
					}
				}
			}
			SocialCardUtility.cachedEntries.RemoveAll((SocialCardUtility.CachedSocialTabEntry x) => !SocialCardUtility.tmpToCache.Contains(x.otherPawn));
			SocialCardUtility.cachedEntries.Sort(SocialCardUtility.CachedEntriesComparer);
			SocialCardUtility.cachedRoles.Clear();
			if (selPawnForSocialInfo.Ideo != null)
			{
				SocialCardUtility.cachedRoles.AddRange(from r in selPawnForSocialInfo.Ideo.RolesListForReading
				where !r.def.leaderRole
				select r);
				Precept_Role precept_Role = Faction.OfPlayer.ideos.PrimaryIdeo.RolesListForReading.FirstOrDefault((Precept_Role p) => p.def.leaderRole);
				if (precept_Role != null)
				{
					SocialCardUtility.cachedRoles.Add(precept_Role);
				}
				SocialCardUtility.cachedRoles.SortBy((Precept_Role x) => x.def.displayOrderInImpact);
			}
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x00285558 File Offset: 0x00283758
		private static bool ShouldShowPawnRelations(Pawn pawn, Pawn selPawnForSocialInfo)
		{
			return SocialCardUtility.showAllRelations || ((!pawn.RaceProps.Animal || !pawn.Dead || pawn.Corpse != null) && pawn.Name != null && !pawn.Name.Numerical && !pawn.relations.hidePawnRelations && !selPawnForSocialInfo.relations.hidePawnRelations && pawn.relations.everSeenByPlayer);
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x002855D0 File Offset: 0x002837D0
		private static void RecacheEntry(Pawn pawn, Pawn selPawnForSocialInfo, int? opinionOfMe = null, int? opinionOfOtherPawn = null)
		{
			bool flag = false;
			foreach (SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry in SocialCardUtility.cachedEntries)
			{
				if (cachedSocialTabEntry.otherPawn == pawn)
				{
					SocialCardUtility.RecacheEntryInt(cachedSocialTabEntry, selPawnForSocialInfo, opinionOfMe, opinionOfOtherPawn);
					flag = true;
				}
			}
			if (flag)
			{
				return;
			}
			SocialCardUtility.CachedSocialTabEntry cachedSocialTabEntry2 = new SocialCardUtility.CachedSocialTabEntry();
			cachedSocialTabEntry2.otherPawn = pawn;
			SocialCardUtility.RecacheEntryInt(cachedSocialTabEntry2, selPawnForSocialInfo, opinionOfMe, opinionOfOtherPawn);
			SocialCardUtility.cachedEntries.Add(cachedSocialTabEntry2);
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x00285658 File Offset: 0x00283858
		private static void RecacheEntryInt(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo, int? opinionOfMe = null, int? opinionOfOtherPawn = null)
		{
			entry.opinionOfMe = ((opinionOfMe != null) ? opinionOfMe.Value : entry.otherPawn.relations.OpinionOf(selPawnForSocialInfo));
			entry.opinionOfOtherPawn = ((opinionOfOtherPawn != null) ? opinionOfOtherPawn.Value : selPawnForSocialInfo.relations.OpinionOf(entry.otherPawn));
			entry.relations.Clear();
			foreach (PawnRelationDef item in selPawnForSocialInfo.GetRelations(entry.otherPawn))
			{
				entry.relations.Add(item);
			}
			entry.relations.Sort((PawnRelationDef a, PawnRelationDef b) => b.importance.CompareTo(a.importance));
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x00285738 File Offset: 0x00283938
		public static void DrawPawnCertainty(Pawn pawn, Rect rect)
		{
			float num = rect.x + 17f;
			GUI.color = pawn.Ideo.Color;
			Rect position = new Rect(num, rect.y + rect.height / 2f - 16f, 32f, 32f);
			GUI.DrawTexture(position, pawn.Ideo.Icon);
			GUI.color = Color.white;
			num += 42f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = new Rect(num, rect.y, rect.width / 2f - num, rect.height);
			Widgets.Label(rect2, pawn.Ideo.name.Truncate(rect2.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			num += rect2.width + 10f;
			Rect rect3 = new Rect(position.x, rect.y + rect.height / 2f - 16f, 0f, 32f);
			Rect rect4 = new Rect(num, rect.y + rect.height / 2f - 16f, rect.width - num - 26f, 32f);
			rect3.xMax = rect4.xMax;
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				string text = pawn.ideo.CertaintyChangePerDay.ToStringPercent();
				if (pawn.ideo.CertaintyChangePerDay >= 0f)
				{
					text = "+" + text;
				}
				TaggedString tip = "CertaintyInIdeo".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO")) + ": " + pawn.ideo.Certainty.ToStringPercent() + "\n\n" + "CertaintyChangePerDay".Translate() + ": " + text + "\n\n";
				string text2 = "MoodChangeRate".Translate() + ": ";
				foreach (CurvePoint curvePoint in ConversionTuning.CertaintyPerDayByMoodCurve.Points)
				{
					string text3 = curvePoint.y.ToStringPercent();
					if (curvePoint.y >= 0f)
					{
						text3 = "+" + text3;
					}
					text2 += "\n -  " + "Mood".Translate() + " " + curvePoint.x.ToStringPercent() + ": " + "PerDay".Translate(text3);
				}
				tip += text2.Colorize(Color.grey);
				float statValue = pawn.GetStatValue(StatDefOf.CertaintyLossFactor, true);
				if (statValue != 1f)
				{
					tip += "\n\n" + StatDefOf.CertaintyLossFactor.LabelCap + ": " + statValue.ToStringPercent() + "\n" + "Factors".Translate() + ":" + ConversionUtility.GetCertaintyReductionFactorsDescription(pawn).Resolve();
				}
				TooltipHandler.TipRegion(rect3, () => tip.Resolve(), 10218219);
			}
			Widgets.FillableBar(rect4.ContractedBy(4f), pawn.ideo.Certainty, SocialCardUtility.BarFullTexHor);
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x00285B24 File Offset: 0x00283D24
		public static void DrawPawnRole(Pawn pawn, Rect rect)
		{
			SocialCardUtility.<>c__DisplayClass26_0 CS$<>8__locals1 = new SocialCardUtility.<>c__DisplayClass26_0();
			CS$<>8__locals1.pawn = pawn;
			float num = 17f;
			SocialCardUtility.<>c__DisplayClass26_0 CS$<>8__locals2 = CS$<>8__locals1;
			Ideo ideo = CS$<>8__locals1.pawn.Ideo;
			CS$<>8__locals2.currentRole = ((ideo != null) ? ideo.GetRole(CS$<>8__locals1.pawn) : null);
			Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			string label = (CS$<>8__locals1.currentRole != null) ? CS$<>8__locals1.currentRole.LabelCap : "NoRoleAssigned".Translate();
			if (CS$<>8__locals1.currentRole != null)
			{
				float y = rect.y + rect.height / 2f - 16f;
				Rect outerRect = rect;
				outerRect.x = num;
				outerRect.y = y;
				outerRect.width = 32f;
				outerRect.height = 32f;
				GUI.color = CS$<>8__locals1.currentRole.ideo.Color;
				Widgets.DrawTextureFitted(outerRect, CS$<>8__locals1.currentRole.Icon, 1f);
				GUI.color = Color.white;
			}
			else
			{
				GUI.color = Color.gray;
			}
			Rect rect2 = new Rect(rect.x, rect.y + rect.height / 2f - 16f, rect.width, 32f);
			rect2.xMin = num;
			rect2.xMax = rect.width - 150f - 10f;
			num += 42f;
			Rect rect3 = rect;
			rect3.xMin = num;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (Mouse.IsOver(rect2))
			{
				string roleDesc = "RoleDesc".Translate().Resolve();
				if (CS$<>8__locals1.currentRole != null)
				{
					roleDesc = string.Concat(new string[]
					{
						roleDesc,
						"\n\n",
						CS$<>8__locals1.currentRole.LabelForPawn(CS$<>8__locals1.pawn),
						": ",
						CS$<>8__locals1.currentRole.GetTip()
					});
				}
				Widgets.DrawHighlight(rect2);
				TipSignal tip = new TipSignal(() => roleDesc, CS$<>8__locals1.pawn.thingIDNumber * 39);
				TooltipHandler.TipRegion(rect2, tip);
			}
			if (CS$<>8__locals1.pawn.IsFreeNonSlaveColonist)
			{
				bool flag = SocialCardUtility.cachedRoles.Any<Precept_Role>();
				if (!flag)
				{
					GUI.color = Color.gray;
				}
				float y2 = rect3.y + rect3.height / 2f - 14f;
				if (Widgets.ButtonText(new Rect(rect.width - 150f, y2, 115f, 28f)
				{
					xMax = rect.width - 26f - 4f
				}, "ChooseRole".Translate() + "...", true, true, flag))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					if (CS$<>8__locals1.currentRole != null)
					{
						list.Add(new FloatMenuOption("RemoveCurrentRole".Translate(), delegate()
						{
							WindowStack windowStack = Find.WindowStack;
							TaggedString text3 = "ChooseRoleConfirmUnassign".Translate(CS$<>8__locals1.currentRole.Named("ROLE"), CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
							Action confirmedAct;
							if ((confirmedAct = CS$<>8__locals1.<>9__2) == null)
							{
								confirmedAct = (CS$<>8__locals1.<>9__2 = delegate()
								{
									CS$<>8__locals1.currentRole.Unassign(CS$<>8__locals1.pawn, true);
								});
							}
							windowStack.Add(Dialog_MessageBox.CreateConfirmation(text3, confirmedAct, false, null));
						}, Widgets.PlaceholderIconTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (CS$<>8__locals1.pawn.Ideo != null)
					{
						using (List<Precept_Role>.Enumerator enumerator = SocialCardUtility.cachedRoles.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								SocialCardUtility.<>c__DisplayClass26_2 CS$<>8__locals4 = new SocialCardUtility.<>c__DisplayClass26_2();
								CS$<>8__locals4.CS$<>8__locals1 = CS$<>8__locals1;
								CS$<>8__locals4.newRole = enumerator.Current;
								if (CS$<>8__locals4.newRole != CS$<>8__locals4.CS$<>8__locals1.currentRole && CS$<>8__locals4.newRole.Active && CS$<>8__locals4.newRole.RequirementsMet(CS$<>8__locals4.CS$<>8__locals1.pawn) && (!CS$<>8__locals4.newRole.def.leaderRole || CS$<>8__locals4.CS$<>8__locals1.pawn.Ideo == primaryIdeo))
								{
									bool flag2 = false;
									TaggedString confirmText = "ChooseRoleConfirmAssign".Translate(CS$<>8__locals4.newRole.Named("ROLE"), CS$<>8__locals4.CS$<>8__locals1.pawn.Named("PAWN"));
									string text = CS$<>8__locals4.newRole.LabelForPawn(CS$<>8__locals4.CS$<>8__locals1.pawn) + " (" + CS$<>8__locals4.newRole.def.label + ")";
									if (CS$<>8__locals4.CS$<>8__locals1.currentRole != null)
									{
										confirmText += " " + "ChooseRoleConfirmAssignHasOtherRole".Translate(CS$<>8__locals4.CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals4.CS$<>8__locals1.currentRole.Named("ROLE"));
										flag2 = true;
									}
									Pawn pawn2 = CS$<>8__locals4.newRole.ChosenPawns().FirstOrDefault<Pawn>();
									if (pawn2 != null && CS$<>8__locals4.newRole is Precept_RoleSingle)
									{
										text = text + ": " + pawn2.LabelShort;
										confirmText += " " + "ChooseRoleConfirmAssignReplace".Translate(pawn2.Named("PAWN"));
										flag2 = true;
									}
									else if (CS$<>8__locals4.newRole.def.leaderRole)
									{
										foreach (Ideo ideo2 in Faction.OfPlayer.ideos.AllIdeos)
										{
											foreach (Precept precept in ideo2.PreceptsListForReading)
											{
												Precept_Role precept_Role;
												if (precept != CS$<>8__locals4.CS$<>8__locals1.currentRole && (precept_Role = (precept as Precept_Role)) != null)
												{
													Pawn pawn3 = precept_Role.ChosenPawnSingle();
													if (precept_Role.def.leaderRole && pawn3 != null && pawn3 != CS$<>8__locals4.CS$<>8__locals1.pawn && pawn3.IsFreeColonist)
													{
														confirmText += " " + "ChooseRoleConfirmAssignReplaceLeader".Translate(pawn3.Named("PAWN"), CS$<>8__locals4.newRole.Named("ROLE"), precept_Role.Named("OTHERROLE"));
														flag2 = true;
														break;
													}
												}
											}
										}
									}
									IEnumerable<WorkTypeDef> disabledWorkTypes = CS$<>8__locals4.newRole.DisabledWorkTypes;
									if (disabledWorkTypes.Any<WorkTypeDef>())
									{
										flag2 = true;
										if (!confirmText.NullOrEmpty())
										{
											confirmText += "\n\n";
										}
										confirmText += "ChooseRoleListWorkTypeRestrictions".Translate(CS$<>8__locals4.CS$<>8__locals1.pawn.Named("PAWN")) + ": \n" + (from x in disabledWorkTypes
										select x.labelShort.ToString()).ToLineList("  - ", false);
									}
									if (!CS$<>8__locals4.newRole.def.grantedAbilities.NullOrEmpty<AbilityDef>())
									{
										flag2 = true;
										if (!confirmText.NullOrEmpty())
										{
											confirmText += "\n\n";
										}
										confirmText += "ChooseRoleListAbilities".Translate(CS$<>8__locals4.CS$<>8__locals1.pawn.Named("PAWN")) + ": \n" + (from x in CS$<>8__locals4.newRole.def.grantedAbilities
										select x.LabelCap.ToString()).ToLineList("  - ", false);
									}
									if (!CS$<>8__locals4.newRole.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
									{
										flag2 = true;
										if (!confirmText.NullOrEmpty())
										{
											confirmText += "\n\n";
										}
										confirmText += "ChooseRoleListApparelDemands".Translate(CS$<>8__locals4.newRole.Named("ROLE")) + ": \n" + CS$<>8__locals4.newRole.AllApparelRequirementLabels(CS$<>8__locals4.CS$<>8__locals1.pawn.gender, CS$<>8__locals4.CS$<>8__locals1.pawn).ToLineList("  - ");
									}
									if (CS$<>8__locals4.newRole.def.roleRequiredWorkTags != WorkTags.None)
									{
										List<string> list2 = new List<string>();
										foreach (WorkTags workTags in CS$<>8__locals4.newRole.def.roleRequiredWorkTags.GetAllSelectedItems<WorkTags>())
										{
											if (CS$<>8__locals4.CS$<>8__locals1.pawn.WorkTagIsDisabled(workTags))
											{
												list2.Add(workTags.LabelTranslated().CapitalizeFirst());
											}
										}
										if (list2.Any<string>())
										{
											flag2 = true;
											if (!confirmText.NullOrEmpty())
											{
												confirmText += "\n\n";
											}
											confirmText += "ChooseRoleRequiredWorkTagsDisabled".Translate(CS$<>8__locals4.CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals4.newRole.Named("ROLE")) + ": \n" + list2.ToLineList("  - ");
										}
									}
									else if (CS$<>8__locals4.newRole.def.roleRequiredWorkTagAny != WorkTags.None)
									{
										bool flag3 = false;
										List<string> list3 = new List<string>();
										foreach (WorkTags workTags2 in CS$<>8__locals4.newRole.def.roleRequiredWorkTagAny.GetAllSelectedItems<WorkTags>())
										{
											if (!CS$<>8__locals4.CS$<>8__locals1.pawn.WorkTagIsDisabled(workTags2))
											{
												flag3 = true;
												break;
											}
											list3.Add(workTags2.LabelTranslated().CapitalizeFirst());
										}
										if (!flag3)
										{
											flag2 = true;
											if (!confirmText.NullOrEmpty())
											{
												confirmText += "\n\n";
											}
											confirmText += "ChooseRoleRequiredWorkTagsDisabled".Translate(CS$<>8__locals4.CS$<>8__locals1.pawn.Named("PAWN"), CS$<>8__locals4.newRole.Named("ROLE")) + ": \n" + list3.ToLineList("  - ");
										}
									}
									if (CS$<>8__locals4.newRole.ChosenPawnSingle() == null && CS$<>8__locals4.newRole is Precept_RoleSingle)
									{
										flag2 = true;
										if (!confirmText.NullOrEmpty())
										{
											confirmText += "\n\n";
										}
										confirmText += "ChooseRoleHint".Translate();
									}
									if (flag2)
									{
										list.Add(new FloatMenuOption(text, delegate()
										{
											TaggedString confirmText;
											confirmText += "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
											WindowStack windowStack = Find.WindowStack;
											confirmText = confirmText;
											Action confirmedAct;
											if ((confirmedAct = CS$<>8__locals4.<>9__8) == null)
											{
												confirmedAct = (CS$<>8__locals4.<>9__8 = delegate()
												{
													Precept_Role currentRole = CS$<>8__locals4.CS$<>8__locals1.currentRole;
													if (currentRole != null)
													{
														currentRole.Unassign(CS$<>8__locals4.CS$<>8__locals1.pawn, true);
													}
													CS$<>8__locals4.newRole.Assign(CS$<>8__locals4.CS$<>8__locals1.pawn, true);
												});
											}
											windowStack.Add(Dialog_MessageBox.CreateConfirmation(confirmText, confirmedAct, false, null));
										}, CS$<>8__locals4.newRole.Icon, CS$<>8__locals4.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals4.<DrawPawnRole>g__DrawTooltip|5), null, 0f, null, null, true, 0)
										{
											orderInPriority = CS$<>8__locals4.newRole.def.displayOrderInImpact
										});
									}
									else
									{
										list.Add(new FloatMenuOption(text, delegate()
										{
											CS$<>8__locals4.newRole.Assign(CS$<>8__locals4.CS$<>8__locals1.pawn, true);
										}, CS$<>8__locals4.newRole.Icon, CS$<>8__locals4.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals4.<DrawPawnRole>g__DrawTooltip|5), null, 0f, null, null, true, 0)
										{
											orderInPriority = CS$<>8__locals4.newRole.def.displayOrderInImpact
										});
									}
								}
							}
						}
						foreach (Precept_Role precept_Role2 in SocialCardUtility.cachedRoles)
						{
							if ((precept_Role2 != CS$<>8__locals1.currentRole && !precept_Role2.RequirementsMet(CS$<>8__locals1.pawn)) || !precept_Role2.Active)
							{
								string text2 = precept_Role2.LabelForPawn(CS$<>8__locals1.pawn) + " (" + precept_Role2.def.label + ")";
								if (precept_Role2.ChosenPawnSingle() != null)
								{
									text2 = text2 + ": " + precept_Role2.ChosenPawnSingle().LabelShort;
								}
								else if (!precept_Role2.RequirementsMet(CS$<>8__locals1.pawn))
								{
									text2 = text2 + ": " + precept_Role2.GetFirstUnmetRequirement(CS$<>8__locals1.pawn).GetLabel(precept_Role2).CapitalizeFirst();
								}
								else if (!precept_Role2.Active && precept_Role2.def.activationBelieverCount > precept_Role2.ideo.ColonistBelieverCountCached)
								{
									text2 += ": " + "InactiveRoleRequiresMoreBelievers".Translate(precept_Role2.def.activationBelieverCount, precept_Role2.ideo.memberName, precept_Role2.ideo.ColonistBelieverCountCached).CapitalizeFirst();
								}
								list.Add(new FloatMenuOption(text2, null, precept_Role2.Icon, precept_Role2.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
								{
									orderInPriority = precept_Role2.def.displayOrderInImpact
								});
							}
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				GUI.color = Color.white;
			}
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(0f, rect.yMax, rect.width);
			GUI.color = Color.white;
		}

		// Token: 0x0600755C RID: 30044 RVA: 0x00286AE4 File Offset: 0x00284CE4
		public static void DrawRelationsAndOpinions(Rect rect, Pawn selPawnForSocialInfo)
		{
			SocialCardUtility.CheckRecache(selPawnForSocialInfo);
			if (Current.ProgramState != ProgramState.Playing)
			{
				SocialCardUtility.showAllRelations = false;
			}
			GUI.BeginGroup(rect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, rect.width, rect.height);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, SocialCardUtility.listScrollViewHeight);
			Rect rect2 = rect;
			if (viewRect.height > outRect.height)
			{
				rect2.width -= 16f;
			}
			Widgets.BeginScrollView(outRect, ref SocialCardUtility.listScrollPosition, viewRect, true);
			float num = 0f;
			float y = SocialCardUtility.listScrollPosition.y;
			float num2 = SocialCardUtility.listScrollPosition.y + outRect.height;
			for (int i = 0; i < SocialCardUtility.cachedEntries.Count; i++)
			{
				float rowHeight = SocialCardUtility.GetRowHeight(SocialCardUtility.cachedEntries[i], rect2.width, selPawnForSocialInfo);
				if (num > y - rowHeight && num < num2)
				{
					SocialCardUtility.DrawPawnRow(num, rect2.width, SocialCardUtility.cachedEntries[i], selPawnForSocialInfo);
				}
				num += rowHeight;
			}
			if (!SocialCardUtility.cachedEntries.Any<SocialCardUtility.CachedSocialTabEntry>())
			{
				GUI.color = Color.gray;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.Label(new Rect(0f, 0f, rect2.width, 30f), "NoRelationships".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (Event.current.type == EventType.Layout)
			{
				SocialCardUtility.listScrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x0600755D RID: 30045 RVA: 0x00286C90 File Offset: 0x00284E90
		private static void DrawPawnRow(float y, float width, SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			float rowHeight = SocialCardUtility.GetRowHeight(entry, width, selPawnForSocialInfo);
			Rect rect = new Rect(0f, y, width, rowHeight);
			Pawn otherPawn = entry.otherPawn;
			if (Mouse.IsOver(rect))
			{
				GUI.color = SocialCardUtility.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => SocialCardUtility.GetPawnRowTooltip(entry, selPawnForSocialInfo), entry.otherPawn.thingIDNumber * 13 + selPawnForSocialInfo.thingIDNumber);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					if (otherPawn.Dead)
					{
						Messages.Message("MessageCantSelectDeadPawn".Translate(otherPawn.LabelShort, otherPawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
					else if (otherPawn.SpawnedOrAnyParentSpawned || otherPawn.IsCaravanMember())
					{
						CameraJumper.TryJumpAndSelect(otherPawn);
					}
					else
					{
						Messages.Message("MessageCantSelectOffMapPawn".Translate(otherPawn.LabelShort, otherPawn).CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					}
				}
				else if (Find.GameInitData.startingAndOptionalPawns.Contains(otherPawn))
				{
					Page_ConfigureStartingPawns page_ConfigureStartingPawns = Find.WindowStack.WindowOfType<Page_ConfigureStartingPawns>();
					if (page_ConfigureStartingPawns != null)
					{
						page_ConfigureStartingPawns.SelectPawn(otherPawn);
						SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
					}
				}
			}
			float width2;
			float width3;
			float width4;
			float width5;
			float width6;
			SocialCardUtility.CalculateColumnsWidths(width, out width2, out width3, out width4, out width5, out width6);
			Rect rect2 = new Rect(5f, y + 3f, width2, rowHeight - 3f);
			SocialCardUtility.DrawRelationLabel(entry, rect2, selPawnForSocialInfo);
			Rect rect3 = new Rect(rect2.xMax, y + 3f, width3, rowHeight - 3f);
			SocialCardUtility.DrawPawnLabel(otherPawn, rect3);
			Rect rect4 = new Rect(rect3.xMax, y + 3f, width4, rowHeight - 3f);
			SocialCardUtility.DrawMyOpinion(entry, rect4, selPawnForSocialInfo);
			Rect rect5 = new Rect(rect4.xMax, y + 3f, width5, rowHeight - 3f);
			SocialCardUtility.DrawHisOpinion(entry, rect5, selPawnForSocialInfo);
			Rect rect6 = new Rect(rect5.xMax, y + 3f, width6, rowHeight - 3f);
			SocialCardUtility.DrawPawnSituationLabel(entry.otherPawn, rect6, selPawnForSocialInfo);
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x00286F14 File Offset: 0x00285114
		private static float GetRowHeight(SocialCardUtility.CachedSocialTabEntry entry, float rowWidth, Pawn selPawnForSocialInfo)
		{
			float width;
			float width2;
			float num;
			float num2;
			float num3;
			SocialCardUtility.CalculateColumnsWidths(rowWidth, out width, out width2, out num, out num2, out num3);
			return Mathf.Max(Mathf.Max(0f, Text.CalcHeight(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo), width)), Text.CalcHeight(SocialCardUtility.GetPawnLabel(entry.otherPawn), width2)) + 3f;
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x00286F64 File Offset: 0x00285164
		private static void CalculateColumnsWidths(float rowWidth, out float relationsWidth, out float pawnLabelWidth, out float myOpinionWidth, out float hisOpinionWidth, out float pawnSituationLabelWidth)
		{
			float num = rowWidth - 10f;
			relationsWidth = num * 0.23f;
			pawnLabelWidth = num * 0.41f;
			myOpinionWidth = num * 0.075f;
			hisOpinionWidth = num * 0.085f;
			pawnSituationLabelWidth = num * 0.2f;
			if (myOpinionWidth < 25f)
			{
				pawnLabelWidth -= 25f - myOpinionWidth;
				myOpinionWidth = 25f;
			}
			if (hisOpinionWidth < 35f)
			{
				pawnLabelWidth -= 35f - hisOpinionWidth;
				hisOpinionWidth = 35f;
			}
		}

		// Token: 0x06007560 RID: 30048 RVA: 0x00286FE8 File Offset: 0x002851E8
		private static void DrawRelationLabel(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			string relationsString = SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo);
			if (!relationsString.NullOrEmpty())
			{
				GUI.color = SocialCardUtility.RelationLabelColor;
				Widgets.Label(rect, relationsString);
			}
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x00287016 File Offset: 0x00285216
		private static void DrawPawnLabel(Pawn pawn, Rect rect)
		{
			GUI.color = SocialCardUtility.PawnLabelColor;
			Widgets.Label(rect, SocialCardUtility.GetPawnLabel(pawn));
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x00287030 File Offset: 0x00285230
		private static void DrawMyOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (!entry.otherPawn.RaceProps.Humanlike || !selPawnForSocialInfo.RaceProps.Humanlike)
			{
				return;
			}
			int opinionOfOtherPawn = entry.opinionOfOtherPawn;
			GUI.color = SocialCardUtility.OpinionLabelColor(opinionOfOtherPawn);
			Widgets.Label(rect, opinionOfOtherPawn.ToStringWithSign());
		}

		// Token: 0x06007563 RID: 30051 RVA: 0x0028707C File Offset: 0x0028527C
		private static void DrawHisOpinion(SocialCardUtility.CachedSocialTabEntry entry, Rect rect, Pawn selPawnForSocialInfo)
		{
			if (!entry.otherPawn.RaceProps.Humanlike || !selPawnForSocialInfo.RaceProps.Humanlike)
			{
				return;
			}
			int opinionOfMe = entry.opinionOfMe;
			Color color = SocialCardUtility.OpinionLabelColor(opinionOfMe);
			GUI.color = new Color(color.r, color.g, color.b, 0.4f);
			Widgets.Label(rect, "(" + opinionOfMe.ToStringWithSign() + ")");
		}

		// Token: 0x06007564 RID: 30052 RVA: 0x002870F4 File Offset: 0x002852F4
		private static void DrawPawnSituationLabel(Pawn pawn, Rect rect, Pawn selPawnForSocialInfo)
		{
			GUI.color = Color.gray;
			string label = SocialCardUtility.GetPawnSituationLabel(pawn, selPawnForSocialInfo).Truncate(rect.width, null);
			Widgets.Label(rect, label);
		}

		// Token: 0x06007565 RID: 30053 RVA: 0x00287127 File Offset: 0x00285327
		private static Color OpinionLabelColor(int opinion)
		{
			if (Mathf.Abs(opinion) < 10)
			{
				return Color.gray;
			}
			if (opinion < 0)
			{
				return ColorLibrary.RedReadable;
			}
			return Color.green;
		}

		// Token: 0x06007566 RID: 30054 RVA: 0x00287148 File Offset: 0x00285348
		private static string GetPawnLabel(Pawn pawn)
		{
			if (pawn.Name != null)
			{
				return pawn.Name.ToStringFull;
			}
			return pawn.LabelCapNoCount;
		}

		// Token: 0x06007567 RID: 30055 RVA: 0x00287164 File Offset: 0x00285364
		public static string GetPawnSituationLabel(Pawn pawn, Pawn fromPOV)
		{
			if (pawn.Dead)
			{
				return "Dead".Translate();
			}
			if (pawn.Destroyed)
			{
				return "Missing".Translate();
			}
			if (PawnUtility.IsKidnappedPawn(pawn))
			{
				return "Kidnapped".Translate();
			}
			QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true).FirstOrDefault((QuestPart_LendColonistsToFaction p) => p.LentColonistsListForReading.Contains(pawn));
			if (questPart_LendColonistsToFaction != null)
			{
				return "Lent".Translate(questPart_LendColonistsToFaction.lendColonistsToFaction.Named("FACTION"), questPart_LendColonistsToFaction.returnLentColonistsInTicks.ToStringTicksToDays("0.0"));
			}
			if (pawn.kindDef == PawnKindDefOf.Slave)
			{
				return "Slave".Translate();
			}
			if (PawnUtility.IsFactionLeader(pawn))
			{
				return "FactionLeader".Translate();
			}
			Faction faction = pawn.Faction;
			if (faction == fromPOV.Faction)
			{
				return "";
			}
			if (faction == null || fromPOV.Faction == null)
			{
				return "Neutral".Translate();
			}
			switch (faction.RelationKindWith(fromPOV.Faction))
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate() + ", " + faction.Name;
			case FactionRelationKind.Neutral:
				return "Neutral".Translate() + ", " + faction.Name;
			case FactionRelationKind.Ally:
				return "Ally".Translate() + ", " + faction.Name;
			default:
				return "";
			}
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x00287334 File Offset: 0x00285534
		private static string GetRelationsString(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			string text = "";
			if (entry.relations.Count != 0)
			{
				for (int i = 0; i < entry.relations.Count; i++)
				{
					PawnRelationDef pawnRelationDef = entry.relations[i];
					if (!text.NullOrEmpty())
					{
						text = text + ", " + pawnRelationDef.GetGenderSpecificLabel(entry.otherPawn);
					}
					else
					{
						text = pawnRelationDef.GetGenderSpecificLabelCap(entry.otherPawn);
					}
				}
				return text;
			}
			if (entry.opinionOfOtherPawn < -20)
			{
				return "Rival".Translate();
			}
			if (entry.opinionOfOtherPawn > 20)
			{
				return "Friend".Translate();
			}
			return "Acquaintance".Translate();
		}

		// Token: 0x06007569 RID: 30057 RVA: 0x002873EC File Offset: 0x002855EC
		private static string GetPawnRowTooltip(SocialCardUtility.CachedSocialTabEntry entry, Pawn selPawnForSocialInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (entry.otherPawn.RaceProps.Humanlike && selPawnForSocialInfo.RaceProps.Humanlike)
			{
				stringBuilder.AppendLine(selPawnForSocialInfo.relations.OpinionExplanation(entry.otherPawn));
				stringBuilder.AppendLine();
				stringBuilder.Append(("SomeonesOpinionOfMe".Translate(entry.otherPawn.LabelShort, entry.otherPawn) + ": ").Colorize(ColoredText.TipSectionTitleColor));
				stringBuilder.Append(entry.opinionOfMe.ToStringWithSign());
			}
			else
			{
				stringBuilder.AppendLine(entry.otherPawn.LabelCapNoCount);
				string pawnSituationLabel = SocialCardUtility.GetPawnSituationLabel(entry.otherPawn, selPawnForSocialInfo);
				if (!pawnSituationLabel.NullOrEmpty())
				{
					stringBuilder.AppendLine(pawnSituationLabel);
				}
				stringBuilder.AppendLine("--------------");
				stringBuilder.Append(SocialCardUtility.GetRelationsString(entry, selPawnForSocialInfo));
			}
			if (Prefs.DevMode)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("(debug) Compatibility: " + selPawnForSocialInfo.relations.CompatibilityWith(entry.otherPawn).ToString("F2"));
				stringBuilder.Append("(debug) RomanceChanceFactor: " + selPawnForSocialInfo.relations.SecondaryRomanceChanceFactor(entry.otherPawn).ToString("F2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600756A RID: 30058 RVA: 0x0028754F File Offset: 0x0028574F
		private static void DrawDebugOptions(Rect rect, Pawn pawn)
		{
			GUI.BeginGroup(rect);
			Widgets.CheckboxLabeled(new Rect(0f, 0f, 145f, 22f), "Dev: AllRelations", ref SocialCardUtility.showAllRelations, false, null, null, false);
			GUI.EndGroup();
		}

		// Token: 0x040040F1 RID: 16625
		private static Vector2 listScrollPosition = Vector2.zero;

		// Token: 0x040040F2 RID: 16626
		private static float listScrollViewHeight = 0f;

		// Token: 0x040040F3 RID: 16627
		private static bool showAllRelations;

		// Token: 0x040040F4 RID: 16628
		private static List<SocialCardUtility.CachedSocialTabEntry> cachedEntries = new List<SocialCardUtility.CachedSocialTabEntry>();

		// Token: 0x040040F5 RID: 16629
		private static List<Precept_Role> cachedRoles = new List<Precept_Role>();

		// Token: 0x040040F6 RID: 16630
		private static Pawn cachedForPawn;

		// Token: 0x040040F7 RID: 16631
		private const float TopPadding = 15f;

		// Token: 0x040040F8 RID: 16632
		private const float TopPaddingDevMode = 20f;

		// Token: 0x040040F9 RID: 16633
		private static readonly Color RelationLabelColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x040040FA RID: 16634
		private static readonly Color PawnLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x040040FB RID: 16635
		private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x040040FC RID: 16636
		private const float RowTopPadding = 3f;

		// Token: 0x040040FD RID: 16637
		private const float RowLeftRightPadding = 5f;

		// Token: 0x040040FE RID: 16638
		private const float IconSize = 32f;

		// Token: 0x040040FF RID: 16639
		private static SocialCardUtility.CachedSocialTabEntryComparer CachedEntriesComparer = new SocialCardUtility.CachedSocialTabEntryComparer();

		// Token: 0x04004100 RID: 16640
		private static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.40392157f, 0.7019608f, 0.28627452f));

		// Token: 0x04004101 RID: 16641
		private static HashSet<Pawn> tmpToCache = new HashSet<Pawn>();

		// Token: 0x020026C2 RID: 9922
		private class CachedSocialTabEntry
		{
			// Token: 0x04009340 RID: 37696
			public Pawn otherPawn;

			// Token: 0x04009341 RID: 37697
			public int opinionOfOtherPawn;

			// Token: 0x04009342 RID: 37698
			public int opinionOfMe;

			// Token: 0x04009343 RID: 37699
			public List<PawnRelationDef> relations = new List<PawnRelationDef>();
		}

		// Token: 0x020026C3 RID: 9923
		private class CachedSocialTabEntryComparer : IComparer<SocialCardUtility.CachedSocialTabEntry>
		{
			// Token: 0x0600D75F RID: 55135 RVA: 0x0040C8C4 File Offset: 0x0040AAC4
			public int Compare(SocialCardUtility.CachedSocialTabEntry a, SocialCardUtility.CachedSocialTabEntry b)
			{
				bool flag = a.relations.Any<PawnRelationDef>();
				bool flag2 = b.relations.Any<PawnRelationDef>();
				if (flag != flag2)
				{
					return flag2.CompareTo(flag);
				}
				if (flag && flag2)
				{
					float num = float.MinValue;
					for (int i = 0; i < a.relations.Count; i++)
					{
						if (a.relations[i].importance > num)
						{
							num = a.relations[i].importance;
						}
					}
					float num2 = float.MinValue;
					for (int j = 0; j < b.relations.Count; j++)
					{
						if (b.relations[j].importance > num2)
						{
							num2 = b.relations[j].importance;
						}
					}
					if (num != num2)
					{
						return num2.CompareTo(num);
					}
				}
				if (a.opinionOfOtherPawn != b.opinionOfOtherPawn)
				{
					return b.opinionOfOtherPawn.CompareTo(a.opinionOfOtherPawn);
				}
				return a.otherPawn.thingIDNumber.CompareTo(b.otherPawn.thingIDNumber);
			}
		}
	}
}
