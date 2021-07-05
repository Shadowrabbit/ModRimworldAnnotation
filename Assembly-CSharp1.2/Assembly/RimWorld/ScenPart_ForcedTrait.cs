using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015DC RID: 5596
	public class ScenPart_ForcedTrait : ScenPart_PawnModifier
	{
		// Token: 0x060079A8 RID: 31144 RVA: 0x00051DF1 File Offset: 0x0004FFF1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraitDef>(ref this.trait, "trait");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
		}

		// Token: 0x060079A9 RID: 31145 RVA: 0x0024D638 File Offset: 0x0024B838
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			if (Widgets.ButtonText(scenPartRect.TopPart(0.333f), this.trait.DataAtDegree(this.degree).LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (TraitDef traitDef in from td in DefDatabase<TraitDef>.AllDefs
				orderby td.label
				select td)
				{
					foreach (TraitDegreeData localDeg2 in traitDef.degreeDatas)
					{
						TraitDef localDef = traitDef;
						TraitDegreeData localDeg = localDeg2;
						list.Add(new FloatMenuOption(localDeg.LabelCap, delegate()
						{
							this.trait = localDef;
							this.degree = localDeg.degree;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			base.DoPawnModifierEditInterface(scenPartRect.BottomPart(0.666f));
		}

		// Token: 0x060079AA RID: 31146 RVA: 0x0024D79C File Offset: 0x0024B99C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveTrait".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.trait.DataAtDegree(this.degree).LabelCap).CapitalizeFirst();
		}

		// Token: 0x060079AB RID: 31147 RVA: 0x00051E1B File Offset: 0x0005001B
		public override void Randomize()
		{
			base.Randomize();
			this.trait = DefDatabase<TraitDef>.GetRandom();
			this.degree = this.trait.degreeDatas.RandomElement<TraitDegreeData>().degree;
		}

		// Token: 0x060079AC RID: 31148 RVA: 0x0024D7FC File Offset: 0x0024B9FC
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_ForcedTrait scenPart_ForcedTrait = other as ScenPart_ForcedTrait;
			return scenPart_ForcedTrait == null || this.trait != scenPart_ForcedTrait.trait || !this.context.OverlapsWith(scenPart_ForcedTrait.context);
		}

		// Token: 0x060079AD RID: 31149 RVA: 0x0024D838 File Offset: 0x0024BA38
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.story == null || pawn.story.traits == null)
			{
				return;
			}
			if (pawn.story.traits.HasTrait(this.trait) && pawn.story.traits.DegreeOfTrait(this.trait) == this.degree)
			{
				return;
			}
			if (pawn.story.traits.HasTrait(this.trait))
			{
				pawn.story.traits.allTraits.RemoveAll((Trait tr) => tr.def == this.trait);
			}
			else
			{
				IEnumerable<Trait> source = from tr in pawn.story.traits.allTraits
				where !tr.ScenForced && !ScenPart_ForcedTrait.PawnHasTraitForcedByBackstory(pawn, tr.def)
				select tr;
				if (source.Any<Trait>())
				{
					Trait trait = (from tr in source
					where this.trait.ConflictsWith(tr.def)
					select tr).FirstOrDefault<Trait>();
					if (trait != null)
					{
						pawn.story.traits.allTraits.Remove(trait);
					}
					else
					{
						pawn.story.traits.allTraits.Remove(source.RandomElement<Trait>());
					}
				}
			}
			pawn.story.traits.GainTrait(new Trait(this.trait, this.degree, true));
		}

		// Token: 0x060079AE RID: 31150 RVA: 0x0024D9B4 File Offset: 0x0024BBB4
		private static bool PawnHasTraitForcedByBackstory(Pawn pawn, TraitDef trait)
		{
			return (pawn.story.childhood != null && pawn.story.childhood.forcedTraits != null && pawn.story.childhood.forcedTraits.Any((TraitEntry te) => te.def == trait)) || (pawn.story.adulthood != null && pawn.story.adulthood.forcedTraits != null && pawn.story.adulthood.forcedTraits.Any((TraitEntry te) => te.def == trait));
		}

		// Token: 0x04004FF4 RID: 20468
		private TraitDef trait;

		// Token: 0x04004FF5 RID: 20469
		private int degree;
	}
}
