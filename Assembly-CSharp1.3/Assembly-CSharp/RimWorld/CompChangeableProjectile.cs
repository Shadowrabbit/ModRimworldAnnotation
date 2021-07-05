using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111B RID: 4379
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06006926 RID: 26918 RVA: 0x002376A8 File Offset: 0x002358A8
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06006927 RID: 26919 RVA: 0x002376B5 File Offset: 0x002358B5
		public ThingDef LoadedShell
		{
			get
			{
				if (this.loadedCount <= 0)
				{
					return null;
				}
				return this.loadedShell;
			}
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06006928 RID: 26920 RVA: 0x002376C8 File Offset: 0x002358C8
		public ThingDef Projectile
		{
			get
			{
				if (!this.Loaded)
				{
					return null;
				}
				return this.LoadedShell.projectileWhenLoaded;
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06006929 RID: 26921 RVA: 0x002376DF File Offset: 0x002358DF
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x0600692A RID: 26922 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x002376EA File Offset: 0x002358EA
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", Array.Empty<object>());
		}

		// Token: 0x0600692C RID: 26924 RVA: 0x00237724 File Offset: 0x00235924
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x0023777B File Offset: 0x0023597B
		public virtual void Notify_ProjectileLaunched()
		{
			if (this.loadedCount > 0)
			{
				this.loadedCount--;
			}
			if (this.loadedCount <= 0)
			{
				this.loadedShell = null;
			}
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x002377A4 File Offset: 0x002359A4
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count > 0) ? shell : null);
		}

		// Token: 0x0600692F RID: 26927 RVA: 0x002377C1 File Offset: 0x002359C1
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x002377E9 File Offset: 0x002359E9
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x00236108 File Offset: 0x00234308
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x04003ADD RID: 15069
		private ThingDef loadedShell;

		// Token: 0x04003ADE RID: 15070
		public int loadedCount;

		// Token: 0x04003ADF RID: 15071
		public StorageSettings allowedShellsSettings;
	}
}
