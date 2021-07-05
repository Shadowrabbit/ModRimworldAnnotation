using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001074 RID: 4212
	public abstract class Building_CorpseCasket : Building_Casket, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x060063F5 RID: 25589 RVA: 0x0021BE79 File Offset: 0x0021A079
		public virtual bool StorageTabVisible
		{
			get
			{
				return !this.HasCorpse;
			}
		}

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x060063F6 RID: 25590 RVA: 0x0021BE84 File Offset: 0x0021A084
		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x060063F7 RID: 25591 RVA: 0x0021BE90 File Offset: 0x0021A090
		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x0021BECB File Offset: 0x0021A0CB
		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		// Token: 0x060063F9 RID: 25593 RVA: 0x0021BED3 File Offset: 0x0021A0D3
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x060063FA RID: 25594 RVA: 0x0021BEE8 File Offset: 0x0021A0E8
		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x060063FB RID: 25595 RVA: 0x0021BF34 File Offset: 0x0021A134
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		// Token: 0x0400387D RID: 14461
		protected StorageSettings storageSettings;
	}
}
