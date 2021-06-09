using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AFB RID: 6907
	public class ITab_Bills : ITab
	{
		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x06009813 RID: 38931 RVA: 0x000654AA File Offset: 0x000636AA
		protected Building_WorkTable SelTable
		{
			get
			{
				return (Building_WorkTable)base.SelThing;
			}
		}

		// Token: 0x06009814 RID: 38932 RVA: 0x000654B7 File Offset: 0x000636B7
		public ITab_Bills()
		{
			this.size = ITab_Bills.WinSize;
			this.labelKey = "TabBills";
			this.tutorTag = "Bills";
		}

		// Token: 0x06009815 RID: 38933 RVA: 0x002CA1DC File Offset: 0x002C83DC
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BillsTab, KnowledgeAmount.FrameDisplayed);
			Rect rect = new Rect(ITab_Bills.WinSize.x - ITab_Bills.PasteX, ITab_Bills.PasteY, ITab_Bills.PasteSize, ITab_Bills.PasteSize);
			if (BillUtility.Clipboard == null)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegionByKey(rect, "PasteBillTip");
			}
			else if (!this.SelTable.def.AllRecipes.Contains(BillUtility.Clipboard.recipe) || !BillUtility.Clipboard.recipe.AvailableNow || !BillUtility.Clipboard.recipe.AvailableOnNow(this.SelTable))
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				TooltipHandler.TipRegionByKey(rect, "ClipboardBillNotAvailableHere");
			}
			else if (this.SelTable.billStack.Count >= 15)
			{
				GUI.color = Color.gray;
				Widgets.DrawTextureFitted(rect, TexButton.Paste, 1f);
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, "PasteBillTip".Translate() + " (" + "PasteBillTip_LimitReached".Translate() + ")");
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
				TooltipHandler.TipRegionByKey(rect, "PasteBillTip");
			}
			Rect rect2 = new Rect(0f, 0f, ITab_Bills.WinSize.x, ITab_Bills.WinSize.y).ContractedBy(10f);
			Func<List<FloatMenuOption>> recipeOptionsMaker = delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				for (int i = 0; i < this.SelTable.def.AllRecipes.Count; i++)
				{
					if (this.SelTable.def.AllRecipes[i].AvailableNow && this.SelTable.def.AllRecipes[i].AvailableOnNow(this.SelTable))
					{
						RecipeDef recipe = this.SelTable.def.AllRecipes[i];
						Predicate<Pawn> <>9__3;
						list.Add(new FloatMenuOption(recipe.LabelCap, delegate()
						{
							List<Pawn> freeColonists = this.SelTable.Map.mapPawns.FreeColonists;
							Predicate<Pawn> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)));
							}
							if (!freeColonists.Any(predicate))
							{
								Bill.CreateNoPawnsWithSkillDialog(recipe);
							}
							Bill bill2 = recipe.MakeNewBill();
							this.SelTable.billStack.AddBill(bill2);
							if (recipe.conceptLearned != null)
							{
								PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
							}
							if (TutorSystem.TutorialMode)
							{
								TutorSystem.Notify_Event("AddBill-" + recipe.LabelCap.Resolve());
							}
						}, recipe.UIIconThing, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, recipe), null));
					}
				}
				if (!list.Any<FloatMenuOption>())
				{
					list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				return list;
			};
			this.mouseoverBill = this.SelTable.billStack.DoListing(rect2, recipeOptionsMaker, ref this.scrollPosition, ref this.viewHeight);
		}

		// Token: 0x06009816 RID: 38934 RVA: 0x000654EB File Offset: 0x000636EB
		public override void TabUpdate()
		{
			if (this.mouseoverBill != null)
			{
				this.mouseoverBill.TryDrawIngredientSearchRadiusOnMap(this.SelTable.Position);
				this.mouseoverBill = null;
			}
		}

		// Token: 0x04006124 RID: 24868
		private float viewHeight = 1000f;

		// Token: 0x04006125 RID: 24869
		private Vector2 scrollPosition;

		// Token: 0x04006126 RID: 24870
		private Bill mouseoverBill;

		// Token: 0x04006127 RID: 24871
		private static readonly Vector2 WinSize = new Vector2(420f, 480f);

		// Token: 0x04006128 RID: 24872
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteX = 48f;

		// Token: 0x04006129 RID: 24873
		[TweakValue("Interface", 0f, 128f)]
		private static float PasteY = 3f;

		// Token: 0x0400612A RID: 24874
		[TweakValue("Interface", 0f, 32f)]
		private static float PasteSize = 24f;
	}
}
