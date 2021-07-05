using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200180C RID: 6156
	[StaticConstructorOnStartup]
	public static class CaravanThingsTabUtility
	{
		// Token: 0x06009009 RID: 36873 RVA: 0x00339A10 File Offset: 0x00337C10
		public static void DoAbandonButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 1383004931));
			}
		}

		// Token: 0x0600900A RID: 36874 RVA: 0x00339AA4 File Offset: 0x00337CA4
		public static void DoAbandonButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonOrBanishViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, false), Gen.HashCombineInt(t.GetHashCode(), 8476546));
			}
		}

		// Token: 0x0600900B RID: 36875 RVA: 0x00339B38 File Offset: 0x00337D38
		public static void DoAbandonSpecificCountButton(Rect rowRect, Thing t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
			}
		}

		// Token: 0x0600900C RID: 36876 RVA: 0x00339BCC File Offset: 0x00337DCC
		public static void DoAbandonSpecificCountButton(Rect rowRect, TransferableImmutable t, Caravan caravan)
		{
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true))
			{
				CaravanAbandonOrBanishUtility.TryAbandonSpecificCountViaInterface(t, caravan);
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => CaravanAbandonOrBanishUtility.GetAbandonOrBanishButtonTooltip(t, true), Gen.HashCombineInt(t.GetHashCode(), 1163428609));
			}
		}

		// Token: 0x0600900D RID: 36877 RVA: 0x00339C60 File Offset: 0x00337E60
		public static void DoOpenSpecificTabButton(Rect rowRect, Pawn p, ref Pawn specificTabForPawn)
		{
			Color baseColor = (p == specificTabForPawn) ? CaravanThingsTabUtility.OpenedSpecificTabButtonColor : Color.white;
			Color mouseoverColor = (p == specificTabForPawn) ? CaravanThingsTabUtility.OpenedSpecificTabButtonMouseoverColor : GenUI.MouseoverColor;
			Rect rect = new Rect(rowRect.width - 24f, (rowRect.height - 24f) / 2f, 24f, 24f);
			if (Widgets.ButtonImage(rect, CaravanThingsTabUtility.SpecificTabButtonTex, baseColor, mouseoverColor, true))
			{
				if (p == specificTabForPawn)
				{
					specificTabForPawn = null;
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					specificTabForPawn = p;
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegionByKey(rect, "OpenSpecificTabButtonTip");
			GUI.color = Color.white;
		}

		// Token: 0x0600900E RID: 36878 RVA: 0x00339D07 File Offset: 0x00337F07
		public static void DoOpenSpecificTabButtonInvisible(Rect rect, Pawn pawn, ref Pawn specificTabForPawn)
		{
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (pawn == specificTabForPawn)
				{
					specificTabForPawn = null;
				}
				else
				{
					specificTabForPawn = pawn;
				}
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600900F RID: 36879 RVA: 0x00339D2C File Offset: 0x00337F2C
		public static void DrawMass(TransferableImmutable transferable, Rect rect)
		{
			float num = 0f;
			for (int i = 0; i < transferable.things.Count; i++)
			{
				num += transferable.things[i].GetStatValue(StatDefOf.Mass, true) * (float)transferable.things[i].stackCount;
			}
			CaravanThingsTabUtility.DrawMass(num, rect);
		}

		// Token: 0x06009010 RID: 36880 RVA: 0x00339D89 File Offset: 0x00337F89
		public static void DrawMass(Thing thing, Rect rect)
		{
			CaravanThingsTabUtility.DrawMass(thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount, rect);
		}

		// Token: 0x06009011 RID: 36881 RVA: 0x00339DA5 File Offset: 0x00337FA5
		private static void DrawMass(float mass, Rect rect)
		{
			GUI.color = TransferableOneWayWidget.ItemMassColor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect, mass.ToStringMass());
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x04005A91 RID: 23185
		public const float MassColumnWidth = 60f;

		// Token: 0x04005A92 RID: 23186
		public const float SpaceAroundIcon = 4f;

		// Token: 0x04005A93 RID: 23187
		public const float SpecificTabButtonSize = 24f;

		// Token: 0x04005A94 RID: 23188
		public const float AbandonButtonSize = 24f;

		// Token: 0x04005A95 RID: 23189
		public const float AbandonSpecificCountButtonSize = 24f;

		// Token: 0x04005A96 RID: 23190
		public static readonly Texture2D AbandonButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/Abandon", true);

		// Token: 0x04005A97 RID: 23191
		public static readonly Texture2D AbandonSpecificCountButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/AbandonSpecificCount", true);

		// Token: 0x04005A98 RID: 23192
		public static readonly Texture2D SpecificTabButtonTex = ContentFinder<Texture2D>.Get("UI/Buttons/OpenSpecificTab", true);

		// Token: 0x04005A99 RID: 23193
		public static readonly Color OpenedSpecificTabButtonColor = new Color(0f, 0.8f, 0f);

		// Token: 0x04005A9A RID: 23194
		public static readonly Color OpenedSpecificTabButtonMouseoverColor = new Color(0f, 0.5f, 0f);
	}
}
