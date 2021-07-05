using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AE8 RID: 2792
	public class InstructionDef : Def
	{
		// Token: 0x060041B6 RID: 16822 RVA: 0x001603B5 File Offset: 0x0015E5B5
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

		// Token: 0x040027EE RID: 10222
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x040027EF RID: 10223
		[MustTranslate]
		public string text;

		// Token: 0x040027F0 RID: 10224
		public bool startCentered;

		// Token: 0x040027F1 RID: 10225
		public bool tutorialModeOnly = true;

		// Token: 0x040027F2 RID: 10226
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x040027F3 RID: 10227
		public InstructionDef eventTagInitiateSource;

		// Token: 0x040027F4 RID: 10228
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x040027F5 RID: 10229
		[NoTranslate]
		public List<string> actionTagsAllowed;

		// Token: 0x040027F6 RID: 10230
		[MustTranslate]
		public string rejectInputMessage;

		// Token: 0x040027F7 RID: 10231
		public ConceptDef concept;

		// Token: 0x040027F8 RID: 10232
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x040027F9 RID: 10233
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x040027FA RID: 10234
		public int targetCount;

		// Token: 0x040027FB RID: 10235
		public ThingDef thingDef;

		// Token: 0x040027FC RID: 10236
		public RecipeDef recipeDef;

		// Token: 0x040027FD RID: 10237
		public int recipeTargetCount = 1;

		// Token: 0x040027FE RID: 10238
		public ThingDef giveOnActivateDef;

		// Token: 0x040027FF RID: 10239
		public int giveOnActivateCount;

		// Token: 0x04002800 RID: 10240
		public bool endTutorial;

		// Token: 0x04002801 RID: 10241
		public bool resetBuildDesignatorStuffs;

		// Token: 0x04002802 RID: 10242
		private static List<string> tmpParseErrors = new List<string>();
	}
}
