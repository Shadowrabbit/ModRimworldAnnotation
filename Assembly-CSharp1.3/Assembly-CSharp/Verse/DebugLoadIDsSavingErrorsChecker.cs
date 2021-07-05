using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200032C RID: 812
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x0600171A RID: 5914 RVA: 0x00088BB1 File Offset: 0x00086DB1
		public void Clear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			this.deepSaved.Clear();
			this.deepSavedInfo.Clear();
			this.referenced.Clear();
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00088BDC File Offset: 0x00086DDC
		public void CheckForErrorsAndClear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (!Scribe.saver.savingForDebug)
			{
				foreach (DebugLoadIDsSavingErrorsChecker.ReferencedObject referencedObject in this.referenced)
				{
					if (!this.deepSaved.Contains(referencedObject.loadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"Object with load ID ",
							referencedObject.loadID,
							" is referenced (xml node name: ",
							referencedObject.label,
							") but is not deep-saved. This will cause errors during loading."
						}));
					}
				}
			}
			this.Clear();
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00088C90 File Offset: 0x00086E90
		public void RegisterDeepSaved(object obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					obj,
					", but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (obj == null)
			{
				return;
			}
			ILoadReferenceable loadReferenceable = obj as ILoadReferenceable;
			if (loadReferenceable != null)
			{
				try
				{
					string uniqueLoadID = loadReferenceable.GetUniqueLoadID();
					if (!this.deepSaved.Add(uniqueLoadID))
					{
						Log.Warning(string.Concat(new string[]
						{
							"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
							uniqueLoadID,
							", but it's already here. label=",
							label,
							" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
						}));
						string text;
						if (this.deepSavedInfo.TryGetValue(uniqueLoadID, out text))
						{
							Log.Warning(string.Concat(new object[]
							{
								loadReferenceable.GetType(),
								" was already deepsaved at ",
								text,
								"."
							}));
						}
					}
					else
					{
						this.deepSavedInfo.Add(uniqueLoadID, Scribe.saver.CurPath);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error in GetUniqueLoadID(): " + arg);
				}
			}
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00088DB0 File Offset: 0x00086FB0
		public void RegisterReferenced(ILoadReferenceable obj, string label)
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					obj,
					", but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (obj == null)
			{
				return;
			}
			try
			{
				this.referenced.Add(new DebugLoadIDsSavingErrorsChecker.ReferencedObject(obj.GetUniqueLoadID(), label));
			}
			catch (Exception arg)
			{
				Log.Error("Error in GetUniqueLoadID(): " + arg);
			}
		}

		// Token: 0x0400100D RID: 4109
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x0400100E RID: 4110
		private Dictionary<string, string> deepSavedInfo = new Dictionary<string, string>();

		// Token: 0x0400100F RID: 4111
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x02001A5A RID: 6746
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x06009C89 RID: 40073 RVA: 0x00369D0B File Offset: 0x00367F0B
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06009C8A RID: 40074 RVA: 0x00369D1B File Offset: 0x00367F1B
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06009C8B RID: 40075 RVA: 0x00369D33 File Offset: 0x00367F33
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06009C8C RID: 40076 RVA: 0x00369D5B File Offset: 0x00367F5B
			public override int GetHashCode()
			{
				return Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.loadID), this.label);
			}

			// Token: 0x06009C8D RID: 40077 RVA: 0x00369D74 File Offset: 0x00367F74
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06009C8E RID: 40078 RVA: 0x00369D7E File Offset: 0x00367F7E
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x040064D3 RID: 25811
			public string loadID;

			// Token: 0x040064D4 RID: 25812
			public string label;
		}
	}
}
