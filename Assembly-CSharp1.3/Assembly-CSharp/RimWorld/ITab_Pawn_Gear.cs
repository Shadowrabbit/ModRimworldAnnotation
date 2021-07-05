using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001344 RID: 4932
	public class ITab_Pawn_Gear : ITab
	{
		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x06007771 RID: 30577 RVA: 0x0029F640 File Offset: 0x0029D840
		public override bool IsVisible
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return this.ShouldShowInventory(selPawnForGear) || this.ShouldShowApparel(selPawnForGear) || this.ShouldShowEquipment(selPawnForGear);
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x06007772 RID: 30578 RVA: 0x0029F670 File Offset: 0x0029D870
		private bool CanControl
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return !selPawnForGear.Downed && !selPawnForGear.InMentalState && (selPawnForGear.Faction == Faction.OfPlayer || selPawnForGear.IsPrisonerOfColony) && (!selPawnForGear.IsPrisonerOfColony || !selPawnForGear.Spawned || selPawnForGear.Map.mapPawns.AnyFreeColonistSpawned) && (!selPawnForGear.IsPrisonerOfColony || (!PrisonBreakUtility.IsPrisonBreaking(selPawnForGear) && (selPawnForGear.CurJob == null || !selPawnForGear.CurJob.exitMapOnArrival)));
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06007773 RID: 30579 RVA: 0x0029F6F9 File Offset: 0x0029D8F9
		private bool CanControlColonist
		{
			get
			{
				return this.CanControl && this.SelPawnForGear.IsColonistPlayerControlled;
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06007774 RID: 30580 RVA: 0x0029F710 File Offset: 0x0029D910
		private Pawn SelPawnForGear
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				throw new InvalidOperationException("Gear tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		// Token: 0x06007775 RID: 30581 RVA: 0x0029F757 File Offset: 0x0029D957
		public ITab_Pawn_Gear()
		{
			this.size = new Vector2(460f, 450f);
			this.labelKey = "TabGear";
			this.tutorTag = "Gear";
		}

		// Token: 0x06007776 RID: 30582 RVA: 0x0029F798 File Offset: 0x0029D998
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f).ContractedBy(10f);
			Rect position = new Rect(rect.x, rect.y, rect.width, rect.height);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, position.width, position.height);
			Rect viewRect = new Rect(0f, 0f, position.width - 16f, this.scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			this.TryDrawMassInfo(ref num, viewRect.width);
			this.TryDrawComfyTemperatureRange(ref num, viewRect.width);
			if (this.ShouldShowOverallArmor(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "OverallArmor".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Sharp, "ArmorSharp".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Blunt, "ArmorBlunt".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Heat, "ArmorHeat".Translate());
			}
			if (this.ShouldShowEquipment(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Equipment".Translate());
				ITab_Pawn_Gear.workingEquipmentList.Clear();
				ITab_Pawn_Gear.workingEquipmentList.AddRange(this.SelPawnForGear.equipment.AllEquipmentListForReading);
				ITab_Pawn_Gear.workingEquipmentList.AddRange(from x in this.SelPawnForGear.apparel.WornApparel
				where x.def.apparel.layers.Contains(ApparelLayerDefOf.Belt)
				select x);
				foreach (Thing thing in ITab_Pawn_Gear.workingEquipmentList)
				{
					this.DrawThingRow(ref num, viewRect.width, thing, false);
				}
			}
			if (this.ShouldShowApparel(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Apparel".Translate());
				foreach (Apparel thing2 in from x in this.SelPawnForGear.apparel.WornApparel
				where !x.def.apparel.layers.Contains(ApparelLayerDefOf.Belt)
				select x into ap
				orderby ap.def.apparel.bodyPartGroups[0].listOrder descending
				select ap)
				{
					this.DrawThingRow(ref num, viewRect.width, thing2, false);
				}
			}
			if (this.ShouldShowInventory(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Inventory".Translate());
				ITab_Pawn_Gear.workingInvList.Clear();
				ITab_Pawn_Gear.workingInvList.AddRange(this.SelPawnForGear.inventory.innerContainer);
				for (int i = 0; i < ITab_Pawn_Gear.workingInvList.Count; i++)
				{
					this.DrawThingRow(ref num, viewRect.width, ITab_Pawn_Gear.workingInvList[i], true);
				}
				ITab_Pawn_Gear.workingInvList.Clear();
			}
			if (Event.current.type == EventType.Layout)
			{
				if (num + 70f > 450f)
				{
					this.size.y = Mathf.Min(num + 70f, (float)(UI.screenHeight - 35) - 165f - 30f);
				}
				else
				{
					this.size.y = 450f;
				}
				this.scrollViewHeight = num + 20f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007777 RID: 30583 RVA: 0x0029FBE4 File Offset: 0x0029DDE4
		private void DrawThingRow(ref float y, float width, Thing thing, bool inventory = false)
		{
			Rect rect = new Rect(0f, y, width, 28f);
			Widgets.InfoCardButton(rect.width - 24f, y, thing);
			rect.width -= 24f;
			bool flag = false;
			if (this.CanControl && (inventory || this.CanControlColonist || (this.SelPawnForGear.Spawned && !this.SelPawnForGear.Map.IsPlayerHome)))
			{
				Rect rect2 = new Rect(rect.width - 24f, y, 24f, 24f);
				bool flag2 = false;
				if (this.SelPawnForGear.IsQuestLodger())
				{
					flag2 = (inventory || !EquipmentUtility.QuestLodgerCanUnequip(thing, this.SelPawnForGear));
				}
				Apparel apparel;
				bool flag3 = (apparel = (thing as Apparel)) != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.IsLocked(apparel);
				flag = (flag2 || flag3);
				if (Mouse.IsOver(rect2))
				{
					if (flag3)
					{
						TooltipHandler.TipRegion(rect2, "DropThingLocked".Translate());
					}
					else if (flag2)
					{
						TooltipHandler.TipRegion(rect2, "DropThingLodger".Translate());
					}
					else
					{
						TooltipHandler.TipRegion(rect2, "DropThing".Translate());
					}
				}
				Color color = flag ? Color.grey : Color.white;
				Color mouseoverColor = flag ? color : GenUI.MouseoverColor;
				if (Widgets.ButtonImage(rect2, TexButton.Drop, color, mouseoverColor, !flag) && !flag)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.InterfaceDrop(thing);
				}
				rect.width -= 24f;
			}
			if (this.CanControlColonist)
			{
				if (FoodUtility.WillIngestFromInventoryNow(this.SelPawnForGear, thing))
				{
					Rect rect3 = new Rect(rect.width - 24f, y, 24f, 24f);
					TooltipHandler.TipRegionByKey(rect3, "ConsumeThing", thing.LabelNoCount, thing);
					if (Widgets.ButtonImage(rect3, TexButton.Ingest, true))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						FoodUtility.IngestFromInventoryNow(this.SelPawnForGear, thing);
					}
				}
				rect.width -= 24f;
			}
			Rect rect4 = rect;
			rect4.xMin = rect4.xMax - 60f;
			CaravanThingsTabUtility.DrawMass(thing, rect4);
			rect.width -= 60f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_Gear.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thing.def.DrawMatSingle != null && thing.def.DrawMatSingle.mainTexture != null)
			{
				Widgets.ThingIcon(new Rect(4f, y, 28f, 28f), thing, 1f, null);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_Gear.ThingLabelColor;
			Rect rect5 = new Rect(36f, y, rect.width - 36f, rect.height);
			string text = thing.LabelCap;
			Apparel apparel2 = thing as Apparel;
			if (apparel2 != null && this.SelPawnForGear.outfits != null && this.SelPawnForGear.outfits.forcedHandler.IsForced(apparel2))
			{
				text += ", " + "ApparelForcedLower".Translate();
			}
			if (flag)
			{
				text += " (" + "ApparelLockedLower".Translate() + ")";
			}
			Text.WordWrap = false;
			Widgets.Label(rect5, text.Truncate(rect5.width, null));
			Text.WordWrap = true;
			if (Mouse.IsOver(rect))
			{
				string text2 = thing.DescriptionDetailed;
				if (thing.def.useHitPoints)
				{
					text2 = string.Concat(new object[]
					{
						text2,
						"\n",
						thing.HitPoints,
						" / ",
						thing.MaxHitPoints
					});
				}
				TooltipHandler.TipRegion(rect, text2);
			}
			y += 28f;
		}

		// Token: 0x06007778 RID: 30584 RVA: 0x002A0014 File Offset: 0x0029E214
		private void TryDrawOverallArmor(ref float curY, float width, StatDef stat, string label)
		{
			float num = 0f;
			float num2 = Mathf.Clamp01(this.SelPawnForGear.GetStatValue(stat, true) / 2f);
			List<BodyPartRecord> allParts = this.SelPawnForGear.RaceProps.body.AllParts;
			List<Apparel> list = (this.SelPawnForGear.apparel != null) ? this.SelPawnForGear.apparel.WornApparel : null;
			for (int i = 0; i < allParts.Count; i++)
			{
				float num3 = 1f - num2;
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].def.apparel.CoversBodyPart(allParts[i]))
						{
							float num4 = Mathf.Clamp01(list[j].GetStatValue(stat, true) / 2f);
							num3 *= 1f - num4;
						}
					}
				}
				num += allParts[i].coverageAbs * (1f - num3);
			}
			num = Mathf.Clamp(num * 2f, 0f, 2f);
			Rect rect = new Rect(0f, curY, width, 100f);
			Widgets.Label(rect, label.Truncate(120f, null));
			rect.xMin += 120f;
			Widgets.Label(rect, num.ToStringPercent());
			curY += 22f;
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x002A0180 File Offset: 0x0029E380
		private void TryDrawMassInfo(ref float curY, float width)
		{
			if (this.SelPawnForGear.Dead || !this.ShouldShowInventory(this.SelPawnForGear))
			{
				return;
			}
			Rect rect = new Rect(0f, curY, width, 22f);
			float num = MassUtility.GearAndInventoryMass(this.SelPawnForGear);
			float num2 = MassUtility.Capacity(this.SelPawnForGear, null);
			Widgets.Label(rect, "MassCarried".Translate(num.ToString("0.##"), num2.ToString("0.##")));
			curY += 22f;
		}

		// Token: 0x0600777A RID: 30586 RVA: 0x002A0210 File Offset: 0x0029E410
		private void TryDrawComfyTemperatureRange(ref float curY, float width)
		{
			if (this.SelPawnForGear.Dead)
			{
				return;
			}
			Rect rect = new Rect(0f, curY, width, 22f);
			float statValue = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMin, true);
			float statValue2 = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
			Widgets.Label(rect, "ComfyTemperatureRange".Translate() + ": " + statValue.ToStringTemperature("F0") + " ~ " + statValue2.ToStringTemperature("F0"));
			curY += 22f;
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x002A02B0 File Offset: 0x0029E4B0
		private void InterfaceDrop(Thing t)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			Apparel apparel = t as Apparel;
			if (apparel != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.WornApparel.Contains(apparel))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel), new JobTag?(JobTag.Misc), false);
				return;
			}
			if (thingWithComps != null && this.SelPawnForGear.equipment != null && this.SelPawnForGear.equipment.AllEquipmentListForReading.Contains(thingWithComps))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, thingWithComps), new JobTag?(JobTag.Misc), false);
				return;
			}
			if (!t.def.destroyOnDrop)
			{
				Thing thing;
				this.SelPawnForGear.inventory.innerContainer.TryDrop(t, this.SelPawnForGear.Position, this.SelPawnForGear.Map, ThingPlaceMode.Near, out thing, null, null);
			}
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x002A03AC File Offset: 0x0029E5AC
		[Obsolete("Will be removed in a future update, use FoodUtility.IngestFromInventoryNow()")]
		private void InterfaceIngest(Thing t)
		{
			FoodUtility.IngestFromInventoryNow(this.SelPawnForGear, t);
		}

		// Token: 0x0600777D RID: 30589 RVA: 0x002A03BA File Offset: 0x0029E5BA
		private bool ShouldShowInventory(Pawn p)
		{
			return p.RaceProps.Humanlike || p.inventory.innerContainer.Any;
		}

		// Token: 0x0600777E RID: 30590 RVA: 0x002A03DB File Offset: 0x0029E5DB
		private bool ShouldShowApparel(Pawn p)
		{
			return p.apparel != null && (p.RaceProps.Humanlike || p.apparel.WornApparel.Any<Apparel>());
		}

		// Token: 0x0600777F RID: 30591 RVA: 0x002A0406 File Offset: 0x0029E606
		private bool ShouldShowEquipment(Pawn p)
		{
			return p.equipment != null;
		}

		// Token: 0x06007780 RID: 30592 RVA: 0x002A0414 File Offset: 0x0029E614
		private bool ShouldShowOverallArmor(Pawn p)
		{
			return p.RaceProps.Humanlike || this.ShouldShowApparel(p) || p.GetStatValue(StatDefOf.ArmorRating_Sharp, true) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Blunt, true) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Heat, true) > 0f;
		}

		// Token: 0x04004263 RID: 16995
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04004264 RID: 16996
		private float scrollViewHeight;

		// Token: 0x04004265 RID: 16997
		private const float TopPadding = 20f;

		// Token: 0x04004266 RID: 16998
		public static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x04004267 RID: 16999
		public static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x04004268 RID: 17000
		private const float ThingIconSize = 28f;

		// Token: 0x04004269 RID: 17001
		private const float ThingRowHeight = 28f;

		// Token: 0x0400426A RID: 17002
		private const float ThingLeftX = 36f;

		// Token: 0x0400426B RID: 17003
		private const float StandardLineHeight = 22f;

		// Token: 0x0400426C RID: 17004
		private const float InitialHeight = 450f;

		// Token: 0x0400426D RID: 17005
		private static List<Thing> workingInvList = new List<Thing>();

		// Token: 0x0400426E RID: 17006
		private static List<Thing> workingEquipmentList = new List<Thing>();
	}
}
