using System;

namespace RimWorld
{
	// Token: 0x02000E91 RID: 3729
	public static class ThoughtMaker
	{
		// Token: 0x06005798 RID: 22424 RVA: 0x001DD38E File Offset: 0x001DB58E
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x001DD3AD File Offset: 0x001DB5AD
		public static Thought_Memory MakeThought(ThoughtDef def, int forcedStage)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.SetForcedStage(forcedStage);
			thought_Memory.Init();
			return thought_Memory;
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x001DD3D3 File Offset: 0x001DB5D3
		public static Thought_Memory MakeThought(ThoughtDef def, Precept sourcePrecept)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.sourcePrecept = sourcePrecept;
			thought_Memory.Init();
			return thought_Memory;
		}
	}
}
