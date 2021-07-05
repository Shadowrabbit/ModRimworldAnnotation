using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001148 RID: 4424
	public class CompIngredients : ThingComp
	{
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06006A3A RID: 27194 RVA: 0x0023BEB3 File Offset: 0x0023A0B3
		public CompProperties_Ingredients Props
		{
			get
			{
				return (CompProperties_Ingredients)this.props;
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06006A3B RID: 27195 RVA: 0x0023BEC0 File Offset: 0x0023A0C0
		public List<string> MergeCompatibilityTags
		{
			get
			{
				if (this.cachedMergeCompatibilityTags == null)
				{
					this.cachedMergeCompatibilityTags = new List<string>();
					if (this.Props.performMergeCompatibilityChecks)
					{
						CompIngredients.ComputeTags(this.cachedMergeCompatibilityTags, this.ingredients);
					}
				}
				return this.cachedMergeCompatibilityTags;
			}
		}

		// Token: 0x06006A3C RID: 27196 RVA: 0x0023BEF9 File Offset: 0x0023A0F9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.ingredients, "ingredients", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ingredients == null)
			{
				this.ingredients = new List<ThingDef>();
			}
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x0023BF32 File Offset: 0x0023A132
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
				this.cachedMergeCompatibilityTags = null;
			}
		}

		// Token: 0x06006A3E RID: 27198 RVA: 0x0023BF58 File Offset: 0x0023A158
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (piece != this.parent)
			{
				CompIngredients compIngredients = piece.TryGetComp<CompIngredients>();
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					compIngredients.ingredients.Add(this.ingredients[i]);
				}
			}
		}

		// Token: 0x06006A3F RID: 27199 RVA: 0x0023BFAC File Offset: 0x0023A1AC
		public override bool AllowStackWith(Thing otherStack)
		{
			if (!base.AllowStackWith(otherStack))
			{
				return false;
			}
			CompIngredients compIngredients = otherStack.TryGetComp<CompIngredients>();
			if (!this.Props.performMergeCompatibilityChecks || !compIngredients.Props.performMergeCompatibilityChecks)
			{
				return true;
			}
			int count = this.MergeCompatibilityTags.Count;
			int count2 = compIngredients.MergeCompatibilityTags.Count;
			bool result;
			if (count == 0 && count2 == 0)
			{
				result = true;
			}
			else if (count != count2 && (count == 0 || count2 == 0))
			{
				result = false;
			}
			else
			{
				CompIngredients.tmpMergedIngredients.Clear();
				CompIngredients.tmpMergedIngredientTags.Clear();
				CompIngredients.tmpMergedIngredients.AddRange(this.ingredients);
				bool flag;
				CompIngredients.MergeIngredients(CompIngredients.tmpMergedIngredients, compIngredients.ingredients, out flag);
				if (flag)
				{
					result = false;
				}
				else
				{
					CompIngredients.ComputeTags(CompIngredients.tmpMergedIngredientTags, CompIngredients.tmpMergedIngredients);
					result = (CompIngredients.tmpMergedIngredientTags.SetsEqual(this.MergeCompatibilityTags) && CompIngredients.tmpMergedIngredientTags.SetsEqual(compIngredients.MergeCompatibilityTags));
				}
			}
			return result;
		}

		// Token: 0x06006A40 RID: 27200 RVA: 0x0023C090 File Offset: 0x0023A290
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompIngredients compIngredients = otherStack.TryGetComp<CompIngredients>();
			bool flag;
			CompIngredients.MergeIngredients(this.ingredients, compIngredients.ingredients, out flag);
			this.cachedMergeCompatibilityTags = null;
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x0023C0C8 File Offset: 0x0023A2C8
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ingredients.Count > 0)
			{
				bool flag = false;
				stringBuilder.Append("Ingredients".Translate() + ": ");
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					ThingDef thingDef = this.ingredients[i];
					stringBuilder.Append((i == 0) ? thingDef.LabelCap.Resolve() : thingDef.label);
					if (this.Props.performMergeCompatibilityChecks)
					{
						IngredientProperties ingredient = thingDef.ingredient;
						if (ingredient != null && ingredient.mergeCompatibilityTags.Count > 0)
						{
							stringBuilder.Append("*");
							flag = true;
						}
					}
					if (i < this.ingredients.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				if (flag)
				{
					stringBuilder.Append(" (* " + "OnlyStacksWithCompatibleMeals".Translate().Resolve() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x0023C1E0 File Offset: 0x0023A3E0
		private static void ComputeTags(List<string> result, List<ThingDef> ingredients)
		{
			foreach (ThingDef thingDef in ingredients)
			{
				if (thingDef.ingredient != null)
				{
					foreach (string element in thingDef.ingredient.mergeCompatibilityTags)
					{
						result.AddDistinct(element);
					}
				}
			}
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x0023C278 File Offset: 0x0023A478
		private static void MergeIngredients(List<ThingDef> destIngredients, List<ThingDef> srcIngredients, out bool lostImportantIngredients)
		{
			lostImportantIngredients = false;
			foreach (ThingDef element in srcIngredients)
			{
				destIngredients.AddDistinct(element);
			}
			if (destIngredients.Count > 3)
			{
				destIngredients.Shuffle<ThingDef>();
				destIngredients.SortStable(CompIngredients.MostTagsCmp);
				while (destIngredients.Count > 3)
				{
					IngredientProperties ingredient = destIngredients.Last<ThingDef>().ingredient;
					List<string> list = (ingredient != null) ? ingredient.mergeCompatibilityTags : null;
					if (list != null && list.Any<string>())
					{
						lostImportantIngredients = true;
					}
					destIngredients.RemoveLast<ThingDef>();
				}
			}
		}

		// Token: 0x04003B42 RID: 15170
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x04003B43 RID: 15171
		private List<string> cachedMergeCompatibilityTags;

		// Token: 0x04003B44 RID: 15172
		private const int MaxNumIngredients = 3;

		// Token: 0x04003B45 RID: 15173
		private static readonly List<ThingDef> tmpMergedIngredients = new List<ThingDef>();

		// Token: 0x04003B46 RID: 15174
		private static readonly List<string> tmpMergedIngredientTags = new List<string>();

		// Token: 0x04003B47 RID: 15175
		private static readonly Func<ThingDef, ThingDef, int> MostTagsCmp = new Func<ThingDef, ThingDef, int>(GenCollection.CompareBy<ThingDef, int>(new Func<ThingDef, int>(CompIngredients.<>c.<>9, ldftn(<.cctor>b__19_0))).Descending<ThingDef>().Compare);
	}
}
