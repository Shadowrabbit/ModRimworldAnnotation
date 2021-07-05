using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016D5 RID: 5845
	public class Bill_Production : Bill, IExposable
	{
		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06008057 RID: 32855 RVA: 0x00056220 File Offset: 0x00054420
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

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06008058 RID: 32856 RVA: 0x00056249 File Offset: 0x00054449
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

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06008059 RID: 32857 RVA: 0x0025F9BC File Offset: 0x0025DBBC
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

		// Token: 0x0600805A RID: 32858 RVA: 0x0025FA4C File Offset: 0x0025DC4C
		public Bill_Production()
		{
		}

		// Token: 0x0600805B RID: 32859 RVA: 0x0025FAA4 File Offset: 0x0025DCA4
		public Bill_Production(RecipeDef recipe) : base(recipe)
		{
		}

		// Token: 0x0600805C RID: 32860 RVA: 0x0025FAFC File Offset: 0x0025DCFC
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

		// Token: 0x0600805D RID: 32861 RVA: 0x0005625E File Offset: 0x0005445E
		public override BillStoreModeDef GetStoreMode()
		{
			return this.storeMode;
		}

		// Token: 0x0600805E RID: 32862 RVA: 0x00056266 File Offset: 0x00054466
		public override Zone_Stockpile GetStoreZone()
		{
			return this.storeZone;
		}

		// Token: 0x0600805F RID: 32863 RVA: 0x0005626E File Offset: 0x0005446E
		public override void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			this.storeMode = mode;
			this.storeZone = zone;
			if (this.storeMode == BillStoreModeDefOf.SpecificStockpile != (this.storeZone != null))
			{
				Log.ErrorOnce("Inconsistent bill StoreMode data set", 75645354, false);
			}
		}

		// Token: 0x06008060 RID: 32864 RVA: 0x0025FC34 File Offset: 0x0025DE34
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

		// Token: 0x06008061 RID: 32865 RVA: 0x0025FCF0 File Offset: 0x0025DEF0
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

		// Token: 0x06008062 RID: 32866 RVA: 0x0025FD78 File Offset: 0x0025DF78
		protected override void DoConfigInterface(Rect baseRect, Color baseColor)
		{
			Rect rect = new Rect(28f, 32f, 100f, 30f);
			GUI.color = new Color(1f, 1f, 1f, 0.65f);
			Widgets.Label(rect, this.RepeatInfoText);
			GUI.color = baseColor;
			WidgetRow widgetRow = new WidgetRow(baseRect.xMax, baseRect.y + 29f, UIDirection.LeftThenUp, 99999f, 4f);
			if (widgetRow.ButtonText("Details".Translate() + "...", null, true, true))
			{
				Find.WindowStack.Add(new Dialog_BillConfig(this, ((Thing)this.billStack.billGiver).Position));
			}
			if (widgetRow.ButtonText(this.repeatMode.LabelCap.Resolve().PadRight(20), null, true, true))
			{
				BillRepeatModeUtility.MakeConfigFloatMenu(this);
			}
			if (widgetRow.ButtonIcon(TexButton.Plus, null, null, true))
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
			if (widgetRow.ButtonIcon(TexButton.Minus, null, null, true))
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

		// Token: 0x06008063 RID: 32867 RVA: 0x000562A6 File Offset: 0x000544A6
		private bool CanUnpause()
		{
			return this.repeatMode == BillRepeatModeDefOf.TargetCount && this.paused && this.pauseWhenSatisfied && this.recipe.WorkerCounter.CountProducts(this) < this.targetCount;
		}

		// Token: 0x06008064 RID: 32868 RVA: 0x00260038 File Offset: 0x0025E238
		public override void DoStatusLineInterface(Rect rect)
		{
			if (this.paused && new WidgetRow(rect.xMax, rect.y, UIDirection.LeftThenUp, 99999f, 4f).ButtonText("Unpause".Translate(), null, true, true))
			{
				this.paused = false;
			}
		}

		// Token: 0x06008065 RID: 32869 RVA: 0x0026008C File Offset: 0x0025E28C
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
				Log.ErrorOnce("Found SpecificStockpile bill store mode without associated stockpile, recovering", 46304128, false);
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

		// Token: 0x06008066 RID: 32870 RVA: 0x0026034C File Offset: 0x0025E54C
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

		// Token: 0x0400531C RID: 21276
		public BillRepeatModeDef repeatMode = BillRepeatModeDefOf.RepeatCount;

		// Token: 0x0400531D RID: 21277
		public int repeatCount = 1;

		// Token: 0x0400531E RID: 21278
		private BillStoreModeDef storeMode = BillStoreModeDefOf.BestStockpile;

		// Token: 0x0400531F RID: 21279
		private Zone_Stockpile storeZone;

		// Token: 0x04005320 RID: 21280
		public int targetCount = 10;

		// Token: 0x04005321 RID: 21281
		public bool pauseWhenSatisfied;

		// Token: 0x04005322 RID: 21282
		public int unpauseWhenYouHave = 5;

		// Token: 0x04005323 RID: 21283
		public bool includeEquipped;

		// Token: 0x04005324 RID: 21284
		public bool includeTainted;

		// Token: 0x04005325 RID: 21285
		public Zone_Stockpile includeFromZone;

		// Token: 0x04005326 RID: 21286
		public FloatRange hpRange = FloatRange.ZeroToOne;

		// Token: 0x04005327 RID: 21287
		public QualityRange qualityRange = QualityRange.All;

		// Token: 0x04005328 RID: 21288
		public bool limitToAllowedStuff;

		// Token: 0x04005329 RID: 21289
		public bool paused;
	}
}
