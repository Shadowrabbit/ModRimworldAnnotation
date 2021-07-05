using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200133E RID: 4926
	public class ITab_Bills : ITab
	{
		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x0600773C RID: 30524 RVA: 0x0029DA22 File Offset: 0x0029BC22
		protected Building_WorkTable SelTable
		{
			get
			{
				return (Building_WorkTable)base.SelThing;
			}
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x0029DA2F File Offset: 0x0029BC2F
		public ITab_Bills()
		{
			this.size = ITab_Bills.WinSize;
			this.labelKey = "TabBills";
			this.tutorTag = "Bills";
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x0029DA64 File Offset: 0x0029BC64
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.FrameDisplayed);
			Rect rect = new Rect(ITab_Bills.WinSize.x - ITab_Bills.PasteX, ITab_Bills.PasteY, ITab_Bills.PasteSize, ITab_Bills.PasteSize);
			if (BillUtility.Clipboard != null)
			{
				if (!this.SelTable.def.AllRecipes.Contains(BillUtility.Clipboard.recipe) || !BillUtility.Clipboard.recipe.AvailableNow || !BillUtility.Clipboard.recipe.AvailableOnNow(this.SelTable, null))
				{
					GUI.color = Color.gray;
					Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
					GUI.color = Color.white;
					if (Mouse.IsOver(rect))
					{
						TooltipHandler.TipRegion(rect, "ClipboardBillNotAvailableHere".Translate() + ": " + BillUtility.Clipboard.LabelCap);
					}
				}
				else if (this.SelTable.billStack.Count >= 15)
				{
					GUI.color = Color.gray;
					Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
					GUI.color = Color.white;
					if (Mouse.IsOver(rect))
					{
						TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + " (" + "PasteBillTip_LimitReached".Translate() + "): " + BillUtility.Clipboard.LabelCap);
					}
				}
				else
				{
					if (Widgets.ButtonImageFitted(rect, TexButton.Paste, Color.white))
					{
						Bill bill = BillUtility.Clipboard.Clone();
						bill.InitializeAfterClone();
						this.SelTable.billStack.AddBill(bill);
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					if (Mouse.IsOver(rect))
					{
						TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + ": " + BillUtility.Clipboard.LabelCap);
					}
				}
			}
			Rect rect2 = new Rect(0f, 0f, ITab_Bills.WinSize.x, ITab_Bills.WinSize.y).ContractedBy(10f);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				ITab_Bills.<>c__DisplayClass10_0 CS$<>8__locals1;
				CS$<>8__locals1.opts = new List<FloatMenuOption>();
				for (int i = 0; i < this.SelTable.def.AllRecipes.Count; i++)
				{
					ITab_Bills.<>c__DisplayClass10_1 CS$<>8__locals2 = new ITab_Bills.<>c__DisplayClass10_1();
					CS$<>8__locals2.<>4__this = this;
					if (this.SelTable.def.AllRecipes[i].AvailableNow && this.SelTable.def.AllRecipes[i].AvailableOnNow(this.SelTable, null))
					{
						CS$<>8__locals2.recipe = this.SelTable.def.AllRecipes[i];
						CS$<>8__locals2.<FillTab>g__Add|1(null, ref CS$<>8__locals1);
						foreach (Precept_Building precept_Building in Faction.OfPlayer.ideos.PrimaryIdeo.cachedPossibleBuildings)
						{
							if (precept_Building.ThingDef == CS$<>8__locals2.recipe.ProducedThingDef)
							{
								CS$<>8__locals2.<FillTab>g__Add|1(precept_Building, ref CS$<>8__locals1);
							}
						}
					}
				}
				if (!CS$<>8__locals1.opts.Any<FloatMenuOption>())
				{
					CS$<>8__locals1.opts.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				return CS$<>8__locals1.opts;
			};
			this.mouseoverBill = this.SelTable.billStack.DoListing(rect2, recipeOptionsMaker, ref this.scrollPosition, ref this.viewHeight);
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x0029DCB8 File Offset: 0x0029BEB8
		public override void TabUpdate()
		{
			if (this.mouseoverBill != null)
			{
				this.mouseoverBill.TryDrawIngredientSearchRadiusOnMap(this.SelTable.Position);
				this.mouseoverBill = null;
			}
		}

		// Token: 0x04004240 RID: 16960
		private float viewHeight = 1000f;

		// Token: 0x04004241 RID: 16961
		private Vector2 scrollPosition;

		// Token: 0x04004242 RID: 16962
		private Bill mouseoverBill;

		// Token: 0x04004243 RID: 16963
		private static readonly Vector2 WinSize = new Vector2(420f, 480f);

		// Token: 0x04004244 RID: 16964
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteX = 48f;

		// Token: 0x04004245 RID: 16965
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteY = 3f;

		// Token: 0x04004246 RID: 16966
		[TweakValue("Interface", 0f, 32f)]
		private static float PasteSize = 24f;
	}
}
