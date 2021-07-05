using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004AD RID: 1197
	public class PostLoadIniter
	{
		// Token: 0x06001DC9 RID: 7625 RVA: 0x000F7974 File Offset: 0x000F5B74
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
				}), false);
				return;
			}
			if (s == null)
			{
				Log.Warning("Trying to register null in RegisterforPostLoadInit.", false);
				return;
			}
			if (this.saveablesToPostLoad.Contains(s))
			{
				Log.Warning("Tried to register in RegisterforPostLoadInit when already registered: " + s, false);
				return;
			}
			this.saveablesToPostLoad.Add(s);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000F79F8 File Offset: 0x000F5BF8
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
					}), false);
				}
			}
			this.Clear();
			Scribe.loader.curParent = null;
			Scribe.loader.curPathRelToParent = null;
			Scribe.mode = LoadSaveMode.Inactive;
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0001AA15 File Offset: 0x00018C15
		public void Clear()
		{
			this.saveablesToPostLoad.Clear();
		}

		// Token: 0x04001547 RID: 5447
		private HashSet<IExposable> saveablesToPostLoad = new HashSet<IExposable>();
	}
}
