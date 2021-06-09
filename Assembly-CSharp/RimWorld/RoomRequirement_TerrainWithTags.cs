using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB1 RID: 7601
	public class RoomRequirement_TerrainWithTags : RoomRequirement
	{
		// Token: 0x0600A538 RID: 42296 RVA: 0x00300154 File Offset: 0x002FE354
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (IntVec3 c in r.Cells)
			{
				List<string> list = c.GetTerrain(r.Map).tags;
				if (list.NullOrEmpty<string>())
				{
					return false;
				}
				bool flag = false;
				foreach (string item in list)
				{
					if (this.tags.Contains(item))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A539 RID: 42297 RVA: 0x00300210 File Offset: 0x002FE410
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_TerrainWithTags roomRequirement_TerrainWithTags = (RoomRequirement_TerrainWithTags)other;
			foreach (string item in this.tags)
			{
				if (!roomRequirement_TerrainWithTags.tags.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A53A RID: 42298 RVA: 0x0006D7FC File Offset: 0x0006B9FC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (string.IsNullOrEmpty(this.labelKey))
			{
				yield return "does not define a label key";
			}
			if (this.tags.NullOrEmpty<string>())
			{
				yield return "tags are null or empty";
			}
			yield break;
			yield break;
		}

		// Token: 0x04007014 RID: 28692
		public List<string> tags;
	}
}
