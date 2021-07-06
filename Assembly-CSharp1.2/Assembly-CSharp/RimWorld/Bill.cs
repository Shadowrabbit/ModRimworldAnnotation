using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000B19 RID: 2841
	public abstract class Bill : IExposable, ILoadReferenceable
	{
		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06004262 RID: 16994 RVA: 0x000316DD File Offset: 0x0002F8DD
		public Map Map
		{
			get
			{
				return this.billStack.billGiver.Map;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06004263 RID: 16995 RVA: 0x000316EF File Offset: 0x0002F8EF
		public virtual string Label
		{
			get
			{
				return this.recipe.label;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x000316FC File Offset: 0x0002F8FC
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.recipe);
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06004265 RID: 16997 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CompletableEver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual string StatusString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x00016647 File Offset: 0x00014847
		protected virtual float StatusLineMinHeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool CanCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x00189D54 File Offset: 0x00187F54
		public bool DeletedOrDereferenced
		{
			get
			{
				if (this.deleted)
				{
					return true;
				}
				Thing thing = this.billStack.billGiver as Thing;
				return thing != null && thing.Destroyed;
			}
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x0003170F File Offset: 0x0002F90F
		public Bill()
		{
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x00189D8C File Offset: 0x00187F8C
		public Bill(RecipeDef recipe)
		{
			this.recipe = recipe;
			this.ingredientFilter = new ThingFilter();
			this.ingredientFilter.CopyAllowancesFrom(recipe.defaultIngredientFilter);
			this.InitializeAfterClone();
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x00031742 File Offset: 0x0002F942
		public void InitializeAfterClone()
		{
			this.loadID = Find.UniqueIDsManager.GetNextBillID();
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x00189DF4 File Offset: 0x00187FF4
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipe, "recipe");
			Scribe_Values.Look<bool>(ref this.suspended, "suspended", false, false);
			Scribe_Values.Look<float>(ref this.ingredientSearchRadius, "ingredientSearchRadius", 999f, false);
			Scribe_Values.Look<IntRange>(ref this.allowedSkillRange, "allowedSkillRange", default(IntRange), false);
			Scribe_References.Look<Pawn>(ref this.pawnRestriction, "pawnRestriction", false);
			if (Scribe.mode == LoadSaveMode.Saving && this.recipe.fixedIngredientFilter != null)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (!this.recipe.fixedIngredientFilter.Allows(thingDef))
					{
						this.ingredientFilter.SetAllow(thingDef, false);
					}
				}
			}
			Scribe_Deep.Look<ThingFilter>(ref this.ingredientFilter, "ingredientFilter", Array.Empty<object>());
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x00189EF8 File Offset: 0x001880F8
		public virtual bool PawnAllowedToStartAnew(Pawn p)
		{
			if (this.pawnRestriction != null)
			{
				return this.pawnRestriction == p;
			}
			if (this.recipe.workSkill != null)
			{
				int level = p.skills.GetSkill(this.recipe.workSkill).Level;
				if (level < this.allowedSkillRange.min)
				{
					JobFailReason.Is("UnderAllowedSkill".Translate(this.allowedSkillRange.min), this.Label);
					return false;
				}
				if (level > this.allowedSkillRange.max)
				{
					JobFailReason.Is("AboveAllowedSkill".Translate(this.allowedSkillRange.max), this.Label);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnDidWork(Pawn p)
		{
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06004272 RID: 17010
		public abstract bool ShouldDoNow();

		// Token: 0x06004273 RID: 17011 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_DoBillStarted(Pawn billDoer)
		{
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x00189FB8 File Offset: 0x001881B8
		protected virtual void DoConfigInterface(Rect rect, Color baseColor)
		{
			rect.yMin += 29f;
			float y = rect.center.y;
			Widgets.InfoCardButton(rect.xMax - (rect.yMax - y) - 12f, y - 12f, this.recipe);
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DoStatusLineInterface(Rect rect)
		{
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0018A010 File Offset: 0x00188210
		public Rect DoInterface(float x, float y, float width, int index)
		{
			Rect rect = new Rect(x, y, width, 53f);
			float num = 0f;
			if (!this.StatusString.NullOrEmpty())
			{
				num = Mathf.Max(17f, this.StatusLineMinHeight);
			}
			rect.height += num;
			Color white = Color.white;
			if (!this.ShouldDoNow())
			{
				white = new Color(1f, 0.7f, 0.7f, 0.7f);
			}
			GUI.color = white;
			Text.Font = GameFont.Small;
			if (index % 2 == 0)
			{
				Widgets.DrawAltRect(rect);
			}
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, 24f, 24f);
			if (this.billStack.IndexOf(this) > 0)
			{
				if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, white, true))
				{
					this.billStack.Reorder(this, -1);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect2, "ReorderBillUpTip");
			}
			if (this.billStack.IndexOf(this) < this.billStack.Count - 1)
			{
				Rect rect3 = new Rect(0f, 24f, 24f, 24f);
				if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, white, true))
				{
					this.billStack.Reorder(this, 1);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect3, "ReorderBillDownTip");
			}
			Widgets.Label(new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f), this.LabelCap);
			this.DoConfigInterface(rect.AtZero(), white);
			Rect rect4 = new Rect(rect.width - 24f, 0f, 24f, 24f);
			if (Widgets.ButtonImage(rect4, TexButton.DeleteX, white, white * GenUI.SubtleMouseoverColor, true))
			{
				this.billStack.Delete(this);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect4, "DeleteBillTip");
			Rect rect6;
			if (this.CanCopy)
			{
				Rect rect5 = new Rect(rect4);
				rect5.x -= rect5.width + 4f;
				if (Widgets.ButtonImageFitted(rect5, TexButton.Copy, white))
				{
					BillUtility.Clipboard = this.Clone();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				TooltipHandler.TipRegionByKey(rect5, "CopyBillTip");
				rect6 = new Rect(rect5);
			}
			else
			{
				rect6 = new Rect(rect4);
			}
			rect6.x -= rect6.width + 4f;
			if (Widgets.ButtonImage(rect6, TexButton.Suspend, white, true))
			{
				this.suspended = !this.suspended;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			TooltipHandler.TipRegionByKey(rect6, "SuspendBillTip");
			if (!this.StatusString.NullOrEmpty())
			{
				Text.Font = GameFont.Tiny;
				Rect rect7 = new Rect(24f, rect.height - num, rect.width - 24f, num);
				Widgets.Label(rect7, this.StatusString);
				this.DoStatusLineInterface(rect7);
			}
			GUI.EndGroup();
			if (this.suspended)
			{
				Text.Font = GameFont.Medium;
				Text.Anchor = TextAnchor.MiddleCenter;
				Rect rect8 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
				GUI.DrawTexture(rect8, TexUI.GrayTextBG);
				Widgets.Label(rect8, "SuspendedCaps".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			return rect;
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x0018A3B4 File Offset: 0x001885B4
		public bool IsFixedOrAllowedIngredient(Thing thing)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(thing))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(thing) && this.ingredientFilter.Allows(thing);
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x0018A428 File Offset: 0x00188628
		public bool IsFixedOrAllowedIngredient(ThingDef def)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				IngredientCount ingredientCount = this.recipe.ingredients[i];
				if (ingredientCount.IsFixedIngredient && ingredientCount.filter.Allows(def))
				{
					return true;
				}
			}
			return this.recipe.fixedIngredientFilter.Allows(def) && this.ingredientFilter.Allows(def);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x0018A49C File Offset: 0x0018869C
		public static void CreateNoPawnsWithSkillDialog(RecipeDef recipe)
		{
			string text = "RecipeRequiresSkills".Translate(recipe.LabelCap);
			text += "\n\n";
			text += recipe.MinSkillString;
			Find.WindowStack.Add(new Dialog_MessageBox(text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x00031754 File Offset: 0x0002F954
		public virtual BillStoreModeDef GetStoreMode()
		{
			return BillStoreModeDefOf.BestStockpile;
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Zone_Stockpile GetStoreZone()
		{
			return null;
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0003175B File Offset: 0x0002F95B
		public virtual void SetStoreMode(BillStoreModeDef mode, Zone_Stockpile zone = null)
		{
			Log.ErrorOnce("Tried to set store mode of a non-production bill", 27190980, false);
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0018A4FC File Offset: 0x001886FC
		public virtual Bill Clone()
		{
			Bill bill = (Bill)Activator.CreateInstance(base.GetType());
			bill.recipe = this.recipe;
			bill.suspended = this.suspended;
			bill.ingredientFilter = new ThingFilter();
			bill.ingredientFilter.CopyAllowancesFrom(this.ingredientFilter);
			bill.ingredientSearchRadius = this.ingredientSearchRadius;
			bill.allowedSkillRange = this.allowedSkillRange;
			bill.pawnRestriction = this.pawnRestriction;
			return bill;
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x0018A574 File Offset: 0x00188774
		public virtual void ValidateSettings()
		{
			if (this.pawnRestriction != null && (this.pawnRestriction.Dead || this.pawnRestriction.Faction != Faction.OfPlayer || this.pawnRestriction.IsKidnapped()))
			{
				if (this != BillUtility.Clipboard)
				{
					Messages.Message("MessageBillValidationPawnUnavailable".Translate(this.pawnRestriction.LabelShortCap, this.Label, this.billStack.billGiver.LabelShort), this.billStack.billGiver as Thing, MessageTypeDefOf.NegativeEvent, true);
				}
				this.pawnRestriction = null;
				return;
			}
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x0003176D File Offset: 0x0002F96D
		public string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Bill_",
				this.recipe.defName,
				"_",
				this.loadID
			});
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x000317A6 File Offset: 0x0002F9A6
		public override string ToString()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x04002D8D RID: 11661
		[Unsaved(false)]
		public BillStack billStack;

		// Token: 0x04002D8E RID: 11662
		private int loadID = -1;

		// Token: 0x04002D8F RID: 11663
		public RecipeDef recipe;

		// Token: 0x04002D90 RID: 11664
		public bool suspended;

		// Token: 0x04002D91 RID: 11665
		public ThingFilter ingredientFilter;

		// Token: 0x04002D92 RID: 11666
		public float ingredientSearchRadius = 999f;

		// Token: 0x04002D93 RID: 11667
		public IntRange allowedSkillRange = new IntRange(0, 20);

		// Token: 0x04002D94 RID: 11668
		public Pawn pawnRestriction;

		// Token: 0x04002D95 RID: 11669
		public bool deleted;

		// Token: 0x04002D96 RID: 11670
		public int lastIngredientSearchFailTicks = -99999;

		// Token: 0x04002D97 RID: 11671
		public const int MaxIngredientSearchRadius = 999;

		// Token: 0x04002D98 RID: 11672
		public const float ButSize = 24f;

		// Token: 0x04002D99 RID: 11673
		private const float InterfaceBaseHeight = 53f;

		// Token: 0x04002D9A RID: 11674
		private const float InterfaceStatusLineHeight = 17f;
	}
}
