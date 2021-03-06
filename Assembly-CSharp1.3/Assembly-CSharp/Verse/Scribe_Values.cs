using System;

namespace Verse
{
	// Token: 0x02000337 RID: 823
	public static class Scribe_Values
	{
		// Token: 0x0600175A RID: 5978 RVA: 0x0008B51C File Offset: 0x0008971C
		public static void Look<T>(ref T value, string label, T defaultValue = default(T), bool forceSave = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (typeof(T) == typeof(TargetInfo))
				{
					Log.Error("Saving a TargetInfo " + label + " with Scribe_Values. TargetInfos must be saved with Scribe_TargetInfo.");
					return;
				}
				if (typeof(Thing).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a Thing reference " + label + ". Use Scribe_References or Scribe_Deep instead.");
					return;
				}
				if (typeof(IExposable).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a IExposable reference " + label + ". Use Scribe_References or Scribe_Deep instead.");
					return;
				}
				if (typeof(Def).IsAssignableFrom(typeof(T)))
				{
					Log.Error("Using Scribe_Values with a Def " + label + ". Use Scribe_Defs instead.");
					return;
				}
				if (forceSave || (value == null && defaultValue != null) || (value != null && !value.Equals(defaultValue)))
				{
					if (value == null)
					{
						if (!Scribe.EnterNode(label))
						{
							return;
						}
						try
						{
							Scribe.saver.WriteAttribute("IsNull", "True");
							return;
						}
						finally
						{
							Scribe.ExitNode();
						}
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.ValueFromNode<T>(Scribe.loader.curXmlParent[label], defaultValue);
			}
		}
	}
}
