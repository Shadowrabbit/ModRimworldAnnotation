using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000966 RID: 2406
	public class Thought_TameVeneratedAnimalDied : Thought_Memory
	{
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06003D39 RID: 15673 RVA: 0x00151918 File Offset: 0x0014FB18
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.animalKindLabel.Named("ANIMALKIND")).CapitalizeFirst();
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003D3A RID: 15674 RVA: 0x00151954 File Offset: 0x0014FB54
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.animalKindLabel.Named("ANIMALKIND")).CapitalizeFirst() + base.CausedByBeliefInPrecept;
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00151999 File Offset: 0x0014FB99
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.animalKindLabel, "animalKindLabel", null, false);
		}

		// Token: 0x040020CF RID: 8399
		public string animalKindLabel;
	}
}
