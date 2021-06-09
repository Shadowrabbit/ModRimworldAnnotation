using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020004AA RID: 1194
	public class LoadIDsWantedBank
	{
		// Token: 0x06001DBE RID: 7614 RVA: 0x000F7444 File Offset: 0x000F5644
		public void ConfirmClear()
		{
			if (this.idsRead.Count > 0 || this.idListsRead.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Not all loadIDs which were read were consumed.");
				if (this.idsRead.Count > 0)
				{
					stringBuilder.AppendLine("Singles:");
					for (int i = 0; i < this.idsRead.Count; i++)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  ",
							this.idsRead[i].targetLoadID.ToStringSafe<string>(),
							" of type ",
							this.idsRead[i].targetType,
							". pathRelToParent=",
							this.idsRead[i].pathRelToParent,
							", parent=",
							this.idsRead[i].parent.ToStringSafe<IExposable>()
						}));
					}
				}
				if (this.idListsRead.Count > 0)
				{
					stringBuilder.AppendLine("Lists:");
					for (int j = 0; j < this.idListsRead.Count; j++)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"  List with ",
							(this.idListsRead[j].targetLoadIDs != null) ? this.idListsRead[j].targetLoadIDs.Count : 0,
							" elements. pathRelToParent=",
							this.idListsRead[j].pathRelToParent,
							", parent=",
							this.idListsRead[j].parent.ToStringSafe<IExposable>()
						}));
					}
				}
				Log.Warning(stringBuilder.ToString().TrimEndNewlines(), false);
			}
			this.Clear();
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0001A9A9 File Offset: 0x00018BA9
		public void Clear()
		{
			this.idsRead.Clear();
			this.idListsRead.Clear();
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000F7624 File Offset: 0x000F5824
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idsRead.Count; i++)
			{
				if (this.idsRead[i].parent == parent && this.idsRead[i].pathRelToParent == pathRelToParent)
				{
					Log.Error(string.Concat(new string[]
					{
						"Tried to register the same load ID twice: ",
						targetLoadID,
						", pathRelToParent=",
						pathRelToParent,
						", parent=",
						parent.ToStringSafe<IExposable>()
					}), false);
					return;
				}
			}
			this.idsRead.Add(new LoadIDsWantedBank.IdRecord(targetLoadID, targetType, pathRelToParent, parent));
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000F76C8 File Offset: 0x000F58C8
		public void RegisterLoadIDReadFromXml(string targetLoadID, Type targetType, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDReadFromXml(targetLoadID, targetType, text, Scribe.loader.curParent);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x000F7708 File Offset: 0x000F5908
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idListsRead.Count; i++)
			{
				if (this.idListsRead[i].parent == parent && this.idListsRead[i].pathRelToParent == pathRelToParent)
				{
					Log.Error("Tried to register the same list of load IDs twice. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>(), false);
					return;
				}
			}
			this.idListsRead.Add(new LoadIDsWantedBank.IdListRecord(targetLoadIDList, pathRelToParent, parent));
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000F7788 File Offset: 0x000F5988
		public void RegisterLoadIDListReadFromXml(List<string> targetLoadIDList, string toAppendToPathRelToParent)
		{
			string text = Scribe.loader.curPathRelToParent;
			if (!toAppendToPathRelToParent.NullOrEmpty())
			{
				text = text + "/" + toAppendToPathRelToParent;
			}
			this.RegisterLoadIDListReadFromXml(targetLoadIDList, text, Scribe.loader.curParent);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000F77C8 File Offset: 0x000F59C8
		public string Take<T>(string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idsRead.Count; i++)
			{
				if (this.idsRead[i].parent == parent && this.idsRead[i].pathRelToParent == pathRelToParent)
				{
					string targetLoadID = this.idsRead[i].targetLoadID;
					if (typeof(T) != this.idsRead[i].targetType)
					{
						Log.Error(string.Concat(new object[]
						{
							"Trying to get load ID of object of type ",
							typeof(T),
							", but it was registered as ",
							this.idsRead[i].targetType,
							". pathRelToParent=",
							pathRelToParent,
							", parent=",
							parent.ToStringSafe<IExposable>()
						}), false);
					}
					this.idsRead.RemoveAt(i);
					return targetLoadID;
				}
			}
			Log.Error("Could not get load ID. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>(), false);
			return null;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000F78E4 File Offset: 0x000F5AE4
		public List<string> TakeList(string pathRelToParent, IExposable parent)
		{
			for (int i = 0; i < this.idListsRead.Count; i++)
			{
				if (this.idListsRead[i].parent == parent && this.idListsRead[i].pathRelToParent == pathRelToParent)
				{
					List<string> targetLoadIDs = this.idListsRead[i].targetLoadIDs;
					this.idListsRead.RemoveAt(i);
					return targetLoadIDs;
				}
			}
			Log.Error("Could not get load IDs list. We're asking for something which was never added during LoadingVars. pathRelToParent=" + pathRelToParent + ", parent=" + parent.ToStringSafe<IExposable>(), false);
			return new List<string>();
		}

		// Token: 0x0400153E RID: 5438
		private List<LoadIDsWantedBank.IdRecord> idsRead = new List<LoadIDsWantedBank.IdRecord>();

		// Token: 0x0400153F RID: 5439
		private List<LoadIDsWantedBank.IdListRecord> idListsRead = new List<LoadIDsWantedBank.IdListRecord>();

		// Token: 0x020004AB RID: 1195
		private struct IdRecord
		{
			// Token: 0x06001DC7 RID: 7623 RVA: 0x0001A9DF File Offset: 0x00018BDF
			public IdRecord(string targetLoadID, Type targetType, string pathRelToParent, IExposable parent)
			{
				this.targetLoadID = targetLoadID;
				this.targetType = targetType;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04001540 RID: 5440
			public string targetLoadID;

			// Token: 0x04001541 RID: 5441
			public Type targetType;

			// Token: 0x04001542 RID: 5442
			public string pathRelToParent;

			// Token: 0x04001543 RID: 5443
			public IExposable parent;
		}

		// Token: 0x020004AC RID: 1196
		private struct IdListRecord
		{
			// Token: 0x06001DC8 RID: 7624 RVA: 0x0001A9FE File Offset: 0x00018BFE
			public IdListRecord(List<string> targetLoadIDs, string pathRelToParent, IExposable parent)
			{
				this.targetLoadIDs = targetLoadIDs;
				this.pathRelToParent = pathRelToParent;
				this.parent = parent;
			}

			// Token: 0x04001544 RID: 5444
			public List<string> targetLoadIDs;

			// Token: 0x04001545 RID: 5445
			public string pathRelToParent;

			// Token: 0x04001546 RID: 5446
			public IExposable parent;
		}
	}
}
