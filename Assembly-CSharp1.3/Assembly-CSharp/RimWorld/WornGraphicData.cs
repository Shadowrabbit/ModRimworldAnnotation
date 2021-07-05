using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A46 RID: 2630
	public class WornGraphicData
	{
		// Token: 0x06003F78 RID: 16248 RVA: 0x00158E04 File Offset: 0x00157004
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

		// Token: 0x06003F79 RID: 16249 RVA: 0x00158EF0 File Offset: 0x001570F0
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

		// Token: 0x040022E2 RID: 8930
		public bool renderUtilityAsPack;

		// Token: 0x040022E3 RID: 8931
		public WornGraphicDirectionData north;

		// Token: 0x040022E4 RID: 8932
		public WornGraphicDirectionData south;

		// Token: 0x040022E5 RID: 8933
		public WornGraphicDirectionData east;

		// Token: 0x040022E6 RID: 8934
		public WornGraphicDirectionData west;

		// Token: 0x040022E7 RID: 8935
		public WornGraphicBodyTypeData male;

		// Token: 0x040022E8 RID: 8936
		public WornGraphicBodyTypeData female;

		// Token: 0x040022E9 RID: 8937
		public WornGraphicBodyTypeData thin;

		// Token: 0x040022EA RID: 8938
		public WornGraphicBodyTypeData hulk;

		// Token: 0x040022EB RID: 8939
		public WornGraphicBodyTypeData fat;
	}
}
