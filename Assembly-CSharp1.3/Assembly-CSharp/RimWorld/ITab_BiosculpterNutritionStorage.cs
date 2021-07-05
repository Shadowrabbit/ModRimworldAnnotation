using System;

namespace RimWorld
{
	// Token: 0x02001108 RID: 4360
	public class ITab_BiosculpterNutritionStorage : ITab_Storage
	{
		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x060068D2 RID: 26834 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool IsPrioritySettingVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x0023649E File Offset: 0x0023469E
		public ITab_BiosculpterNutritionStorage()
		{
			this.labelKey = "Nutrition";
		}
	}
}
