using System;

namespace Verse
{
	// Token: 0x02000243 RID: 579
	public abstract class ModSettings : IExposable
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0005E890 File Offset: 0x0005CA90
		// (set) Token: 0x060010BC RID: 4284 RVA: 0x0005E898 File Offset: 0x0005CA98
		public Mod Mod { get; internal set; }

		// Token: 0x060010BD RID: 4285 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0005E8A1 File Offset: 0x0005CAA1
		public void Write()
		{
			LoadedModManager.WriteModSettings(this.Mod.Content.FolderName, this.Mod.GetType().Name, this);
		}
	}
}
