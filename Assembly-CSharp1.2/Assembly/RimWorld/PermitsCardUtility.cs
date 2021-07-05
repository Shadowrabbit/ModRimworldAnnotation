using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A55 RID: 6741
	[StaticConstructorOnStartup]
	public static class PermitsCardUtility
	{
		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x0600949F RID: 38047 RVA: 0x002B1248 File Offset: 0x002AF448
		private static bool ShowSwitchFactionButton
		{
			get
			{
				int num = 0;
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (!faction.IsPlayer && !faction.def.permanentEnemy && !faction.temporary)
					{
						using (IEnumerator<RoyalTitlePermitDef> enumerator2 = DefDatabase<RoyalTitlePermitDef>.AllDefs.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.faction == faction.def)
								{
									num++;
									break;
								}
							}
						}
					}
				}
				return num > 1;
			}
		}

		// Token: 0x060094A0 RID: 38048 RVA: 0x002B12FC File Offset: 0x002AF4FC
		private static int TotalReturnPermitsCost(Pawn pawn)
		{
			int num = 8;
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].OnCooldown && allFactionPermits[i].Permit.royalAid != null)
				{
					num += allFactionPermits[i].Permit.royalAid.favorCost;
				}
			}
			return num;
		}

		// Token: 0x060094A1 RID: 38049 RVA: 0x002B1364 File Offset: 0x002AF564
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Permits are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 43244, false);
				return;
			}
			rect.yMax -= 4f;
			if (PermitsCardUtility.ShowSwitchFactionButton)
			{
				Rect rect2 = new Rect(rect.x, rect.y, 32f, 32f);
				if (Widgets.ButtonImage(rect2, PermitsCardUtility.SwitchFactionIcon, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Faction faction in Find.FactionManager.AllFactionsVisibleInViewOrder)
					{
						if (!faction.IsPlayer && !faction.def.permanentEnemy)
						{
							Faction localFaction = faction;
							list.Add(new FloatMenuOption(localFaction.Name, delegate()
							{
								PermitsCardUtility.selectedFaction = localFaction;
								PermitsCardUtility.selectedPermit = null;
							}, localFaction.def.FactionIcon, localFaction.Color, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				TooltipHandler.TipRegion(rect2, "SwitchFaction_Desc".Translate());
			}
			if (PermitsCardUtility.selectedFaction.def.HasRoyalTitles)
			{
				string label = "ReturnAllPermits".Translate();
				Rect rect3 = new Rect(rect.xMax - 180f, rect.y - 4f, 180f, 51f);
				int num = PermitsCardUtility.TotalReturnPermitsCost(pawn);
				if (Widgets.ButtonText(rect3, label, true, true, true))
				{
					if (!pawn.royalty.PermitsFromFaction(PermitsCardUtility.selectedFaction).Any<FactionPermit>())
					{
						Messages.Message("NoPermitsToReturn".Translate(pawn.Named("PAWN")), new LookTargets(pawn), MessageTypeDefOf.RejectInput, false);
					}
					else if (pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction) < num)
					{
						Messages.Message("NotEnoughFavor".Translate(num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR"), pawn.Named("PAWN"), pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction).Named("CURFAVOR")), MessageTypeDefOf.RejectInput, true);
					}
					else
					{
						string str = "ReturnAllPermits_Confirm".Translate(8.Named("BASEFAVORCOST"), num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR"), PermitsCardUtility.selectedFaction.Named("FACTION"));
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, delegate
						{
							pawn.royalty.RefundPermits(8, PermitsCardUtility.selectedFaction);
						}, true, null));
					}
				}
				TooltipHandler.TipRegion(rect3, "ReturnAllPermits_Desc".Translate(8.Named("BASEFAVORCOST"), num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR")));
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(PermitsCardUtility.selectedFaction);
			Rect rect4 = new Rect(rect.xMax - 360f - 4f, rect.y - 4f, 360f, 55f);
			string text = "CurrentTitle".Translate() + ": " + ((currentTitle != null) ? currentTitle.GetLabelFor(pawn).CapitalizeFirst() : "None".Translate()) + "\n" + "UnusedPermits".Translate() + ": " + pawn.royalty.GetPermitPoints(PermitsCardUtility.selectedFaction);
			if (!PermitsCardUtility.selectedFaction.def.royalFavorLabel.NullOrEmpty())
			{
				text = string.Concat(new object[]
				{
					text,
					"\n",
					PermitsCardUtility.selectedFaction.def.royalFavorLabel.CapitalizeFirst(),
					": ",
					pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction)
				});
			}
			Widgets.Label(rect4, text);
			rect.yMin += 55f;
			Rect rect5 = new Rect(rect);
			rect5.width *= 0.33f;
			PermitsCardUtility.DoLeftRect(rect5, pawn);
			PermitsCardUtility.DoRightRect(new Rect(rect)
			{
				xMin = rect5.xMax + 10f
			}, pawn);
		}

		// Token: 0x060094A2 RID: 38050 RVA: 0x002B1890 File Offset: 0x002AFA90
		private static void DoLeftRect(Rect rect, Pawn pawn)
		{
			float num = 0f;
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(PermitsCardUtility.selectedFaction);
			Rect position = new Rect(rect);
			GUI.BeginGroup(position);
			if (PermitsCardUtility.selectedPermit != null)
			{
				Text.Font = GameFont.Medium;
				Rect rect2 = new Rect(0f, num, position.width, 0f);
				Widgets.LabelCacheHeight(ref rect2, PermitsCardUtility.selectedPermit.LabelCap, true, false);
				Text.Font = GameFont.Small;
				num += rect2.height;
				if (!PermitsCardUtility.selectedPermit.description.NullOrEmpty())
				{
					Rect rect3 = new Rect(0f, num, position.width, 0f);
					Widgets.LabelCacheHeight(ref rect3, PermitsCardUtility.selectedPermit.description, true, false);
					num += rect3.height + 16f;
				}
				Rect rect4 = new Rect(0f, num, position.width, 0f);
				string text = "Cooldown".Translate() + ": " + "PeriodDays".Translate(PermitsCardUtility.selectedPermit.cooldownDays);
				if (PermitsCardUtility.selectedPermit.royalAid != null && PermitsCardUtility.selectedPermit.royalAid.favorCost > 0 && !PermitsCardUtility.selectedFaction.def.royalFavorLabel.NullOrEmpty())
				{
					text = text + ("\n" + "CooldownUseFavorCost".Translate(PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("HONOR")).CapitalizeFirst() + ": ") + PermitsCardUtility.selectedPermit.royalAid.favorCost;
				}
				if (PermitsCardUtility.selectedPermit.minTitle != null)
				{
					text = text + "\n" + "RequiresTitle".Translate(PermitsCardUtility.selectedPermit.minTitle.GetLabelForBothGenders()).Resolve().Colorize((currentTitle != null && currentTitle.seniority >= PermitsCardUtility.selectedPermit.minTitle.seniority) ? Color.white : ColoredText.RedReadable);
				}
				if (PermitsCardUtility.selectedPermit.prerequisite != null)
				{
					text = text + "\n" + "UpgradeFrom".Translate(PermitsCardUtility.selectedPermit.prerequisite.LabelCap).Resolve().Colorize(PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit.prerequisite, pawn) ? Color.white : ColoredText.RedReadable);
				}
				Widgets.LabelCacheHeight(ref rect4, text, true, false);
				num += rect4.height + 4f;
				Rect rect5 = new Rect(0f, position.height - 50f, position.width, 50f);
				if (PermitsCardUtility.selectedPermit.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction) && !PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit, pawn) && Widgets.ButtonText(rect5, "AcceptPermit".Translate(), true, true, true))
				{
					SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
					pawn.royalty.AddPermit(PermitsCardUtility.selectedPermit, PermitsCardUtility.selectedFaction);
				}
			}
			GUI.EndGroup();
		}

		// Token: 0x060094A3 RID: 38051 RVA: 0x002B1BBC File Offset: 0x002AFDBC
		private static void DoRightRect(Rect rect, Pawn pawn)
		{
			Widgets.DrawMenuSection(rect);
			if (PermitsCardUtility.selectedFaction == null)
			{
				return;
			}
			List<RoyalTitlePermitDef> allDefsListForReading = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;
			Rect outRect = rect.ContractedBy(10f);
			Rect rect2 = default(Rect);
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RoyalTitlePermitDef permit = allDefsListForReading[i];
				if (PermitsCardUtility.CanDrawPermit(permit))
				{
					rect2.width = Mathf.Max(rect2.width, PermitsCardUtility.DrawPosition(permit).x + 200f + 26f);
					rect2.height = Mathf.Max(rect2.height, PermitsCardUtility.DrawPosition(permit).y + 50f + 26f);
				}
			}
			Widgets.BeginScrollView(outRect, ref PermitsCardUtility.rightScrollPosition, rect2, true);
			GUI.BeginGroup(rect2.ContractedBy(10f));
			PermitsCardUtility.DrawLines();
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				RoyalTitlePermitDef royalTitlePermitDef = allDefsListForReading[j];
				if (PermitsCardUtility.CanDrawPermit(royalTitlePermitDef))
				{
					Vector2 vector = PermitsCardUtility.DrawPosition(royalTitlePermitDef);
					Rect rect3 = new Rect(vector.x, vector.y, 200f, 50f);
					Color color = Widgets.NormalOptionColor;
					Color color2 = PermitsCardUtility.PermitUnlocked(royalTitlePermitDef, pawn) ? TexUI.FinishedResearchColor : TexUI.AvailResearchColor;
					Color borderColor;
					if (PermitsCardUtility.selectedPermit == royalTitlePermitDef)
					{
						borderColor = TexUI.HighlightBorderResearchColor;
						color2 += TexUI.HighlightBgResearchColor;
					}
					else
					{
						borderColor = TexUI.DefaultBorderResearchColor;
					}
					if (!royalTitlePermitDef.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction) && !PermitsCardUtility.PermitUnlocked(royalTitlePermitDef, pawn))
					{
						color = Color.red;
					}
					if (Widgets.CustomButtonText(ref rect3, string.Empty, color2, color, borderColor, false, 1, true, true))
					{
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						PermitsCardUtility.selectedPermit = royalTitlePermitDef;
					}
					TextAnchor anchor = Text.Anchor;
					Color color3 = GUI.color;
					GUI.color = color;
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect3, royalTitlePermitDef.LabelCap);
					GUI.color = color3;
					Text.Anchor = anchor;
				}
			}
			GUI.EndGroup();
			Widgets.EndScrollView();
		}

		// Token: 0x060094A4 RID: 38052 RVA: 0x002B1DB0 File Offset: 0x002AFFB0
		private static void DrawLines()
		{
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			List<RoyalTitlePermitDef> allDefsListForReading = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					RoyalTitlePermitDef royalTitlePermitDef = allDefsListForReading[j];
					if (PermitsCardUtility.CanDrawPermit(royalTitlePermitDef))
					{
						Vector2 vector = PermitsCardUtility.DrawPosition(royalTitlePermitDef);
						start.x = vector.x;
						start.y = vector.y + 25f;
						RoyalTitlePermitDef prerequisite = royalTitlePermitDef.prerequisite;
						if (prerequisite != null)
						{
							Vector2 vector2 = PermitsCardUtility.DrawPosition(prerequisite);
							end.x = vector2.x + 200f;
							end.y = vector2.y + 25f;
							if ((i == 1 && PermitsCardUtility.selectedPermit == royalTitlePermitDef) || PermitsCardUtility.selectedPermit == prerequisite)
							{
								Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
							}
							else if (i == 0)
							{
								Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
							}
						}
					}
				}
			}
		}

		// Token: 0x060094A5 RID: 38053 RVA: 0x000634FD File Offset: 0x000616FD
		[Obsolete("Will be removed in future version")]
		private static bool PermitAvailable(RoyalTitlePermitDef permit, Pawn pawn)
		{
			return permit.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction);
		}

		// Token: 0x060094A6 RID: 38054 RVA: 0x002B1EBC File Offset: 0x002B00BC
		private static bool PermitUnlocked(RoyalTitlePermitDef permit, Pawn pawn)
		{
			if (pawn.royalty.HasPermit(permit, PermitsCardUtility.selectedFaction))
			{
				return true;
			}
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].Permit.prerequisite == permit && allFactionPermits[i].Faction == PermitsCardUtility.selectedFaction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060094A7 RID: 38055 RVA: 0x0006350B File Offset: 0x0006170B
		private static Vector2 DrawPosition(RoyalTitlePermitDef permit)
		{
			Vector2 a = new Vector2(permit.uiPosition.x * 200f, permit.uiPosition.y * 50f);
			return a + a * PermitsCardUtility.PermitOptionSpacing;
		}

		// Token: 0x060094A8 RID: 38056 RVA: 0x00063544 File Offset: 0x00061744
		private static bool CanDrawPermit(RoyalTitlePermitDef permit)
		{
			return permit.permitPointCost > 0 && (permit.faction == null || permit.faction == PermitsCardUtility.selectedFaction.def);
		}

		// Token: 0x04005E7C RID: 24188
		private static Vector2 rightScrollPosition;

		// Token: 0x04005E7D RID: 24189
		public static RoyalTitlePermitDef selectedPermit;

		// Token: 0x04005E7E RID: 24190
		public static Faction selectedFaction;

		// Token: 0x04005E7F RID: 24191
		private const float LeftRectPercent = 0.33f;

		// Token: 0x04005E80 RID: 24192
		private const float TitleHeight = 55f;

		// Token: 0x04005E81 RID: 24193
		private const float ReturnButtonWidth = 180f;

		// Token: 0x04005E82 RID: 24194
		private const float PermitOptionWidth = 200f;

		// Token: 0x04005E83 RID: 24195
		private const float PermitOptionHeight = 50f;

		// Token: 0x04005E84 RID: 24196
		private const float AcceptButtonHeight = 50f;

		// Token: 0x04005E85 RID: 24197
		private const float SwitchFactionsButtonSize = 32f;

		// Token: 0x04005E86 RID: 24198
		private const float LineWidthNotSelected = 2f;

		// Token: 0x04005E87 RID: 24199
		private const float LineWidthSelected = 4f;

		// Token: 0x04005E88 RID: 24200
		private const int ReturnPermitsCost = 8;

		// Token: 0x04005E89 RID: 24201
		private static readonly Vector2 PermitOptionSpacing = new Vector2(0.25f, 0.35f);

		// Token: 0x04005E8A RID: 24202
		private static readonly Texture2D SwitchFactionIcon = ContentFinder<Texture2D>.Get("UI/Icons/SwitchFaction", true);
	}
}
