using System;

namespace RimWorld
{
	// Token: 0x02001B1D RID: 6941
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x060098B1 RID: 39089 RVA: 0x002CE1C0 File Offset: 0x002CC3C0
		protected override IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				IStoreSettingsParent selStoreSettingsParent = base.SelStoreSettingsParent;
				if (selStoreSettingsParent != null)
				{
					return selStoreSettingsParent;
				}
				Building_TurretGun building_TurretGun = base.SelObject as Building_TurretGun;
				if (building_TurretGun != null)
				{
					return base.GetThingOrThingCompStoreSettingsParent(building_TurretGun.gun);
				}
				return null;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x060098B2 RID: 39090 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060098B3 RID: 39091 RVA: 0x00065CA8 File Offset: 0x00063EA8
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}
	}
}
