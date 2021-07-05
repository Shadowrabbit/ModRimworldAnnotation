using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001088 RID: 4232
	public class Bill_Production : Bill, IExposable
	{
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060064B7 RID: 25783 RVA: 0x0021EBD4 File Offset: 0x0021CDD4
		protected override string StatusString
		{
			get
			{
				if (this.paused)
				{
					return " " + "Paused".Translate();
				}
				return "";
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060064B8 RID: 25784 RVA: 0x0021EBFD File Offset: 0x0021CDFD
		protected override float StatusLineMinHeight
		{
			get
			{
				if (!this.CanUnpause())
				{
					return 0f;
				}
				return 24f;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060064B9 RID: 25785 RVA: 0x0021EC14 File Offset: 0x0021CE14
		public string RepeatInfoText
		{
			get
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					return "Forever".Translate();
				}
				if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					return this.repeatCount.ToString() + "x";
				}
				if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					return this.recipe.WorkerCounter.CountProducts(this).ToString() + "/" + this.targetCount.ToString();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060064BA RID: 25786 RVA: 0x0021ECA4 File Offset: 0x0021CEA4
		public Bill_Production()
		{
		}

		// Token: 0x060064BB RID: 25787 RVA: 0x0021ECFC File Offset: 0x0021CEFC
		public Bill_Production(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x060064BC RID: 25788 RVA: 0x0021ED54 File Offset: 0x0021CF54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<BillRepeatModeDef>(ref this.repeatMode, "repeatMode");
			Scribe_Values.Look<int>(ref this.repeatCount, "repeatCount", 0, false);
			Scribe_Defs.Look<BillStoreModeDef>(ref this.storeMode, "storeMode");
			Scribe_References.Look<Zone_Stockpile>(ref this.storeZone, "storeZone", false);
			Scribe_Values.Look<int>(ref this.targetCount, "targetCount", 0, false);
			Scribe_Values.Look<bool>(ref this.pauseWhenSatisfied, "pauseWhenSatisfied", false, false);
			Scribe_Values.Look<int>(ref this.unpauseWhenYouHave, "unpauseWhenYouHave", 0, false);
			Scribe_Values.Look<bool>(ref this.includeEquipped, "includeEquipped", false, false);
			Scribe_Values.Look<bool>(ref this.includeTainted, "includeTainted", false, false);
			Scribe_References.Look<Zone_Stockpile>(ref this.includeFromZone, "includeFromZone", false);
			Scribe_Values.Look<FloatRange>(ref this.hpRange, "hpRange", FloatRange.ZeroToOne, false);
			Scribe_Values.Look<QualityRange>(ref this.qualityRange, "qualityRange", QualityRange.All, false);
			Scribe_Values.Look<bool>(ref this.limitToAllowedStuff, "limitToAllowedStuff", false, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			if (this.repeatMode == null)
			{
				this.repeatMode = BillRepeatModeDefOf.RepeatCount;
			}
			if (this.storeMode == null)
			{
				this.storeMode = BillStoreModeDefOf.BestStockpile;
			}
		}

		// Token: 0x060064BD RID: 25789 RVA: 0x0021EE8B File Offset: 0x0021D08B
		public override BillStoreModeDef GetStoreMode()
		{
			return this.storeMode;
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x0021EE93 File Offset: 0x0021D093
		public override Zone_Stockpile GetStoreZone()
		{
			return this.storeZone;
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x0021EE9B File Offset: 0x0021D09B
		public override void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			this.storeMode = mode;
			this.storeZone = zone;
			if (this.storeMode == BillStoreModeDefOf.SpecificStockpile != (this.storeZone != null))
			{
				Log.ErrorOnce("Inconsistent bill StoreMode data set", 75645354);
			}
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x0021EED4 File Offset: 0x0021D0D4
		public override bool ShouldDoNow()
		{
			if (this.repeatMode != BillRepeatModeDefOf.TargetCount)
			{
				this.paused = false;
			}
			if (this.suspended)
			{
				return false;
			}
			if (this.repeatMode == BillRepeatModeDefOf.Forever)
			{
				return true;
			}
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				return this.repeatCount > 0;
			}
			if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
			{
				int num = this.recipe.WorkerCounter.CountProducts(this);
				if (this.pauseWhenSatisfied && num >= this.targetCount)
				{
					this.paused = true;
				}
				if (num <= this.unpauseWhenYouHave || !this.pauseWhenSatisfied)
				{
					this.paused = false;
				}
				return !this.paused && num < this.targetCount;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x0021EF90 File Offset: 0x0021D190
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
			{
				if (this.repeatCount > 0)
				{
					this.repeatCount--;
				}
				if (this.repeatCount == 0)
				{
					Messages.Message("MessageBillComplete".Translate(this.LabelCap), (Thing)this.billStack.billGiver, MessageTypeDefOf.TaskCompletion, true);
				}
			}
			this.recipe.Worker.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x060064C2 RID: 25794 RVA: 0x0021F018 File Offset: 0x0021D218
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_BillConfig(this, ((Thing)this.billStack.billGiver).Position));
			}
			if (widgetRow.ButtonText(this.repeatMode.LabelCap.Resolve().PadRight(20), null, true, true, true, null))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(TexButton.Plus, null, null, null, null, true))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount += num;
					this.unpauseWhenYouHave += num;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount += GenUI.CurrentAdjustmentMultiplier();
				}
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
				}
			}
			if (widgetRow.ButtonIcon(TexButton.Minus, null, null, null, null, true))
			{
				if (this.repeatMode == BillRepeatModeDefOf.Forever)
				{
					this.repeatMode = BillRepeatModeDefOf.RepeatCount;
					this.repeatCount = 1;
				}
				else if (this.repeatMode == BillRepeatModeDefOf.TargetCount)
				{
					int num2 = this.recipe.targetCountAdjustment * GenUI.CurrentAdjustmentMultiplier();
					this.targetCount = Mathf.Max(0, this.targetCount - num2);
					this.unpauseWhenYouHave = Mathf.Max(0, this.unpauseWhenYouHave - num2);
				}
				else if (this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					this.repeatCount = Mathf.Max(0, this.repeatCount - GenUI.CurrentAdjustmentMultiplier());
				}
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				if (TutorSystem.TutorialMode && this.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					TutorSystem.Notify_Event(this.recipe.defName + "-RepeatCountSetTo-" + this.repeatCount);
				}
			}
		}

		// Token: 0x060064C3 RID: 25795 RVA: 0x0021F313 File Offset: 0x0021D513
		private bool CanUnpause()
		{
			return this.repeatMode == BillRepeatModeDefOf.TargetCount && this.paused && this.pauseWhenSatisfied && this.recipe.WorkerCounter.CountProducts(this) < this.targetCount;
		}

		// Token: 0x060064C4 RID: 25796 RVA: 0x0021F350 File Offset: 0x0021D550
		public override void DoStatusLineInterface(Rect rect)
		{
			if (this.paused && new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenUp, 99999f, 4f).ButtonText("Unpause".Translate(), null, true, true, true, null))
			{
				this.paused = false;
			}
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x0021F3B0 File Offset: 0x0021D5B0
		public override void ValidateSettings()
		{
			base.ValidateSettings();
			if (this.storeZone != null)
			{
				if (!this.storeZone.zoneManager.AllZones.Contains(this.storeZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationStoreZoneDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), this.storeZone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
				}
				else if (base.Map != null && !base.Map.zoneManager.AllZones.Contains(this.storeZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationStoreZoneUnavailable".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), this.storeZone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
				}
			}
			else if (this.storeMode == BillStoreModeDefOf.SpecificStockpile)
			{
				this.SetStoreMode(BillStoreModeDefOf.DropOnFloor, null);
				Log.ErrorOnce("Found SpecificStockpile bill store mode without associated stockpile, recovering", 46304128);
			}
			if (this.includeFromZone != null)
			{
				if (!this.includeFromZone.zoneManager.AllZones.Contains(this.includeFromZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationIncludeZoneDeleted".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), this.includeFromZone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.includeFromZone = null;
					return;
				}
				if (base.Map != null && !base.Map.zoneManager.AllZones.Contains(this.includeFromZone))
				{
					if (this != BillUtility.Clipboard)
					{
						Messages.Message("MessageBillValidationIncludeZoneUnavailable".Translate(this.LabelCap, this.billStack.billGiver.LabelShort.CapitalizeFirst(), this.includeFromZone.label), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
					}
					this.includeFromZone = null;
				}
			}
		}

		// Token: 0x060064C6 RID: 25798 RVA: 0x0021F66C File Offset: 0x0021D86C
		public override Bill Clone()
		{
			Bill_Production bill_Production = (Bill_Production)base.Clone();
			bill_Production.repeatMode = this.repeatMode;
			bill_Production.repeatCount = this.repeatCount;
			bill_Production.storeMode = this.storeMode;
			bill_Production.storeZone = this.storeZone;
			bill_Production.targetCount = this.targetCount;
			bill_Production.pauseWhenSatisfied = this.pauseWhenSatisfied;
			bill_Production.unpauseWhenYouHave = this.unpauseWhenYouHave;
			bill_Production.includeEquipped = this.includeEquipped;
			bill_Production.includeTainted = this.includeTainted;
			bill_Production.includeFromZone = this.includeFromZone;
			bill_Production.hpRange = this.hpRange;
			bill_Production.qualityRange = this.qualityRange;
			bill_Production.limitToAllowedStuff = this.limitToAllowedStuff;
			bill_Production.paused = this.paused;
			return bill_Production;
		}

		// Token: 0x040038AB RID: 14507
		public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;

		// Token: 0x040038AC RID: 14508
		public int repeatCount = 1;

		// Token: 0x040038AD RID: 14509
		private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

		// Token: 0x040038AE RID: 14510
		private Zone_Stockpile storeZone;

		// Token: 0x040038AF RID: 14511
		public int targetCount = 10;

		// Token: 0x040038B0 RID: 14512
		public bool pauseWhenSatisfied;

		// Token: 0x040038B1 RID: 14513
		public int unpauseWhenYouHave = 5;

		// Token: 0x040038B2 RID: 14514
		public bool includeEquipped;

		// Token: 0x040038B3 RID: 14515
		public bool includeTainted;

		// Token: 0x040038B4 RID: 14516
		public Zone_Stockpile includeFromZone;

		// Token: 0x040038B5 RID: 14517
		public FloatRange hpRange = FloatRange.ZeroToOne;

		// Token: 0x040038B6 RID: 14518
		public QualityRange qualityRange = QualityRange.All;

		// Token: 0x040038B7 RID: 14519
		public bool limitToAllowedStuff;

		// Token: 0x040038B8 RID: 14520
		public bool paused;
	}
}
