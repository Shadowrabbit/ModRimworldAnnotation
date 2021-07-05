using System;

namespace Verse
{
	// Token: 0x0200035C RID: 860
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x00015B1E File Offset: 0x00013D1E
		// (set) Token: 0x06001610 RID: 5648 RVA: 0x00015B26 File Offset: 0x00013D26
		public Mod Mod { get; internal set; }

		// Token: 0x06001611 RID: 5649 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExposeData()
		{
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x00015B2F File Offset: 0x00013D2F
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
