using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F68 RID: 3944
	public class WornGraphicData
	{
		// Token: 0x0600569C RID: 22172 RVA: 0x001CAB84 File Offset: 0x001C8D84
		public Vector2 BeltOffsetAt(Rot4 facing, BodyTypeDef bodyType)
		{
			WornGraphicDirectionData wornGraphicDirectionData = default(WornGraphicDirectionData);
			switch (facing.AsInt)
			{
			case 0:
				wornGraphicDirectionData = this.north;
				break;
			case 1:
				wornGraphicDirectionData = this.east;
				break;
			case 2:
				wornGraphicDirectionData = this.south;
				break;
			case 3:
				wornGraphicDirectionData = this.west;
				break;
			}
			Vector2 vector = wornGraphicDirectionData.offset;
			if (bodyType == BodyTypeDefOf.Male)
			{
				vector += wornGraphicDirectionData.male.offset;
			}
			else if (bodyType == BodyTypeDefOf.Female)
			{
				vector += wornGraphicDirectionData.female.offset;
			}
			else if (bodyType == BodyTypeDefOf.Thin)
			{
				vector += wornGraphicDirectionData.thin.offset;
			}
			else if (bodyType == BodyTypeDefOf.Hulk)
			{
				vector += wornGraphicDirectionData.hulk.offset;
			}
			else if (bodyType == BodyTypeDefOf.Fat)
			{
				vector += wornGraphicDirectionData.fat.offset;
			}
			return vector;
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x001CAC70 File Offset: 0x001C8E70
		public Vector2 BeltScaleAt(BodyTypeDef bodyType)
		{
			Vector2 result = Vector2.one;
			if (bodyType == BodyTypeDefOf.Male)
			{
				result = this.male.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Female)
			{
				result = this.female.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Thin)
			{
				result = this.thin.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Hulk)
			{
				result = this.hulk.Scale;
			}
			else if (bodyType == BodyTypeDefOf.Fat)
			{
				result = this.fat.Scale;
			}
			return result;
		}

		// Token: 0x040037DB RID: 14299
		public bool renderUtilityAsPack;

		// Token: 0x040037DC RID: 14300
		public WornGraphicDirectionData north;

		// Token: 0x040037DD RID: 14301
		public WornGraphicDirectionData south;

		// Token: 0x040037DE RID: 14302
		public WornGraphicDirectionData east;

		// Token: 0x040037DF RID: 14303
		public WornGraphicDirectionData west;

		// Token: 0x040037E0 RID: 14304
		public WornGraphicBodyTypeData male;

		// Token: 0x040037E1 RID: 14305
		public WornGraphicBodyTypeData female;

		// Token: 0x040037E2 RID: 14306
		public WornGraphicBodyTypeData thin;

		// Token: 0x040037E3 RID: 14307
		public WornGraphicBodyTypeData hulk;

		// Token: 0x040037E4 RID: 14308
		public WornGraphicBodyTypeData fat;
	}
}
