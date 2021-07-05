using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x02000326 RID: 806
	public class LoadIDsWantedBank
	{
		// Token: 0x060016F4 RID: 5876 RVA: 0x00087210 File Offset: 0x00085410
		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> keyValuePair in this.idsRead)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  ",
							keyValuePair.Value.targetLoadID.ToStringSafe<string>(),
							" of type ",
							keyValuePair.Value.targetType,
							". pathRelToParent=",
							keyValuePair.Value.pathRelToParent,
							", parent=",
							keyValuePair.Value.parent.ToStringSafe<IExposable>()
						}));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					foreach (KeyValuePair<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> keyValuePair2 in this.idListsRead)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  List with ",
							(keyValuePair2.Value.targetLoadIDs != null) ? keyValuePair2.Value.targetLoadIDs.Count : 0,
							" elements. pathRelToParent=",
							keyValuePair2.Value.pathRelToParent,
							", parent=",
							keyValuePair2.Value.parent.ToStringSafe<IExposable>()
						}));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines());
			}
			this.Clear();
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00087414 File Offset: 0x00085614
		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x0008742C File Offset: 0x0008562C
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			if (this.idsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to register the same load ID twice: ",
					targetLoadID,
					", pathRelToParent=",
					pathRelToParent,
					", parent=",
					parent.ToStringSafe<IExposable>()
				}));
				return;
			}
			LoadIDsWantedBank.IdRecord value = new LoadIDsWantedBank.IdRecord(targetLoadID, targetType, pathRelToParent, parent);
			this.idsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x000874AC File Offset: 0x000856AC
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x000874EC File Offset: 0x000856EC
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			if (this.idListsRead.ContainsKey(new ValueTuple<IExposable, string>(parent, pathRelToParent)))
			{
				Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
				return;
			}
			LoadIDsWantedBank.IdListRecord value = new LoadIDsWantedBank.IdListRecord(targetLoadIDList, pathRelToParent, parent);
			this.idListsRead.Add(new ValueTuple<IExposable, string>(parent, pathRelToParent), value);
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00087548 File Offset: 0x00085748
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00087588 File Offset: 0x00085788
		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdRecord idRecord;
			if (this.idsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idRecord))
			{
				string targetLoadID = idRecord.targetLoadID;
				if (typeof(T) != idRecord.targetType)
				{
					Log.Error(string.Concat(new object[]
					{
						"Trying to get load ID of object of type ",
						typeof(T),
						", but it was registered as ",
						idRecord.targetType,
						". pathRelToParent=",
						pathRelToParent,
						", parent=",
						parent.ToStringSafe<IExposable>()
					}));
				}
				this.idsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadID;
			}
			Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return null;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00087650 File Offset: 0x00085850
		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			LoadIDsWantedBank.IdListRecord idListRecord;
			if (this.idListsRead.TryGetValue(new ValueTuple<IExposable, string>(parent, pathRelToParent), out idListRecord))
			{
				List<string> targetLoadIDs = idListRecord.targetLoadIDs;
				this.idListsRead.Remove(new ValueTuple<IExposable, string>(parent, pathRelToParent));
				return targetLoadIDs;
			}
			Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>());
			return new List<string>();
		}

		// Token: 0x04000FFA RID: 4090
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord> idsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdRecord>();

		// Token: 0x04000FFB RID: 4091
		private Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord> idListsRead = new Dictionary<ValueTuple<IExposable, string>, LoadIDsWantedBank.IdListRecord>();

		// Token: 0x02001A58 RID: 6744
		private struct IdRecord
		{
			// Token: 0x06009C87 RID: 40071 RVA: 0x00369CD5 File Offset: 0x00367ED5
			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x040064CC RID: 25804
			public string targetLoadID;

			// Token: 0x040064CD RID: 25805
			public Type targetType;

			// Token: 0x040064CE RID: 25806
			public string pathRelToParent;

			// Token: 0x040064CF RID: 25807
			public IExposable parent;
		}

		// Token: 0x02001A59 RID: 6745
		private struct IdListRecord
		{
			// Token: 0x06009C88 RID: 40072 RVA: 0x00369CF4 File Offset: 0x00367EF4
			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x040064D0 RID: 25808
			public List<string> targetLoadIDs;

			// Token: 0x040064D1 RID: 25809
			public string pathRelToParent;

			// Token: 0x040064D2 RID: 25810
			public IExposable parent;
		}
	}
}
