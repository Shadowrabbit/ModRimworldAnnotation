using System;

namespace Verse
{
	// Token: 0x020004B6 RID: 1206
	public static class Scribe_BodyParts
	{
		// Token: 0x06001DFA RID: 7674 RVA: 0x000F922C File Offset: 0x000F742C
		public static void Look(ref BodyPartRecord part, string label, BodyPartRecord defaultValue = null)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (part == defaultValue || !Scribe.EnterNode(label))
				{
					return;
				}
				try
				{
					if (part == null)
					{
						Scribe.saver.WriteAttribute("IsNull", "True");
						return;
					}
					string defName = part.body.defName;
					Scribe_Values.Look<string>(ref defName, "body", null, false);
					int index = part.Index;
					Scribe_Values.Look<int>(ref index, "index", 0, true);
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				part = ScribeExtractor.BodyPartFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
			}
		}
	}
}
