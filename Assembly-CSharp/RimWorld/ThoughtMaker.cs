using System;

namespace RimWorld
{
	// Token: 0x02001562 RID: 5474
	public static class ThoughtMaker
	{
		// Token: 0x060076B2 RID: 30386 RVA: 0x000500E5 File Offset: 0x0004E2E5
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x00050104 File Offset: 0x0004E304
		public static Thought_Memory MakeThought(ThoughtDef def, int forcedStage)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.SetForcedStage(forcedStage);
			thought_Memory.Init();
			return thought_Memory;
		}
	}
}
