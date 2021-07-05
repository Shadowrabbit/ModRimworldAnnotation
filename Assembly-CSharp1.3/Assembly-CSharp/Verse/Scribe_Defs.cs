using System;

namespace Verse
{
	// Token: 0x02000333 RID: 819
	public static class Scribe_Defs
	{
		// Token: 0x06001742 RID: 5954 RVA: 0x0008AAB0 File Offset: 0x00088CB0
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
