using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001529 RID: 5417
	public class RoomRequirement_TerrainWithTags : RoomRequirement
	{
		// Token: 0x060080E5 RID: 32997 RVA: 0x002D9FD0 File Offset: 0x002D81D0
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

		// Token: 0x060080E6 RID: 32998 RVA: 0x002DA08C File Offset: 0x002D828C
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

		// Token: 0x060080E7 RID: 32999 RVA: 0x002DA100 File Offset: 0x002D8300
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

		// Token: 0x060080E8 RID: 33000 RVA: 0x002DA110 File Offset: 0x002D8310
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04005054 RID: 20564
		public List<string> tags;
	}
}
