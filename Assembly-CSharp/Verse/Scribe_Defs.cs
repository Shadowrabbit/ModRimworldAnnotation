using System;

namespace Verse
{
	// Token: 0x020004B9 RID: 1209
	public static class Scribe_Defs
	{
		// Token: 0x06001E05 RID: 7685 RVA: 0x000FA2CC File Offset: 0x000F84CC
		public static void Look<T>(ref T value, string label) where T : Def, new()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text;
				if (value == null)
				{
					text = "null";
				}
				else
				{
					text = value.defName;
				}
				Scribe_Values.Look<string>(ref text, label, "null", false);
				return;
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.DefFromNode<T>(Scribe.loader.curXmlParent[label]);
			}
		}
	}
}
