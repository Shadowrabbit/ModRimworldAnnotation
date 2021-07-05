using System;

namespace RimWorld
{
	// Token: 0x02001353 RID: 4947
	public class ITab_Shells : ITab_Storage
	{
		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x060077C1 RID: 30657 RVA: 0x002A3210 File Offset: 0x002A1410
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

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x060077C2 RID: 30658 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060077C3 RID: 30659 RVA: 0x002A3246 File Offset: 0x002A1446
		public ITab_Shells()
		{
			this.labelKey = "TabShells";
			this.tutorTag = "Shells";
		}
	}
}
