using System;

namespace Verse
{
	// Token: 0x02000330 RID: 816
	public static class Scribe_BodyParts
	{
		// Token: 0x06001737 RID: 5943 RVA: 0x000899F4 File Offset: 0x00087BF4
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
