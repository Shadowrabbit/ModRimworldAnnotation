using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001934 RID: 6452
	public struct AlertReport
	{
		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x06008F03 RID: 36611 RVA: 0x00293238 File Offset: 0x00291438
		public bool AnyCulpritValid
		{
			get
			{
				if (!this.culpritsThings.NullOrEmpty<Thing>() || !this.culpritsPawns.NullOrEmpty<Pawn>() || !this.culpritsCaravans.NullOrEmpty<Caravan>())
				{
					return true;
				}
				if (this.culpritTarget != null && this.culpritTarget.Value.IsValid)
				{
					return true;
				}
				if (this.culpritsTargets != null)
				{
					for (int i = 0; i < this.culpritsTargets.Count; i++)
					{
						if (this.culpritsTargets[i].IsValid)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06008F04 RID: 36612 RVA: 0x0005FBA5 File Offset: 0x0005DDA5
		public IEnumerable<GlobalTargetInfo> AllCulprits
		{
			get
			{
				if (this.culpritsThings != null)
				{
					int num;
					for (int i = 0; i < this.culpritsThings.Count; i = num + 1)
					{
						yield return this.culpritsThings[i];
						num = i;
					}
				}
				if (this.culpritsPawns != null)
				{
					int num;
					for (int i = 0; i < this.culpritsPawns.Count; i = num + 1)
					{
						yield return this.culpritsPawns[i];
						num = i;
					}
				}
				if (this.culpritsCaravans != null)
				{
					int num;
					for (int i = 0; i < this.culpritsCaravans.Count; i = num + 1)
					{
						yield return this.culpritsCaravans[i];
						num = i;
					}
				}
				if (this.culpritTarget != null)
				{
					yield return this.culpritTarget.Value;
				}
				if (this.culpritsTargets != null)
				{
					int num;
					for (int i = 0; i < this.culpritsTargets.Count; i = num + 1)
					{
						yield return this.culpritsTargets[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x06008F05 RID: 36613 RVA: 0x002932CC File Offset: 0x002914CC
		public static AlertReport CulpritIs(GlobalTargetInfo culp)
		{
			AlertReport result = default(AlertReport);
			result.active = culp.IsValid;
			if (culp.IsValid)
			{
				result.culpritTarget = new GlobalTargetInfo?(culp);
			}
			return result;
		}

		// Token: 0x06008F06 RID: 36614 RVA: 0x00293308 File Offset: 0x00291508
		public static AlertReport CulpritsAre(List<Thing> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsThings = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06008F07 RID: 36615 RVA: 0x00293334 File Offset: 0x00291534
		public static AlertReport CulpritsAre(List<Pawn> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsPawns = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06008F08 RID: 36616 RVA: 0x00293360 File Offset: 0x00291560
		public static AlertReport CulpritsAre(List<Caravan> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsCaravans = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06008F09 RID: 36617 RVA: 0x0029338C File Offset: 0x0029158C
		public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsTargets = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06008F0A RID: 36618 RVA: 0x002933B8 File Offset: 0x002915B8
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x06008F0B RID: 36619 RVA: 0x0005FBBA File Offset: 0x0005DDBA
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06008F0C RID: 36620 RVA: 0x0005FBC7 File Offset: 0x0005DDC7
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x06008F0D RID: 36621 RVA: 0x0005FBD4 File Offset: 0x0005DDD4
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06008F0E RID: 36622 RVA: 0x002933D8 File Offset: 0x002915D8
		public static AlertReport Active
		{
			get
			{
				return new AlertReport
				{
					active = true
				};
			}
		}

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x06008F0F RID: 36623 RVA: 0x002933F8 File Offset: 0x002915F8
		public static AlertReport Inactive
		{
			get
			{
				return new AlertReport
				{
					active = false
				};
			}
		}

		// Token: 0x04005B40 RID: 23360
		public bool active;

		// Token: 0x04005B41 RID: 23361
		public List<Thing> culpritsThings;

		// Token: 0x04005B42 RID: 23362
		public List<Pawn> culpritsPawns;

		// Token: 0x04005B43 RID: 23363
		public List<Caravan> culpritsCaravans;

		// Token: 0x04005B44 RID: 23364
		public List<GlobalTargetInfo> culpritsTargets;

		// Token: 0x04005B45 RID: 23365
		public GlobalTargetInfo? culpritTarget;
	}
}
