using System;

namespace Verse
{
	// Token: 0x020000F4 RID: 244
	public abstract class IngredientValueGetter
	{
		// Token: 0x06000697 RID: 1687
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x06000698 RID: 1688
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06000699 RID: 1689 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
