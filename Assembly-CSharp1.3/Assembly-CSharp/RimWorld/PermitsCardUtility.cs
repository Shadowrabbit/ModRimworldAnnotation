using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200130D RID: 4877
	[StaticConstructorOnStartup]
	public static class PermitsCardUtility
	{
		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x06007546 RID: 30022 RVA: 0x002840EC File Offset: 0x002822EC
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

		// Token: 0x06007547 RID: 30023 RVA: 0x002841A0 File Offset: 0x002823A0
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

		// Token: 0x06007548 RID: 30024 RVA: 0x00284208 File Offset: 0x00282408
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			if (!ModLister.CheckRoyalty("Permit"))
			{
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
							}, localFaction.def.FactionIcon, localFaction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
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

		// Token: 0x06007549 RID: 30025 RVA: 0x00284728 File Offset: 0x00282928
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
					text = text + "\n" + "RequiresTitle".Translate(PermitsCardUtility.selectedPermit.minTitle.GetLabelForBothGenders()).Resolve().Colorize((currentTitle != null && currentTitle.seniority >= PermitsCardUtility.selectedPermit.minTitle.seniority) ? Color.white : ColorLibrary.RedReadable);
				}
				if (PermitsCardUtility.selectedPermit.prerequisite != null)
				{
					text = text + "\n" + "UpgradeFrom".Translate(PermitsCardUtility.selectedPermit.prerequisite.LabelCap).Resolve().Colorize(PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit.prerequisite, pawn) ? Color.white : ColorLibrary.RedReadable);
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

		// Token: 0x0600754A RID: 30026 RVA: 0x00284A54 File Offset: 0x00282C54
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

		// Token: 0x0600754B RID: 30027 RVA: 0x00284C48 File Offset: 0x00282E48
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

		// Token: 0x0600754C RID: 30028 RVA: 0x00284D54 File Offset: 0x00282F54
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

		// Token: 0x0600754D RID: 30029 RVA: 0x00284DBD File Offset: 0x00282FBD
		private static Vector2 DrawPosition(RoyalTitlePermitDef permit)
		{
			Vector2 a = new Vector2(permit.uiPosition.x * 200f, permit.uiPosition.y * 50f);
			return a + a * PermitsCardUtility.PermitOptionSpacing;
		}

		// Token: 0x0600754E RID: 30030 RVA: 0x00284DF6 File Offset: 0x00282FF6
		private static bool CanDrawPermit(RoyalTitlePermitDef permit)
		{
			return permit.permitPointCost > 0 && (permit.faction == null || permit.faction == PermitsCardUtility.selectedFaction.def);
		}

		// Token: 0x040040DF RID: 16607
		private static Vector2 rightScrollPosition;

		// Token: 0x040040E0 RID: 16608
		public static RoyalTitlePermitDef selectedPermit;

		// Token: 0x040040E1 RID: 16609
		public static Faction selectedFaction;

		// Token: 0x040040E2 RID: 16610
		private const float LeftRectPercent = 0.33f;

		// Token: 0x040040E3 RID: 16611
		private const float TitleHeight = 55f;

		// Token: 0x040040E4 RID: 16612
		private const float ReturnButtonWidth = 180f;

		// Token: 0x040040E5 RID: 16613
		private const float PermitOptionWidth = 200f;

		// Token: 0x040040E6 RID: 16614
		private const float PermitOptionHeight = 50f;

		// Token: 0x040040E7 RID: 16615
		private const float AcceptButtonHeight = 50f;

		// Token: 0x040040E8 RID: 16616
		private const float SwitchFactionsButtonSize = 32f;

		// Token: 0x040040E9 RID: 16617
		private const float LineWidthNotSelected = 2f;

		// Token: 0x040040EA RID: 16618
		private const float LineWidthSelected = 4f;

		// Token: 0x040040EB RID: 16619
		private const int ReturnPermitsCost = 8;

		// Token: 0x040040EC RID: 16620
		private static readonly Vector2 PermitOptionSpacing = new Vector2(0.25f, 0.35f);

		// Token: 0x040040ED RID: 16621
		private static readonly Texture2D SwitchFactionIcon = ContentFinder<Texture2D>.Get("UI/Icons/SwitchFaction", true);
	}
}
