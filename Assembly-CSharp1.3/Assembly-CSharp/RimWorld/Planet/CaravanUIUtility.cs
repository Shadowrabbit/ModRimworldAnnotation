using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017B6 RID: 6070
	public static class CaravanUIUtility
	{
		// Token: 0x06008CCF RID: 36047 RVA: 0x0032914C File Offset: 0x0032734C
		public static void CreateCaravanTransferableWidgets(List<TransferableOneWay> transferables, out TransferableOneWayWidget pawnsTransfer, out TransferableOneWayWidget itemsTransfer, out TransferableOneWayWidget travelSuppliesTransfer, string thingCountTip, IgnorePawnsInventoryMode ignorePawnInventoryMass, Func<float> availableMassGetter, bool ignoreSpawnedCorpsesGearAndInventoryMass, int tile, bool playerPawnsReadOnly = false)
		{
			pawnsTransfer = new TransferableOneWayWidget(null, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, true, true, false, true, false, playerPawnsReadOnly, false);
			CaravanUIUtility.AddPawnsSections(pawnsTransfer, transferables);
			itemsTransfer = new TransferableOneWayWidget(from x in transferables
			where CaravanUIUtility.GetTransferableCategory(x) == CaravanUIUtility.TransferableCategory.Item
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, false, false, true, false, true, false, false);
			travelSuppliesTransfer = new TransferableOneWayWidget(from x in transferables
			where CaravanUIUtility.GetTransferableCategory(x) == CaravanUIUtility.TransferableCategory.TravelSupplies
			select x, null, null, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, false, false, true, false, true, false, false);
		}

		// Token: 0x06008CD0 RID: 36048 RVA: 0x00329214 File Offset: 0x00327414
		private static CaravanUIUtility.TransferableCategory GetTransferableCategory(Transferable t)
		{
			if (t.ThingDef.category == ThingCategory.Pawn)
			{
				return CaravanUIUtility.TransferableCategory.Pawn;
			}
			if ((!t.ThingDef.thingCategories.NullOrEmpty<ThingCategoryDef>() && t.ThingDef.thingCategories.Contains(ThingCategoryDefOf.Medicine)) || (t.ThingDef.IsIngestible && !t.ThingDef.IsDrug && !t.ThingDef.IsCorpse) || (t.AnyThing.GetInnerIfMinified().def.IsBed && t.AnyThing.GetInnerIfMinified().def.building.bed_caravansCanUse))
			{
				return CaravanUIUtility.TransferableCategory.TravelSupplies;
			}
			return CaravanUIUtility.TransferableCategory.Item;
		}

		// Token: 0x06008CD1 RID: 36049 RVA: 0x003292B8 File Offset: 0x003274B8
		public static void AddPawnsSections(TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
		{
			IEnumerable<TransferableOneWay> source = from x in transferables
			where x.ThingDef.category == ThingCategory.Pawn
			select x;
			widget.AddSection("ColonistsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsFreeNonSlaveColonist
			select x);
			if (ModsConfig.IdeologyActive)
			{
				widget.AddSection("SlavesSection".Translate(), from x in source
				where ((Pawn)x.AnyThing).IsSlave
				select x);
			}
			widget.AddSection("PrisonersSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsPrisoner
			select x);
			widget.AddSection("CaptureSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).Downed && CaravanUtility.ShouldAutoCapture((Pawn)x.AnyThing, Faction.OfPlayer)
			select x);
			widget.AddSection("AnimalsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).RaceProps.Animal
			select x);
		}

		// Token: 0x06008CD2 RID: 36050 RVA: 0x00329414 File Offset: 0x00327614
		private static string GetDaysWorthOfFoodLabel(Pair<float, float> daysWorthOfFood, bool multiline)
		{
			if (daysWorthOfFood.First >= 600f)
			{
				return "InfiniteDaysWorthOfFoodInfo".Translate();
			}
			string text = daysWorthOfFood.First.ToString("0.#");
			string str = multiline ? "\n" : " ";
			if (daysWorthOfFood.Second < 600f && daysWorthOfFood.Second < daysWorthOfFood.First)
			{
				text += str + "(" + "DaysWorthOfFoodInfoRot".Translate(daysWorthOfFood.Second.ToString("0.#") + ")");
			}
			return text;
		}

		// Token: 0x06008CD3 RID: 36051 RVA: 0x003294CC File Offset: 0x003276CC
		private static Color GetDaysWorthOfFoodColor(Pair<float, float> daysWorthOfFood, int? ticksToArrive)
		{
			if (daysWorthOfFood.First >= 600f)
			{
				return Color.white;
			}
			float num = Mathf.Min(daysWorthOfFood.First, daysWorthOfFood.Second);
			if (ticksToArrive != null)
			{
				return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodKnownRouteColor, num / ((float)ticksToArrive.Value / 60000f));
			}
			return GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodColor, num);
		}

		// Token: 0x06008CD4 RID: 36052 RVA: 0x00329530 File Offset: 0x00327730
		private static string GetMovementSpeedLabel(float tilesPerDay, bool immobile, bool shortVersion)
		{
			if (immobile)
			{
				return shortVersion ? "TilesPerDayImmobileShort".Translate() : "TilesPerDayImmobile".Translate();
			}
			return tilesPerDay.ToString("0.#") + " " + "TilesPerDay".Translate();
		}

		// Token: 0x06008CD5 RID: 36053 RVA: 0x00329589 File Offset: 0x00327789
		private static Color GetMovementSpeedColor(float tilesPerDay, bool immobile)
		{
			if (!immobile)
			{
				return GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, tilesPerDay);
			}
			return Color.red;
		}

		// Token: 0x06008CD6 RID: 36054 RVA: 0x003295A0 File Offset: 0x003277A0
		public static void DrawCaravanInfo(CaravanUIUtility.CaravanInfo info, CaravanUIUtility.CaravanInfo? info2, int currentTile, int? ticksToArrive, float lastMassFlashTime, Rect rect, bool lerpMassColor = true, string extraDaysWorthOfFoodTipInfo = null, bool multiline = false)
		{
			CaravanUIUtility.tmpInfo.Clear();
			TaggedString taggedString = info.massUsage.ToStringEnsureThreshold(info.massCapacity, 0) + " / " + info.massCapacity.ToString("F0") + " " + "kg".Translate();
			TaggedString taggedString2;
			if (info2 == null)
			{
				taggedString2 = null;
			}
			else
			{
				string str = info2.Value.massUsage.ToStringEnsureThreshold(info2.Value.massCapacity, 0);
				string str2 = " / ";
				CaravanUIUtility.CaravanInfo value = info2.Value;
				taggedString2 = str + str2 + value.massCapacity.ToString("F0") + " " + "kg".Translate();
			}
			TaggedString taggedString3 = taggedString2;
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Mass".Translate(), taggedString, CaravanUIUtility.GetMassColor(info.massUsage, info.massCapacity, lerpMassColor), CaravanUIUtility.GetMassTip(info.massUsage, info.massCapacity, info.massCapacityExplanation, (info2 != null) ? new float?(info2.Value.massUsage) : null, (info2 != null) ? new float?(info2.Value.massCapacity) : null, (info2 != null) ? info2.Value.massCapacityExplanation : null), taggedString3, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.massUsage, info2.Value.massCapacity, lerpMassColor) : Color.white, lastMassFlashTime));
			if (info.extraMassUsage != -1f)
			{
				TaggedString taggedString4 = info.extraMassUsage.ToStringEnsureThreshold(info.extraMassCapacity, 0) + " / " + info.extraMassCapacity.ToString("F0") + " " + "kg".Translate();
				TaggedString taggedString5;
				if (info2 == null)
				{
					taggedString5 = null;
				}
				else
				{
					string str3 = info2.Value.extraMassUsage.ToStringEnsureThreshold(info2.Value.extraMassCapacity, 0);
					string str4 = " / ";
					CaravanUIUtility.CaravanInfo value = info2.Value;
					taggedString5 = str3 + str4 + value.extraMassCapacity.ToString("F0") + " " + "kg".Translate();
				}
				TaggedString taggedString6 = taggedString5;
				CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMass".Translate(), taggedString4, CaravanUIUtility.GetMassColor(info.extraMassUsage, info.extraMassCapacity, true), CaravanUIUtility.GetMassTip(info.extraMassUsage, info.extraMassCapacity, info.extraMassCapacityExplanation, (info2 != null) ? new float?(info2.Value.extraMassUsage) : null, (info2 != null) ? new float?(info2.Value.extraMassCapacity) : null, (info2 != null) ? info2.Value.extraMassCapacityExplanation : null), taggedString6, (info2 != null) ? CaravanUIUtility.GetMassColor(info2.Value.extraMassUsage, info2.Value.extraMassCapacity, true) : Color.white, -9999f));
			}
			string text = "CaravanMovementSpeedTip".Translate();
			if (!info.tilesPerDayExplanation.NullOrEmpty())
			{
				text = text + "\n\n" + info.tilesPerDayExplanation;
			}
			if (info2 != null && !info2.Value.tilesPerDayExplanation.NullOrEmpty())
			{
				text = text + "\n\n-----\n\n" + info2.Value.tilesPerDayExplanation;
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMovementSpeed".Translate(), CaravanUIUtility.GetMovementSpeedLabel(info.tilesPerDay, info.massUsage > info.massCapacity, info2 != null), CaravanUIUtility.GetMovementSpeedColor(info.tilesPerDay, info.massUsage > info.massCapacity), text, (info2 != null) ? CaravanUIUtility.GetMovementSpeedLabel(info2.Value.tilesPerDay, info2.Value.massUsage > info2.Value.massCapacity, true) : null, (info2 != null) ? CaravanUIUtility.GetMovementSpeedColor(info2.Value.tilesPerDay, info2.Value.massUsage > info2.Value.massCapacity) : Color.white, -9999f));
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("DaysWorthOfFood".Translate(), CaravanUIUtility.GetDaysWorthOfFoodLabel(info.daysWorthOfFood, multiline), CaravanUIUtility.GetDaysWorthOfFoodColor(info.daysWorthOfFood, ticksToArrive), "DaysWorthOfFoodTooltip".Translate() + extraDaysWorthOfFoodTipInfo + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(currentTile, Find.TickManager.TicksAbs), (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodLabel(info2.Value.daysWorthOfFood, multiline) : null, (info2 != null) ? CaravanUIUtility.GetDaysWorthOfFoodColor(info2.Value.daysWorthOfFood, ticksToArrive) : Color.white, -9999f));
			string text2 = info.foragedFoodPerDay.Second.ToString("0.#");
			string text3;
			if (info2 == null)
			{
				text3 = null;
			}
			else
			{
				CaravanUIUtility.CaravanInfo value = info2.Value;
				text3 = value.foragedFoodPerDay.Second.ToString("0.#");
			}
			string text4 = text3;
			TaggedString taggedString7 = "ForagedFoodPerDayTip".Translate();
			taggedString7 += "\n\n" + info.foragedFoodPerDayExplanation;
			if (info2 != null)
			{
				taggedString7 += "\n\n-----\n\n" + info2.Value.foragedFoodPerDayExplanation;
			}
			if (info.foragedFoodPerDay.Second <= 0f)
			{
				if (info2 == null)
				{
					goto IL_6A6;
				}
				CaravanUIUtility.CaravanInfo value = info2.Value;
				if (value.foragedFoodPerDay.Second <= 0f)
				{
					goto IL_6A6;
				}
			}
			string text5 = multiline ? "\n" : " ";
			if (info2 == null)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					text5,
					"(",
					info.foragedFoodPerDay.First.label,
					")"
				});
			}
			else
			{
				string[] array = new string[5];
				array[0] = text4;
				array[1] = text5;
				array[2] = "(";
				int num = 3;
				CaravanUIUtility.CaravanInfo value = info2.Value;
				array[num] = value.foragedFoodPerDay.First.label.Truncate(50f, null);
				array[4] = ")";
				text4 = string.Concat(array);
			}
			IL_6A6:
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("ForagedFoodPerDay".Translate(), text2, Color.white, taggedString7, text4, Color.white, -9999f));
			string text6 = "CaravanVisibilityTip".Translate();
			if (!info.visibilityExplanation.NullOrEmpty())
			{
				text6 = text6 + "\n\n" + info.visibilityExplanation;
			}
			if (info2 != null && !info2.Value.visibilityExplanation.NullOrEmpty())
			{
				text6 = text6 + "\n\n-----\n\n" + info2.Value.visibilityExplanation;
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Visibility".Translate(), info.visibility.ToStringPercent(), GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info.visibility), text6, (info2 != null) ? info2.Value.visibility.ToStringPercent() : null, (info2 != null) ? GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info2.Value.visibility) : Color.white, -9999f));
			TransferableUIUtility.DrawExtraInfo(CaravanUIUtility.tmpInfo, rect);
		}

		// Token: 0x06008CD7 RID: 36055 RVA: 0x00329D81 File Offset: 0x00327F81
		private static Color GetMassColor(float massUsage, float massCapacity, bool lerpMassColor)
		{
			if (massCapacity == 0f)
			{
				return Color.white;
			}
			if (massUsage > massCapacity)
			{
				return Color.red;
			}
			if (lerpMassColor)
			{
				return GenUI.LerpColor(CaravanUIUtility.MassColor, massUsage / massCapacity);
			}
			return Color.white;
		}

		// Token: 0x06008CD8 RID: 36056 RVA: 0x00329DB4 File Offset: 0x00327FB4
		private static string GetMassTip(float massUsage, float massCapacity, string massCapacityExplanation, float? massUsage2, float? massCapacity2, string massCapacity2Explanation)
		{
			TaggedString taggedString = "MassCarriedSimple".Translate() + ": " + massUsage.ToStringEnsureThreshold(massCapacity, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity.ToString("F2") + " " + "kg".Translate();
			if (massUsage2 != null)
			{
				taggedString += "\n <-> \n" + "MassCarriedSimple".Translate() + ": " + massUsage2.Value.ToStringEnsureThreshold(massCapacity2.Value, 2) + " " + "kg".Translate() + "\n" + "MassCapacity".Translate() + ": " + massCapacity2.Value.ToString("F2") + " " + "kg".Translate();
			}
			taggedString += "\n\n" + "CaravanMassUsageTooltip".Translate();
			if (!massCapacityExplanation.NullOrEmpty())
			{
				taggedString += "\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacityExplanation;
			}
			if (!massCapacity2Explanation.NullOrEmpty())
			{
				taggedString += "\n\n-----\n\n" + "MassCapacity".Translate() + ":" + "\n" + massCapacity2Explanation;
			}
			return taggedString;
		}

		// Token: 0x0400594D RID: 22861
		private static readonly List<Pair<float, Color>> MassColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.37f, Color.green),
			new Pair<float, Color>(0.82f, Color.yellow),
			new Pair<float, Color>(1f, new Color(1f, 0.6f, 0f))
		};

		// Token: 0x0400594E RID: 22862
		private static readonly List<Pair<float, Color>> TilesPerDayColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.001f, ColorLibrary.RedReadable),
			new Pair<float, Color>(1f, Color.yellow),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x0400594F RID: 22863
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(1f, Color.red),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x04005950 RID: 22864
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodKnownRouteColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.3f, ColorLibrary.RedReadable),
			new Pair<float, Color>(0.9f, Color.yellow),
			new Pair<float, Color>(1.02f, Color.green)
		};

		// Token: 0x04005951 RID: 22865
		private static readonly List<Pair<float, Color>> VisibilityColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.01f, Color.green),
			new Pair<float, Color>(0.2f, Color.green),
			new Pair<float, Color>(1f, Color.white),
			new Pair<float, Color>(1.2f, ColorLibrary.RedReadable)
		};

		// Token: 0x04005952 RID: 22866
		private static List<TransferableUIUtility.ExtraInfo> tmpInfo = new List<TransferableUIUtility.ExtraInfo>();

		// Token: 0x02002A0A RID: 10762
		private enum TransferableCategory
		{
			// Token: 0x04009DE6 RID: 40422
			Pawn,
			// Token: 0x04009DE7 RID: 40423
			Item,
			// Token: 0x04009DE8 RID: 40424
			TravelSupplies
		}

		// Token: 0x02002A0B RID: 10763
		public struct CaravanInfo
		{
			// Token: 0x0600E3CB RID: 58315 RVA: 0x0042A0A0 File Offset: 0x004282A0
			public CaravanInfo(float massUsage, float massCapacity, string massCapacityExplanation, float tilesPerDay, string tilesPerDayExplanation, Pair<float, float> daysWorthOfFood, Pair<ThingDef, float> foragedFoodPerDay, string foragedFoodPerDayExplanation, float visibility, string visibilityExplanation, float extraMassUsage = -1f, float extraMassCapacity = -1f, string extraMassCapacityExplanation = null)
			{
				this.massUsage = massUsage;
				this.massCapacity = massCapacity;
				this.massCapacityExplanation = massCapacityExplanation;
				this.tilesPerDay = tilesPerDay;
				this.tilesPerDayExplanation = tilesPerDayExplanation;
				this.daysWorthOfFood = daysWorthOfFood;
				this.foragedFoodPerDay = foragedFoodPerDay;
				this.foragedFoodPerDayExplanation = foragedFoodPerDayExplanation;
				this.visibility = visibility;
				this.visibilityExplanation = visibilityExplanation;
				this.extraMassUsage = extraMassUsage;
				this.extraMassCapacity = extraMassCapacity;
				this.extraMassCapacityExplanation = extraMassCapacityExplanation;
			}

			// Token: 0x04009DE9 RID: 40425
			public float massUsage;

			// Token: 0x04009DEA RID: 40426
			public float massCapacity;

			// Token: 0x04009DEB RID: 40427
			public string massCapacityExplanation;

			// Token: 0x04009DEC RID: 40428
			public float tilesPerDay;

			// Token: 0x04009DED RID: 40429
			public string tilesPerDayExplanation;

			// Token: 0x04009DEE RID: 40430
			public Pair<float, float> daysWorthOfFood;

			// Token: 0x04009DEF RID: 40431
			public Pair<ThingDef, float> foragedFoodPerDay;

			// Token: 0x04009DF0 RID: 40432
			public string foragedFoodPerDayExplanation;

			// Token: 0x04009DF1 RID: 40433
			public float visibility;

			// Token: 0x04009DF2 RID: 40434
			public string visibilityExplanation;

			// Token: 0x04009DF3 RID: 40435
			public float extraMassUsage;

			// Token: 0x04009DF4 RID: 40436
			public float extraMassCapacity;

			// Token: 0x04009DF5 RID: 40437
			public string extraMassCapacityExplanation;
		}
	}
}
