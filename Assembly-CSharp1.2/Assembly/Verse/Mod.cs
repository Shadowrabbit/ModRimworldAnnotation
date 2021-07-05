using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000335 RID: 821
	public abstract class Mod
	{
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060014E5 RID: 5349 RVA: 0x00014F4D File Offset: 0x0001314D
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00014F55 File Offset: 0x00013155
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x000D0DB8 File Offset: 0x000CEFB8
		public T GetSettings<T>() where T : ModSettings, new()
		{
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)), false);
				return default(T);
			}
			if (this.modSettings != null)
			{
				return (T)((object)this.modSettings);
			}
			this.modSettings = LoadedModManager.ReadModSettings<T>(this.intContent.FolderName, base.GetType().Name);
			this.modSettings.Mod = this;
			return this.modSettings as T;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00014F64 File Offset: 0x00013164
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0000A713 File Offset: 0x00008913
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x04001040 RID: 4160
		private ModSettings modSettings;

		// Token: 0x04001041 RID: 4161
		private ModContentPack intContent;
	}
}
