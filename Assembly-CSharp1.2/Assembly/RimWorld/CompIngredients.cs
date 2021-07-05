using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CF RID: 6095
	public class CompIngredients : ThingComp
	{
		// Token: 0x060086D0 RID: 34512 RVA: 0x0005A77A File Offset: 0x0005897A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.ingredients, "ingredients", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ingredients == null)
			{
				this.ingredients = new List<ThingDef>();
			}
		}

		// Token: 0x060086D1 RID: 34513 RVA: 0x0005A7B3 File Offset: 0x000589B3
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
			}
		}

		// Token: 0x060086D2 RID: 34514 RVA: 0x00279D34 File Offset: 0x00277F34
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

		// Token: 0x060086D3 RID: 34515 RVA: 0x00279D88 File Offset: 0x00277F88
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			List<ThingDef> list = otherStack.TryGetComp<CompIngredients>().ingredients;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.ingredients.Contains(list[i]))
				{
					this.ingredients.Add(list[i]);
				}
			}
			if (this.ingredients.Count > 3)
			{
				this.ingredients.Shuffle<ThingDef>();
				while (this.ingredients.Count > 3)
				{
					this.ingredients.Remove(this.ingredients[this.ingredients.Count - 1]);
				}
			}
		}

		// Token: 0x060086D4 RID: 34516 RVA: 0x00279E30 File Offset: 0x00278030
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ingredients.Count > 0)
			{
				stringBuilder.Append("Ingredients".Translate() + ": ");
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					stringBuilder.Append((i == 0) ? this.ingredients[i].LabelCap.Resolve() : this.ingredients[i].label);
					if (i < this.ingredients.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040056AA RID: 22186
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x040056AB RID: 22187
		private const int MaxNumIngredients = 3;
	}
}
