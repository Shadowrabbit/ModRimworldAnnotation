using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E9A RID: 3738
	public class Thought_KilledInnocentAnimal : Thought_Memory
	{
		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060057D1 RID: 22481 RVA: 0x001DDE38 File Offset: 0x001DC038
		public override string LabelCap
		{
			get
			{
				string text = GenLabel.BestKindLabel(this.animal, this.gender, this.plural, -1);
				if (this.otherAnimals)
				{
					text += ", " + "Etc".Translate();
				}
				return base.CurStage.label.Formatted(text).CapitalizeFirst();
			}
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x001DDEA9 File Offset: 0x001DC0A9
		public void SetAnimal(Pawn animal)
		{
			this.animal = animal.kindDef;
			this.gender = animal.gender;
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x001DDEC4 File Offset: 0x001DC0C4
		public override void Notify_NewThoughtInGroupAdded(Thought_Memory memory)
		{
			base.Notify_NewThoughtInGroupAdded(memory);
			Thought_KilledInnocentAnimal thought_KilledInnocentAnimal;
			if ((thought_KilledInnocentAnimal = (memory as Thought_KilledInnocentAnimal)) != null)
			{
				if (thought_KilledInnocentAnimal.animal == this.animal)
				{
					this.plural = true;
					return;
				}
				this.otherAnimals = true;
			}
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x001DDF00 File Offset: 0x001DC100
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animal, "animal");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<bool>(ref this.plural, "plural", false, false);
			Scribe_Values.Look<bool>(ref this.otherAnimals, "otherAnimals", false, false);
		}

		// Token: 0x040033C8 RID: 13256
		public PawnKindDef animal;

		// Token: 0x040033C9 RID: 13257
		public Gender gender;

		// Token: 0x040033CA RID: 13258
		public bool plural;

		// Token: 0x040033CB RID: 13259
		public bool otherAnimals;
	}
}
