using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC2 RID: 4034
	public class PawnRelationDef : Def
	{
		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06005832 RID: 22578 RVA: 0x0003D3B1 File Offset: 0x0003B5B1
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

		// Token: 0x06005833 RID: 22579 RVA: 0x0003D3E3 File Offset: 0x0003B5E3
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x0003D408 File Offset: 0x0003B608
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x0003D416 File Offset: 0x0003B616
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x0003D436 File Offset: 0x0003B636
		public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.lostThoughtFemale;
			}
			return this.lostThought;
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x0003D456 File Offset: 0x0003B656
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		// Token: 0x06005838 RID: 22584 RVA: 0x0003D476 File Offset: 0x0003B676
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

		// Token: 0x04003A36 RID: 14902
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x04003A37 RID: 14903
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04003A38 RID: 14904
		public float importance;

		// Token: 0x04003A39 RID: 14905
		public bool implied;

		// Token: 0x04003A3A RID: 14906
		public bool reflexive;

		// Token: 0x04003A3B RID: 14907
		public int opinionOffset;

		// Token: 0x04003A3C RID: 14908
		public float generationChanceFactor;

		// Token: 0x04003A3D RID: 14909
		public float romanceChanceFactor = 1f;

		// Token: 0x04003A3E RID: 14910
		public float incestOpinionOffset;

		// Token: 0x04003A3F RID: 14911
		public bool familyByBloodRelation;

		// Token: 0x04003A40 RID: 14912
		public ThoughtDef diedThought;

		// Token: 0x04003A41 RID: 14913
		public ThoughtDef diedThoughtFemale;

		// Token: 0x04003A42 RID: 14914
		public ThoughtDef lostThought;

		// Token: 0x04003A43 RID: 14915
		public ThoughtDef lostThoughtFemale;

		// Token: 0x04003A44 RID: 14916
		public List<ThoughtDef> soldThoughts;

		// Token: 0x04003A45 RID: 14917
		public ThoughtDef killedThought;

		// Token: 0x04003A46 RID: 14918
		public ThoughtDef killedThoughtFemale;

		// Token: 0x04003A47 RID: 14919
		[Unsaved(false)]
		private PawnRelationWorker workerInt;
	}
}
