using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001343 RID: 4931
	public class ITab_Pawn_FormingCaravan : ITab
	{
		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x0600775F RID: 30559 RVA: 0x0029E9A7 File Offset: 0x0029CBA7
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsFormingCaravan();
			}
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x0029E9B4 File Offset: 0x0029CBB4
		public ITab_Pawn_FormingCaravan()
		{
			this.size = new Vector2(480f, 450f);
			this.labelKey = "TabFormingCaravan";
		}

		// Token: 0x06007761 RID: 30561 RVA: 0x0029E9E8 File Offset: 0x0029CBE8
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			string status = ((LordJob_FormAndSendCaravan)base.SelPawn.GetLord().LordJob).Status;
			Widgets.Label(new Rect(0f, num, rect.width, 100f), status);
			num += 22f;
			num += 4f;
			this.DoPeopleAndAnimals(rect, ref num);
			num += 4f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x0029EB0C File Offset: 0x0029CD0C
		public override void TabUpdate()
		{
			base.TabUpdate();
			if (base.SelPawn != null && base.SelPawn.GetLord() != null)
			{
				Lord lord = base.SelPawn.GetLord();
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], false, false, true);
				}
			}
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x0029EB70 File Offset: 0x0029CD70
		private void DoPeopleAndAnimals(Rect inRect, ref float curY)
		{
			Widgets.ListSeparator(ref curY, inRect.width, "CaravanMembers".Translate());
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.IsFreeColonist)
				{
					num++;
					if (pawn.InMentalState)
					{
						num2++;
					}
				}
				else if (pawn.IsPrisoner)
				{
					num3++;
					if (pawn.InMentalState)
					{
						num4++;
					}
				}
				else if (pawn.RaceProps.Animal)
				{
					num5++;
					if (pawn.InMentalState)
					{
						num6++;
					}
					if (pawn.RaceProps.packAnimal)
					{
						num7++;
					}
				}
			}
			string pawnsCountLabel = this.GetPawnsCountLabel(num, num2, -1);
			string pawnsCountLabel2 = this.GetPawnsCountLabel(num3, num4, -1);
			string pawnsCountLabel3 = this.GetPawnsCountLabel(num5, num6, num7);
			float y = curY;
			float num8;
			this.DoPeopleAndAnimalsEntry(inRect, Faction.OfPlayer.def.pawnsPlural.CapitalizeFirst(), pawnsCountLabel, ref curY, out num8);
			float y2 = curY;
			float num9;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanPrisoners".Translate(), pawnsCountLabel2, ref curY, out num9);
			float y3 = curY;
			float num10;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanAnimals".Translate(), pawnsCountLabel3, ref curY, out num10);
			float width = Mathf.Max(new float[]
			{
				num8,
				num9,
				num10
			}) + 2f;
			Rect rect = new Rect(0f, y, width, 22f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				this.HighlightColonists();
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectColonistsLater();
			}
			Rect rect2 = new Rect(0f, y2, width, 22f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				this.HighlightPrisoners();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.SelectPrisonersLater();
			}
			Rect rect3 = new Rect(0f, y3, width, 22f);
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				this.HighlightAnimals();
			}
			if (Widgets.ButtonInvisible(rect3, true))
			{
				this.SelectAnimalsLater();
			}
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x0029EDB0 File Offset: 0x0029CFB0
		private void DoPeopleAndAnimalsEntry(Rect inRect, string leftLabel, string rightLabel, ref float curY, out float drawnWidth)
		{
			Rect rect = new Rect(0f, curY, inRect.width, 100f);
			Widgets.Label(rect, leftLabel);
			rect.xMin += 120f;
			Widgets.Label(rect, rightLabel);
			curY += 22f;
			drawnWidth = 120f + Text.CalcSize(rightLabel).x;
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x0029EE1C File Offset: 0x0029D01C
		private void DoItemsLists(Rect inRect, ref float curY)
		{
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = (LordJob_FormAndSendCaravan)base.SelPawn.GetLord().LordJob;
			Rect position = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float a = 0f;
			GUI.BeginGroup(position);
			Widgets.ListSeparator(ref a, position.width, "ItemsToLoad".Translate());
			bool flag = false;
			for (int i = 0; i < lordJob_FormAndSendCaravan.transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = lordJob_FormAndSendCaravan.transferables[i];
				if (transferableOneWay.CountToTransfer > 0 && transferableOneWay.HasAnyThing)
				{
					flag = true;
					this.DoThingRow(transferableOneWay.ThingDef, transferableOneWay.CountToTransfer, transferableOneWay.things, position.width, ref a);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, position.width, null);
			}
			GUI.EndGroup();
			Rect position2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			GUI.BeginGroup(position2);
			Widgets.ListSeparator(ref b, position2.width, "LoadedItems".Translate());
			bool flag2 = false;
			for (int j = 0; j < lordJob_FormAndSendCaravan.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = lordJob_FormAndSendCaravan.lord.ownedPawns[j];
				if (!pawn.inventory.UnloadEverything)
				{
					for (int k = 0; k < pawn.inventory.innerContainer.Count; k++)
					{
						Thing thing = pawn.inventory.innerContainer[k];
						flag2 = true;
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
						ITab_Pawn_FormingCaravan.tmpSingleThing.Add(thing);
						this.DoThingRow(thing.def, thing.stackCount, ITab_Pawn_FormingCaravan.tmpSingleThing, position2.width, ref b);
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
					}
				}
			}
			if (!flag2)
			{
				Widgets.NoneLabel(ref b, position.width, null);
			}
			GUI.EndGroup();
			curY += Mathf.Max(a, b);
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x0029F058 File Offset: 0x0029D258
		private void SelectColonistsLater()
		{
			Lord lord = base.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x06007767 RID: 30567 RVA: 0x0029F0D0 File Offset: 0x0029D2D0
		private void HighlightColonists()
		{
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x06007768 RID: 30568 RVA: 0x0029F12C File Offset: 0x0029D32C
		private void SelectPrisonersLater()
		{
			Lord lord = base.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x0029F1A4 File Offset: 0x0029D3A4
		private void HighlightPrisoners()
		{
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600776A RID: 30570 RVA: 0x0029F200 File Offset: 0x0029D400
		private void SelectAnimalsLater()
		{
			Lord lord = base.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x0029F27C File Offset: 0x0029D47C
		private void HighlightAnimals()
		{
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600776C RID: 30572 RVA: 0x0029F2DC File Offset: 0x0029D4DC
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x0600776D RID: 30573 RVA: 0x0029F2F8 File Offset: 0x0029D4F8
		public static void SelectNow(List<Thing> things)
		{
			if (!things.Any<Thing>())
			{
				return;
			}
			Find.Selector.ClearSelection();
			bool flag = false;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].Spawned)
				{
					if (!flag)
					{
						CameraJumper.TryJump(things[i]);
					}
					Find.Selector.Select(things[i], true, true);
					flag = true;
				}
			}
			if (!flag)
			{
				CameraJumper.TryJumpAndSelect(things[0]);
			}
		}

		// Token: 0x0600776E RID: 30574 RVA: 0x0029F378 File Offset: 0x0029D578
		private string GetPawnsCountLabel(int count, int countInMentalState, int countPackAnimals)
		{
			string text = count.ToString();
			bool flag = countInMentalState > 0;
			bool flag2 = count > 0 && countPackAnimals >= 0;
			if (flag || flag2)
			{
				text += " (";
				if (flag2)
				{
					text += countPackAnimals.ToString() + " " + "PackAnimalsCountLower".Translate();
				}
				if (flag)
				{
					if (flag2)
					{
						text += ", ";
					}
					text += countInMentalState.ToString() + " " + "InMentalStateCountLower".Translate();
				}
				text += ")";
			}
			return text;
		}

		// Token: 0x0600776F RID: 30575 RVA: 0x0029F430 File Offset: 0x0029D630
		private void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_FormingCaravan.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thingDef.DrawMatSingle != null && thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f, null);
				}
				else
				{
					Widgets.ThingIcon(rect2, thingDef, null, null, 1f, null);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_FormingCaravan.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string str;
			if (things.Count == 1)
			{
				str = things[0].LabelCap;
			}
			else
			{
				str = GenLabel.ThingLabel(thingDef, null, count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, str);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x04004255 RID: 16981
		private Vector2 scrollPosition;

		// Token: 0x04004256 RID: 16982
		private float lastDrawnHeight;

		// Token: 0x04004257 RID: 16983
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x04004258 RID: 16984
		private static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x04004259 RID: 16985
		private const float TopPadding = 20f;

		// Token: 0x0400425A RID: 16986
		private const float StandardLineHeight = 22f;

		// Token: 0x0400425B RID: 16987
		private const float ExtraSpaceBetweenSections = 4f;

		// Token: 0x0400425C RID: 16988
		private const float SpaceBetweenItemsLists = 10f;

		// Token: 0x0400425D RID: 16989
		private const float ThingRowHeight = 28f;

		// Token: 0x0400425E RID: 16990
		private const float ThingIconSize = 28f;

		// Token: 0x0400425F RID: 16991
		private const float ThingLeftX = 36f;

		// Token: 0x04004260 RID: 16992
		private static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x04004261 RID: 16993
		private static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x04004262 RID: 16994
		private static List<Thing> tmpPawns = new List<Thing>();
	}
}
