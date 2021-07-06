using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AD RID: 6061
	public class CompChangeableProjectile : ThingComp, IStoreSettingsParent
	{
		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x060085FA RID: 34298 RVA: 0x00059D5F File Offset: 0x00057F5F
		public CompProperties_ChangeableProjectile Props
		{
			get
			{
				return (CompProperties_ChangeableProjectile)this.props;
			}
		}

		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x060085FB RID: 34299 RVA: 0x00059D6C File Offset: 0x00057F6C
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

		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x060085FC RID: 34300 RVA: 0x00059D7F File Offset: 0x00057F7F
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

		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x060085FD RID: 34301 RVA: 0x00059D96 File Offset: 0x00057F96
		public bool Loaded
		{
			get
			{
				return this.LoadedShell != null;
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x060085FE RID: 34302 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060085FF RID: 34303 RVA: 0x00059DA1 File Offset: 0x00057FA1
		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.loadedShell, "loadedShell");
			Scribe_Values.Look<int>(ref this.loadedCount, "loadedCount", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedShellsSettings, "allowedShellsSettings", Array.Empty<object>());
		}

		// Token: 0x06008600 RID: 34304 RVA: 0x0027750C File Offset: 0x0027570C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedShellsSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedShellsSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06008601 RID: 34305 RVA: 0x00059DDA File Offset: 0x00057FDA
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

		// Token: 0x06008602 RID: 34306 RVA: 0x00059E03 File Offset: 0x00058003
		public void LoadShell(ThingDef shell, int count)
		{
			this.loadedCount = Mathf.Max(count, 0);
			this.loadedShell = ((count > 0) ? shell : null);
		}

		// Token: 0x06008603 RID: 34307 RVA: 0x00059E20 File Offset: 0x00058020
		public Thing RemoveShell()
		{
			Thing thing = ThingMaker.MakeThing(this.loadedShell, null);
			thing.stackCount = this.loadedCount;
			this.loadedCount = 0;
			this.loadedShell = null;
			return thing;
		}

		// Token: 0x06008604 RID: 34308 RVA: 0x00059E48 File Offset: 0x00058048
		public StorageSettings GetStoreSettings()
		{
			return this.allowedShellsSettings;
		}

		// Token: 0x06008605 RID: 34309 RVA: 0x00059E50 File Offset: 0x00058050
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x04005662 RID: 22114
		private ThingDef loadedShell;

		// Token: 0x04005663 RID: 22115
		public int loadedCount;

		// Token: 0x04005664 RID: 22116
		public StorageSettings allowedShellsSettings;
	}
}
