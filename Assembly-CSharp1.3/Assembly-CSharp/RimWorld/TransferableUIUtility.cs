using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001319 RID: 4889
	[StaticConstructorOnStartup]
	public static class TransferableUIUtility
	{
		// Token: 0x060075F7 RID: 30199 RVA: 0x0028A6E8 File Offset: 0x002888E8
		public static void DoCountAdjustInterface(Rect rect, Transferable trad, int index, int min, int max, bool flash = false, List<TransferableCountToTransferStoppingPoint> extraStoppingPoints = null, bool readOnly = false)
		{
			TransferableUIUtility.stoppingPoints.Clear();
			if (extraStoppingPoints != null)
			{
				TransferableUIUtility.stoppingPoints.AddRange(extraStoppingPoints);
			}
			for (int i = TransferableUIUtility.stoppingPoints.Count - 1; i >= 0; i--)
			{
				if (TransferableUIUtility.stoppingPoints[i].threshold != 0 && (TransferableUIUtility.stoppingPoints[i].threshold <= min || TransferableUIUtility.stoppingPoints[i].threshold >= max))
				{
					TransferableUIUtility.stoppingPoints.RemoveAt(i);
				}
			}
			bool flag = false;
			for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
			{
				if (TransferableUIUtility.stoppingPoints[j].threshold == 0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TransferableUIUtility.stoppingPoints.Add(new TransferableCountToTransferStoppingPoint(0, "0", "0"));
			}
			TransferableUIUtility.DoCountAdjustInterfaceInternal(rect, trad, index, min, max, flash, readOnly);
		}

		// Token: 0x060075F8 RID: 30200 RVA: 0x0028A7C4 File Offset: 0x002889C4
		private static void DoCountAdjustInterfaceInternal(Rect rect, Transferable trad, int index, int min, int max, bool flash, bool readOnly)
		{
			rect = rect.Rounded();
			Rect rect2 = new Rect(rect.center.x - 45f, rect.center.y - 12.5f, 90f, 25f).Rounded();
			if (flash)
			{
				GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
			}
			TransferableOneWay transferableOneWay = trad as TransferableOneWay;
			bool flag = transferableOneWay != null && transferableOneWay.HasAnyThing && transferableOneWay.AnyThing is Pawn && transferableOneWay.MaxCount == 1;
			if (!trad.Interactive || readOnly)
			{
				if (flag)
				{
					bool flag2 = trad.CountToTransfer != 0;
					Widgets.Checkbox(rect2.position, ref flag2, 24f, true, false, null, null);
				}
				else
				{
					GUI.color = ((trad.CountToTransfer == 0) ? TransferableUIUtility.ZeroCountColor : Color.white);
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect2, trad.CountToTransfer.ToStringCached());
				}
			}
			else if (flag)
			{
				bool flag3 = trad.CountToTransfer != 0;
				bool flag4 = flag3;
				Widgets.Checkbox(rect2.position, ref flag4, 24f, false, true, null, null);
				if (flag4 != flag3)
				{
					if (flag4)
					{
						trad.AdjustTo(trad.GetMaximumToTransfer());
					}
					else
					{
						trad.AdjustTo(trad.GetMinimumToTransfer());
					}
				}
			}
			else
			{
				Rect rect3 = rect2.ContractedBy(2f);
				rect3.xMax -= 15f;
				rect3.xMin += 16f;
				int countToTransfer = trad.CountToTransfer;
				string editBuffer = trad.EditBuffer;
				Widgets.TextFieldNumeric<int>(rect3, ref countToTransfer, ref editBuffer, (float)min, (float)max);
				trad.AdjustTo(countToTransfer);
				trad.EditBuffer = editBuffer;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (trad.Interactive && !flag && !readOnly)
			{
				TransferablePositiveCountDirection positiveCountDirection = trad.PositiveCountDirection;
				int num = (positiveCountDirection == TransferablePositiveCountDirection.Source) ? 1 : -1;
				int num2 = GenUI.CurrentAdjustmentMultiplier();
				bool flag5 = trad.GetRange() == 1;
				if (trad.CanAdjustBy(num * num2).Accepted)
				{
					Rect rect4 = new Rect(rect2.x - 30f, rect.y, 30f, rect.height);
					if (flag5)
					{
						rect4.x -= rect4.width;
						rect4.width += rect4.width;
					}
					if (Widgets.ButtonText(rect4, "<", true, true, true))
					{
						trad.AdjustBy(num * num2);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					if (!flag5)
					{
						string label = "<<";
						int? num3 = null;
						int num4 = 0;
						for (int i = 0; i < TransferableUIUtility.stoppingPoints.Count; i++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint = TransferableUIUtility.stoppingPoints[i];
							if (positiveCountDirection == TransferablePositiveCountDirection.Source)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold < num4 || num3 == null))
								{
									label = transferableCountToTransferStoppingPoint.leftLabel;
									num3 = new int?(transferableCountToTransferStoppingPoint.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint.threshold && (transferableCountToTransferStoppingPoint.threshold > num4 || num3 == null))
							{
								label = transferableCountToTransferStoppingPoint.leftLabel;
								num3 = new int?(transferableCountToTransferStoppingPoint.threshold);
							}
						}
						rect4.x -= rect4.width;
						if (Widgets.ButtonText(rect4, label, true, true, true))
						{
							if (num3 != null)
							{
								trad.AdjustTo(num3.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMaximumToTransfer());
							}
							else
							{
								trad.AdjustTo(trad.GetMinimumToTransfer());
							}
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						}
					}
				}
				if (trad.CanAdjustBy(-num * num2).Accepted)
				{
					Rect rect5 = new Rect(rect2.xMax, rect.y, 30f, rect.height);
					if (flag5)
					{
						rect5.width += rect5.width;
					}
					if (Widgets.ButtonText(rect5, ">", true, true, true))
					{
						trad.AdjustBy(-num * num2);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					if (!flag5)
					{
						string label2 = ">>";
						int? num5 = null;
						int num6 = 0;
						for (int j = 0; j < TransferableUIUtility.stoppingPoints.Count; j++)
						{
							TransferableCountToTransferStoppingPoint transferableCountToTransferStoppingPoint2 = TransferableUIUtility.stoppingPoints[j];
							if (positiveCountDirection == TransferablePositiveCountDirection.Destination)
							{
								if (trad.CountToTransfer < transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold < num6 || num5 == null))
								{
									label2 = transferableCountToTransferStoppingPoint2.rightLabel;
									num5 = new int?(transferableCountToTransferStoppingPoint2.threshold);
								}
							}
							else if (trad.CountToTransfer > transferableCountToTransferStoppingPoint2.threshold && (transferableCountToTransferStoppingPoint2.threshold > num6 || num5 == null))
							{
								label2 = transferableCountToTransferStoppingPoint2.rightLabel;
								num5 = new int?(transferableCountToTransferStoppingPoint2.threshold);
							}
						}
						rect5.x += rect5.width;
						if (Widgets.ButtonText(rect5, label2, true, true, true))
						{
							if (num5 != null)
							{
								trad.AdjustTo(num5.Value);
							}
							else if (num == 1)
							{
								trad.AdjustTo(trad.GetMinimumToTransfer());
							}
							else
							{
								trad.AdjustTo(trad.GetMaximumToTransfer());
							}
							SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
						}
					}
				}
			}
			if (trad.CountToTransfer != 0)
			{
				Rect position = new Rect(rect2.x + rect2.width / 2f - (float)(TransferableUIUtility.TradeArrow.width / 2), rect2.y + rect2.height / 2f - (float)(TransferableUIUtility.TradeArrow.height / 2), (float)TransferableUIUtility.TradeArrow.width, (float)TransferableUIUtility.TradeArrow.height);
				TransferablePositiveCountDirection positiveCountDirection2 = trad.PositiveCountDirection;
				if ((positiveCountDirection2 == TransferablePositiveCountDirection.Source && trad.CountToTransfer > 0) || (positiveCountDirection2 == TransferablePositiveCountDirection.Destination && trad.CountToTransfer < 0))
				{
					position.x += position.width;
					position.width *= -1f;
				}
				GUI.DrawTexture(position, TransferableUIUtility.TradeArrow);
			}
		}

		// Token: 0x060075F9 RID: 30201 RVA: 0x0028ADDC File Offset: 0x00288FDC
		public static void DrawTransferableInfo(Transferable trad, Rect idRect, Color labelColor)
		{
			if (!trad.HasAnyThing && trad.IsThing)
			{
				return;
			}
			if (Mouse.IsOver(idRect))
			{
				Widgets.DrawHighlight(idRect);
			}
			Rect rect = new Rect(0f, 0f, 27f, 27f);
			if (trad.IsThing)
			{
				Widgets.ThingIcon(rect, trad.AnyThing, 1f, null);
			}
			else
			{
				trad.DrawIcon(rect);
			}
			if (trad.IsThing)
			{
				Widgets.InfoCardButton(40f, 0f, trad.AnyThing);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = new Rect(80f, 0f, idRect.width - 80f, idRect.height);
			Text.WordWrap = false;
			GUI.color = labelColor;
			Widgets.Label(rect2, trad.LabelCap);
			GUI.color = Color.white;
			Text.WordWrap = true;
			if (Mouse.IsOver(idRect))
			{
				Transferable localTrad = trad;
				TooltipHandler.TipRegion(idRect, new TipSignal(delegate()
				{
					if (!localTrad.HasAnyThing && localTrad.IsThing)
					{
						return "";
					}
					string text = localTrad.LabelCap;
					string tipDescription = localTrad.TipDescription;
					if (!tipDescription.NullOrEmpty())
					{
						text = text + ": " + tipDescription;
					}
					return text;
				}, localTrad.GetHashCode()));
			}
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x0028AEF3 File Offset: 0x002890F3
		public static float DefaultListOrderPriority(Transferable transferable)
		{
			if (!transferable.HasAnyThing)
			{
				return 0f;
			}
			return TransferableUIUtility.DefaultListOrderPriority(transferable.ThingDef);
		}

		// Token: 0x060075FB RID: 30203 RVA: 0x0028AF10 File Offset: 0x00289110
		public static float DefaultListOrderPriority(ThingDef def)
		{
			if (def == ThingDefOf.Silver)
			{
				return 100f;
			}
			if (def == ThingDefOf.Gold)
			{
				return 99f;
			}
			if (def.Minifiable)
			{
				return 90f;
			}
			if (def.IsApparel)
			{
				return 80f;
			}
			if (def.IsRangedWeapon)
			{
				return 70f;
			}
			if (def.IsMeleeWeapon)
			{
				return 60f;
			}
			if (def.isTechHediff)
			{
				return 50f;
			}
			if (def.CountAsResource)
			{
				return -10f;
			}
			return 20f;
		}

		// Token: 0x060075FC RID: 30204 RVA: 0x0028AF94 File Offset: 0x00289194
		public static void DoTransferableSorters(TransferableSorterDef sorter1, TransferableSorterDef sorter2, Action<TransferableSorterDef> sorter1Setter, Action<TransferableSorterDef> sorter2Setter)
		{
			GUI.BeginGroup(TransferableUIUtility.SortersRect);
			Text.Font = GameFont.Tiny;
			Rect rect = new Rect(0f, 0f, 60f, 27f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, "SortBy".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(rect.xMax + 10f, 0f, 130f, 27f);
			if (Widgets.ButtonText(rect2, sorter1.LabelCap, true, true, true))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter1Setter);
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 10f, 0f, 130f, 27f), sorter2.LabelCap, true, true, true))
			{
				TransferableUIUtility.OpenSorterChangeFloatMenu(sorter2Setter);
			}
			GUI.EndGroup();
		}

		// Token: 0x060075FD RID: 30205 RVA: 0x0028B068 File Offset: 0x00289268
		public static void DoTransferableSearcher(QuickSearchWidget searchWidget, Action onSearchChange)
		{
			Rect searcherRect = TransferableUIUtility.SearcherRect;
			GUI.BeginGroup(searcherRect);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, (searcherRect.height - 24f) / 2f, 170f, 24f);
			searchWidget.OnGUI(rect, onSearchChange);
			Text.Font = GameFont.Tiny;
			GUI.EndGroup();
		}

		// Token: 0x060075FE RID: 30206 RVA: 0x0028B0C4 File Offset: 0x002892C4
		public static void DoExtraAnimalIcons(Transferable trad, Rect rect, ref float curX)
		{
			Pawn pawn = trad.AnyThing as Pawn;
			if (pawn != null && pawn.RaceProps.Animal)
			{
				if (pawn.IsCaravanRideable())
				{
					Rect rect2 = new Rect(curX - TransferableUIUtility.RideableIconWidth, (rect.height - TransferableUIUtility.RideableIconWidth) / 2f, TransferableUIUtility.RideableIconWidth, TransferableUIUtility.RideableIconWidth);
					curX -= rect2.width;
					GUI.DrawTexture(rect2, TransferableUIUtility.RideableIcon);
					if (Mouse.IsOver(rect2))
					{
						TooltipHandler.TipRegion(rect2, CaravanRideableUtility.GetIconTooltipText(pawn));
					}
				}
				if (pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
				{
					Rect rect3 = new Rect(curX - TransferableUIUtility.BondIconWidth, (rect.height - TransferableUIUtility.BondIconWidth) / 2f, TransferableUIUtility.BondIconWidth, TransferableUIUtility.BondIconWidth);
					curX -= TransferableUIUtility.BondIconWidth;
					GUI.DrawTexture(rect3, TransferableUIUtility.BondIcon);
					if (Mouse.IsOver(rect3))
					{
						string iconTooltipText = TrainableUtility.GetIconTooltipText(pawn);
						if (!iconTooltipText.NullOrEmpty())
						{
							TooltipHandler.TipRegion(rect3, iconTooltipText);
						}
					}
				}
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
				{
					Rect rect4 = new Rect(curX - TransferableUIUtility.PregnancyIconWidth, (rect.height - TransferableUIUtility.PregnancyIconWidth) / 2f, TransferableUIUtility.PregnancyIconWidth, TransferableUIUtility.PregnancyIconWidth);
					curX -= TransferableUIUtility.PregnancyIconWidth;
					if (Mouse.IsOver(rect4))
					{
						TooltipHandler.TipRegion(rect4, PawnColumnWorker_Pregnant.GetTooltipText(pawn));
					}
					GUI.DrawTexture(rect4, TransferableUIUtility.PregnantIcon);
				}
			}
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x0028B244 File Offset: 0x00289444
		private static void OpenSorterChangeFloatMenu(Action<TransferableSorterDef> sorterSetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			List<TransferableSorterDef> allDefsListForReading = DefDatabase<TransferableSorterDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				TransferableSorterDef def = allDefsListForReading[i];
				list.Add(new FloatMenuOption(def.LabelCap, delegate()
				{
					sorterSetter(def);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06007600 RID: 30208 RVA: 0x0028B2E0 File Offset: 0x002894E0
		public static void DrawExtraInfo(List<TransferableUIUtility.ExtraInfo> info, Rect rect)
		{
			if (rect.width > (float)info.Count * 230f)
			{
				rect.x += Mathf.Floor((rect.width - (float)info.Count * 230f) / 2f);
				rect.width = (float)info.Count * 230f;
			}
			GUI.BeginGroup(rect);
			float num = Mathf.Floor(rect.width / (float)info.Count);
			float num2 = 0f;
			for (int i = 0; i < info.Count; i++)
			{
				float num3 = (i == info.Count - 1) ? (rect.width - num2) : num;
				Rect rect2 = new Rect(num2, 0f, num3, rect.height);
				Rect rect3 = new Rect(num2, 0f, num3, rect.height / 2f);
				Rect rect4 = new Rect(num2, rect.height / 2f, num3, rect.height / 2f);
				if (Time.time - info[i].lastFlashTime < 1f)
				{
					GUI.DrawTexture(rect2, TransferableUIUtility.FlashTex);
				}
				Text.Anchor = TextAnchor.LowerCenter;
				Text.Font = GameFont.Tiny;
				GUI.color = Color.gray;
				Widgets.Label(new Rect(rect3.x, rect3.y - 2f, rect3.width, rect3.height - -3f), info[i].key);
				Rect rect5 = new Rect(rect4.x, rect4.y + -3f + 2f, rect4.width, rect4.height - -3f);
				Text.Font = GameFont.Small;
				if (info[i].secondValue.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					GUI.color = info[i].color;
					Widgets.Label(rect5, info[i].value);
				}
				else
				{
					Rect rect6 = rect5;
					rect6.width = Mathf.Floor(rect5.width / 2f - 15f);
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = info[i].color;
					Widgets.Label(rect6, info[i].value);
					Rect rect7 = rect5;
					rect7.xMin += Mathf.Ceil(rect5.width / 2f + 15f);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = info[i].secondColor;
					Widgets.Label(rect7, info[i].secondValue);
					Rect position = rect5;
					position.x = Mathf.Floor(rect5.x + rect5.width / 2f - 7.5f);
					position.y += 3f;
					position.width = 15f;
					position.height = 15f;
					GUI.color = Color.white;
					GUI.DrawTexture(position, TransferableUIUtility.DividerTex);
				}
				GUI.color = Color.white;
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, info[i].tip);
				num2 += num3;
			}
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007601 RID: 30209 RVA: 0x0028B628 File Offset: 0x00289828
		public static void DrawCaptiveTradeInfo(Transferable trad, ITrader trader, Rect rect, ref float curX)
		{
			Pawn pawn;
			if ((pawn = (trad.AnyThing as Pawn)) == null || pawn.guest == null || !pawn.RaceProps.Humanlike)
			{
				return;
			}
			if (TransferableUIUtility.TransferableIsCaptive(trad) && (pawn.IsSlaveOfColony || pawn.IsPrisonerOfColony))
			{
				if (pawn.HomeFaction == trader.Faction)
				{
					Rect rect2 = new Rect(curX - TransferableUIUtility.SlaveTradeIconWidth, (rect.height - TransferableUIUtility.SlaveTradeIconWidth) / 2f, TransferableUIUtility.SlaveTradeIconWidth, TransferableUIUtility.SlaveTradeIconWidth);
					curX -= TransferableUIUtility.SlaveTradeIconWidth;
					GUI.DrawTexture(rect2, GuestUtility.RansomIcon);
					if (Mouse.IsOver(rect2))
					{
						TooltipHandler.TipRegion(rect2, "SellingAsRansom".Translate());
						return;
					}
				}
				else
				{
					Rect rect3 = new Rect(curX - TransferableUIUtility.SlaveTradeIconWidth, (rect.height - TransferableUIUtility.SlaveTradeIconWidth) / 2f, TransferableUIUtility.SlaveTradeIconWidth, TransferableUIUtility.SlaveTradeIconWidth);
					curX -= TransferableUIUtility.SlaveTradeIconWidth;
					GUI.DrawTexture(rect3, GuestUtility.SlaveIcon);
					if (Mouse.IsOver(rect3))
					{
						TooltipHandler.TipRegion(rect3, "SellingAsSlave".Translate());
						return;
					}
				}
			}
			else
			{
				float width = 140f;
				string label = ((pawn.guest.joinStatus == JoinStatus.JoinAsColonist) ? "JoinsAsColonist" : "JoinsAsSlave").Translate();
				Rect rect4 = new Rect(curX, 0f, width, rect.height);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect4, label);
				Text.Anchor = TextAnchor.UpperLeft;
				if (Mouse.IsOver(rect4))
				{
					Widgets.DrawHighlight(rect4);
					string key = (pawn.guest.joinStatus == JoinStatus.JoinAsColonist) ? "JoinsAsColonistDesc" : "JoinsAsSlaveDesc";
					TooltipHandler.TipRegion(rect4, key.Translate());
				}
			}
		}

		// Token: 0x06007602 RID: 30210 RVA: 0x0028B7E0 File Offset: 0x002899E0
		public static bool TransferableIsCaptive(Transferable trad)
		{
			Pawn pawn;
			return (pawn = (trad.AnyThing as Pawn)) != null && pawn.RaceProps.Humanlike && (pawn.IsSlave || pawn.IsPrisoner);
		}

		// Token: 0x06007603 RID: 30211 RVA: 0x0028B81B File Offset: 0x00289A1B
		public static bool TradeIsPlayerSellingToSlavery(Tradeable trad, Faction traderFaction)
		{
			return TransferableUIUtility.TransferableIsCaptive(trad) && trad.CountHeldBy(Transactor.Colony) > 0 && ((Pawn)trad.AnyThing).HomeFaction != traderFaction;
		}

		// Token: 0x04004158 RID: 16728
		private static List<TransferableCountToTransferStoppingPoint> stoppingPoints = new List<TransferableCountToTransferStoppingPoint>();

		// Token: 0x04004159 RID: 16729
		private const float AmountAreaWidth = 90f;

		// Token: 0x0400415A RID: 16730
		private const float AmountAreaHeight = 25f;

		// Token: 0x0400415B RID: 16731
		private const float AdjustArrowWidth = 30f;

		// Token: 0x0400415C RID: 16732
		public const float ResourceIconSize = 27f;

		// Token: 0x0400415D RID: 16733
		public const float SortersHeight = 27f;

		// Token: 0x0400415E RID: 16734
		public const float ExtraInfoHeight = 40f;

		// Token: 0x0400415F RID: 16735
		public const float ExtraInfoMargin = 12f;

		// Token: 0x04004160 RID: 16736
		public static readonly Color ZeroCountColor = new Color(0.5f, 0.5f, 0.5f);

		// Token: 0x04004161 RID: 16737
		public static readonly Texture2D FlashTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.4f));

		// Token: 0x04004162 RID: 16738
		private static readonly Texture2D TradeArrow = ContentFinder<Texture2D>.Get("UI/Widgets/TradeArrow", true);

		// Token: 0x04004163 RID: 16739
		private static readonly Texture2D DividerTex = ContentFinder<Texture2D>.Get("UI/Widgets/Divider", true);

		// Token: 0x04004164 RID: 16740
		private static readonly Texture2D PregnantIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Pregnant", true);

		// Token: 0x04004165 RID: 16741
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x04004166 RID: 16742
		private static readonly Texture2D RideableIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Rideable", true);

		// Token: 0x04004167 RID: 16743
		private static readonly Rect SortersRect = new Rect(0f, 0f, 350f, 27f);

		// Token: 0x04004168 RID: 16744
		private static readonly Rect SearcherRect = new Rect(360f, 0f, 170f, 27f);

		// Token: 0x04004169 RID: 16745
		[TweakValue("Interface", 0f, 50f)]
		private static float PregnancyIconWidth = 24f;

		// Token: 0x0400416A RID: 16746
		[TweakValue("Interface", 0f, 50f)]
		private static float BondIconWidth = 24f;

		// Token: 0x0400416B RID: 16747
		[TweakValue("Interface", 0f, 50f)]
		private static float RideableIconWidth = 24f;

		// Token: 0x0400416C RID: 16748
		[TweakValue("Interface", 0f, 50f)]
		private static float SlaveTradeIconWidth = 24f;

		// Token: 0x020026E4 RID: 9956
		public struct ExtraInfo
		{
			// Token: 0x0600D7ED RID: 55277 RVA: 0x0040DE98 File Offset: 0x0040C098
			public ExtraInfo(string key, string value, Color color, string tip, float lastFlashTime = -9999f)
			{
				this.key = key;
				this.value = value;
				this.color = color;
				this.tip = tip;
				this.lastFlashTime = lastFlashTime;
				this.secondValue = null;
				this.secondColor = default(Color);
			}

			// Token: 0x0600D7EE RID: 55278 RVA: 0x0040DED2 File Offset: 0x0040C0D2
			public ExtraInfo(string key, string value, Color color, string tip, string secondValue, Color secondColor, float lastFlashTime = -9999f)
			{
				this.key = key;
				this.value = value;
				this.color = color;
				this.tip = tip;
				this.lastFlashTime = lastFlashTime;
				this.secondValue = secondValue;
				this.secondColor = secondColor;
			}

			// Token: 0x040093BD RID: 37821
			public string key;

			// Token: 0x040093BE RID: 37822
			public string value;

			// Token: 0x040093BF RID: 37823
			public string secondValue;

			// Token: 0x040093C0 RID: 37824
			public string tip;

			// Token: 0x040093C1 RID: 37825
			public float lastFlashTime;

			// Token: 0x040093C2 RID: 37826
			public Color color;

			// Token: 0x040093C3 RID: 37827
			public Color secondColor;
		}
	}
}
