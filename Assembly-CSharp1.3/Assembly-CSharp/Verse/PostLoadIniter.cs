using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000328 RID: 808
	public class PostLoadIniter
	{
		// Token: 0x06001701 RID: 5889 RVA: 0x000879A0 File Offset: 0x00085BA0
		public void RegisterForPostLoadInit(IExposable s)
		{
			if (Scribe.mode != LoadSaveMode.LoadingVars)
			{
				Log.Error(string.Concat(new object[]
				{
					"Registered ",
					s,
					" for post load init, but current mode is ",
					Scribe.mode
				}));
				return;
			}
			if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.");
				return;
			}
			if (this.saveablesToPostLoad.Contains(s))
			{
				Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + s);
				return;
			}
			this.saveablesToPostLoad.Add(s);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00087A24 File Offset: 0x00085C24
		public void DoAllPostLoadInits()
		{
			Scribe.mode = LoadSaveMode.PostLoadInit;
			foreach (IExposable exposable in this.saveablesToPostLoad)
			{
				try
				{
					Scribe.loader.curParent = exposable;
					Scribe.loader.curPathRelToParent = null;
					exposable.ExposeData();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not do PostLoadInit on ",
						exposable.ToStringSafe<IExposable>(),
						": ",
						ex
					}));
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00087AF4 File Offset: 0x00085CF4
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x04000FFD RID: 4093
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
