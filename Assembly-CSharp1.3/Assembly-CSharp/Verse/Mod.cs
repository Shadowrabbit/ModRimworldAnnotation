using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000234 RID: 564
	public abstract class Mod
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x0005B978 File Offset: 0x00059B78
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0005B980 File Offset: 0x00059B80
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0005B990 File Offset: 0x00059B90
		public T GetSettings<T>() where T : ModSettings, new()
		{
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)));
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

		// Token: 0x06001013 RID: 4115 RVA: 0x0005BA50 File Offset: 0x00059C50
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x04000C7F RID: 3199
		private ModSettings modSettings;

		// Token: 0x04000C80 RID: 3200
		private ModContentPack intContent;
	}
}
