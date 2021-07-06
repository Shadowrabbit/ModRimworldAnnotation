using System;

namespace Verse
{
	// Token: 0x0200016D RID: 365
	public abstract class IngredientValueGetter
	{
		// Token: 0x06000944 RID: 2372
		public abstract float ValuePerUnitOf(ThingDef t);

		// Token: 0x06000945 RID: 2373
		public abstract string BillRequirementsDescription(RecipeDef r, IngredientCount ing);

		// Token: 0x06000946 RID: 2374 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string ExtraDescriptionLine(RecipeDef r)
		{
			return null;
		}
	}
}
