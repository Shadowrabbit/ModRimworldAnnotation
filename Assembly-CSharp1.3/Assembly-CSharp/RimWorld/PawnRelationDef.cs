using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9F RID: 2719
	public class PawnRelationDef : Def
	{
		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060040AF RID: 16559 RVA: 0x0015DA24 File Offset: 0x0015BC24
		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0015DA56 File Offset: 0x0015BC56
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0015DA7B File Offset: 0x0015BC7B
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0015DA89 File Offset: 0x0015BC89
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0015DAA9 File Offset: 0x0015BCA9
		public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.lostThoughtFemale;
			}
			return this.lostThought;
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0015DAC9 File Offset: 0x0015BCC9
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0015DAE9 File Offset: 0x0015BCE9
		public ThoughtDef GetGenderSpecificThought(Pawn pawn, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
			{
				return this.GetGenderSpecificLostThought(pawn);
			}
			return this.GetGenderSpecificDiedThought(pawn);
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0015DAFE File Offset: 0x0015BCFE
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implied && this.reflexive)
			{
				yield return this.defName + ": implied relations can't use the \"reflexive\" option.";
				this.reflexive = false;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400258F RID: 9615
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x04002590 RID: 9616
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04002591 RID: 9617
		public float importance;

		// Token: 0x04002592 RID: 9618
		public bool implied;

		// Token: 0x04002593 RID: 9619
		public bool reflexive;

		// Token: 0x04002594 RID: 9620
		public int opinionOffset;

		// Token: 0x04002595 RID: 9621
		public float generationChanceFactor;

		// Token: 0x04002596 RID: 9622
		public float romanceChanceFactor = 1f;

		// Token: 0x04002597 RID: 9623
		public float incestOpinionOffset;

		// Token: 0x04002598 RID: 9624
		public bool familyByBloodRelation;

		// Token: 0x04002599 RID: 9625
		public ThoughtDef diedThought;

		// Token: 0x0400259A RID: 9626
		public ThoughtDef diedThoughtFemale;

		// Token: 0x0400259B RID: 9627
		public ThoughtDef lostThought;

		// Token: 0x0400259C RID: 9628
		public ThoughtDef lostThoughtFemale;

		// Token: 0x0400259D RID: 9629
		public List<ThoughtDef> soldThoughts;

		// Token: 0x0400259E RID: 9630
		public ThoughtDef killedThought;

		// Token: 0x0400259F RID: 9631
		public ThoughtDef killedThoughtFemale;

		// Token: 0x040025A0 RID: 9632
		[Unsaved(false)]
		private PawnRelationWorker workerInt;
	}
}
