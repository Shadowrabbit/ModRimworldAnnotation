using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100B RID: 4107
	public class InstructionDef : Def
	{
		// Token: 0x0600599C RID: 22940 RVA: 0x0003E3BB File Offset: 0x0003C5BB
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.instructionClass == null)
			{
				yield return "no instruction class";
			}
			if (this.text.NullOrEmpty())
			{
				yield return "no text";
			}
			if (this.eventTagInitiate.NullOrEmpty())
			{
				yield return "no eventTagInitiate";
			}
			InstructionDef.tmpParseErrors.Clear();
			this.text.AdjustedForKeys(InstructionDef.tmpParseErrors, false);
			int num;
			for (int i = 0; i < InstructionDef.tmpParseErrors.Count; i = num + 1)
			{
				yield return "text error: " + InstructionDef.tmpParseErrors[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003C3A RID: 15418
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x04003C3B RID: 15419
		[MustTranslate]
		public string text;

		// Token: 0x04003C3C RID: 15420
		public bool startCentered;

		// Token: 0x04003C3D RID: 15421
		public bool tutorialModeOnly = true;

		// Token: 0x04003C3E RID: 15422
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x04003C3F RID: 15423
		public InstructionDef eventTagInitiateSource;

		// Token: 0x04003C40 RID: 15424
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x04003C41 RID: 15425
		[NoTranslate]
		public List<string> actionTagsAllowed;

		// Token: 0x04003C42 RID: 15426
		[MustTranslate]
		public string rejectInputMessage;

		// Token: 0x04003C43 RID: 15427
		public ConceptDef concept;

		// Token: 0x04003C44 RID: 15428
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x04003C45 RID: 15429
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x04003C46 RID: 15430
		public int targetCount;

		// Token: 0x04003C47 RID: 15431
		public ThingDef thingDef;

		// Token: 0x04003C48 RID: 15432
		public RecipeDef recipeDef;

		// Token: 0x04003C49 RID: 15433
		public int recipeTargetCount = 1;

		// Token: 0x04003C4A RID: 15434
		public ThingDef giveOnActivateDef;

		// Token: 0x04003C4B RID: 15435
		public int giveOnActivateCount;

		// Token: 0x04003C4C RID: 15436
		public bool endTutorial;

		// Token: 0x04003C4D RID: 15437
		public bool resetBuildDesignatorStuffs;

		// Token: 0x04003C4E RID: 15438
		private static List<string> tmpParseErrors = new List<string>();
	}
}
