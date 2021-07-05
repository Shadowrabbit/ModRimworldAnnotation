using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001508 RID: 5384
	public static class ApparelUtility
	{
		// Token: 0x0600741C RID: 29724 RVA: 0x002360C8 File Offset: 0x002342C8
		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			for (int i = 0; i < A.apparel.layers.Count; i++)
			{
				for (int j = 0; j < B.apparel.layers.Count; j++)
				{
					if (A.apparel.layers[i] == B.apparel.layers[j])
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			List<BodyPartGroupDef> bodyPartGroups = A.apparel.bodyPartGroups;
			List<BodyPartGroupDef> bodyPartGroups2 = B.apparel.bodyPartGroups;
			BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
			BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
			for (int k = 0; k < bodyPartGroups.Count; k++)
			{
				if (interferingBodyPartGroups2.Contains(bodyPartGroups[k]))
				{
					return false;
				}
			}
			for (int l = 0; l < bodyPartGroups2.Count; l++)
			{
				if (interferingBodyPartGroups.Contains(bodyPartGroups2[l]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600741D RID: 29725 RVA: 0x002361CC File Offset: 0x002343CC
		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<ApparelUtility.LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayerDef layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		// Token: 0x0600741E RID: 29726 RVA: 0x00236230 File Offset: 0x00234430
		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			ApparelUtility.<>c__DisplayClass3_0 CS$<>8__locals1 = new ApparelUtility.<>c__DisplayClass3_0();
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			bool flag = false;
			for (int j = 0; j < hediffs.Count; j++)
			{
				if (hediffs[j] is Hediff_MissingPart)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			CS$<>8__locals1.groups = apparel.apparel.bodyPartGroups;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.groups.Count; i = i2 + 1)
			{
				if (notMissingParts.Any((BodyPartRecord x) => x.IsInGroup(CS$<>8__locals1.groups[i])))
				{
					return true;
				}
				i2 = i;
			}
			return false;
		}

		// Token: 0x02001509 RID: 5385
		public struct LayerGroupPair : IEquatable<ApparelUtility.LayerGroupPair>
		{
			// Token: 0x0600741F RID: 29727 RVA: 0x0004E44F File Offset: 0x0004C64F
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x06007420 RID: 29728 RVA: 0x0004E45F File Offset: 0x0004C65F
			public override bool Equals(object rhs)
			{
				return rhs is ApparelUtility.LayerGroupPair && this.Equals((ApparelUtility.LayerGroupPair)rhs);
			}

			// Token: 0x06007421 RID: 29729 RVA: 0x0004E477 File Offset: 0x0004C677
			public bool Equals(ApparelUtility.LayerGroupPair other)
			{
				return other.layer == this.layer && other.group == this.group;
			}

			// Token: 0x06007422 RID: 29730 RVA: 0x0004E497 File Offset: 0x0004C697
			public override int GetHashCode()
			{
				return (17 * 23 + this.layer.GetHashCode()) * 23 + this.group.GetHashCode();
			}

			// Token: 0x04004C9F RID: 19615
			private readonly ApparelLayerDef layer;

			// Token: 0x04004CA0 RID: 19616
			private readonly BodyPartGroupDef group;
		}
	}
}
