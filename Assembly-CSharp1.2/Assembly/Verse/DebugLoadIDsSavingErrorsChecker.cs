using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004B2 RID: 1202
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x06001DE2 RID: 7650 RVA: 0x0001AA82 File Offset: 0x00018C82
		public void Clear()
		{
			if (!Prefs.DevMode)
			{
				return;
			}
			this.deepSaved.Clear();
			this.referenced.Clear();
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000F8B24 File Offset: 0x000F6D24
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
						}), false);
					}
				}
			}
			this.Clear();
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000F8BD8 File Offset: 0x000F6DD8
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
				}), false);
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
					if (!this.deepSaved.Add(loadReferenceable.GetUniqueLoadID()))
					{
						Log.Warning(string.Concat(new string[]
						{
							"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
							loadReferenceable.GetUniqueLoadID(),
							", but it's already here. label=",
							label,
							" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
						}), false);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error in GetUniqueLoadID(): " + arg, false);
				}
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000F8CA8 File Offset: 0x000F6EA8
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
				}), false);
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
				Log.Error("Error in GetUniqueLoadID(): " + arg, false);
			}
		}

		// Token: 0x0400155D RID: 5469
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x0400155E RID: 5470
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x020004B3 RID: 1203
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x06001DE7 RID: 7655 RVA: 0x0001AAC0 File Offset: 0x00018CC0
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06001DE8 RID: 7656 RVA: 0x0001AAD0 File Offset: 0x00018CD0
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06001DE9 RID: 7657 RVA: 0x0001AAE8 File Offset: 0x00018CE8
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06001DEA RID: 7658 RVA: 0x0001AB10 File Offset: 0x00018D10
			public override int GetHashCode()
			{
				return Gen.HashCombine<string>(Gen.HashCombine<string>(0, this.loadID), this.label);
			}

			// Token: 0x06001DEB RID: 7659 RVA: 0x0001AB29 File Offset: 0x00018D29
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06001DEC RID: 7660 RVA: 0x0001AB33 File Offset: 0x00018D33
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400155F RID: 5471
			public string loadID;

			// Token: 0x04001560 RID: 5472
			public string label;
		}
	}
}
