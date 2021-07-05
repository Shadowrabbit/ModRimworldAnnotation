using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123D RID: 4669
	public struct AlertReport
	{
		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06007015 RID: 28693 RVA: 0x002558A0 File Offset: 0x00253AA0
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

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06007016 RID: 28694 RVA: 0x00255931 File Offset: 0x00253B31
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

		// Token: 0x06007017 RID: 28695 RVA: 0x00255948 File Offset: 0x00253B48
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

		// Token: 0x06007018 RID: 28696 RVA: 0x00255984 File Offset: 0x00253B84
		public static AlertReport CulpritsAre(List<Thing> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsThings = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x06007019 RID: 28697 RVA: 0x002559B0 File Offset: 0x00253BB0
		public static AlertReport CulpritsAre(List<Pawn> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsPawns = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600701A RID: 28698 RVA: 0x002559DC File Offset: 0x00253BDC
		public static AlertReport CulpritsAre(List<Caravan> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsCaravans = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600701B RID: 28699 RVA: 0x00255A08 File Offset: 0x00253C08
		public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsTargets = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600701C RID: 28700 RVA: 0x00255A34 File Offset: 0x00253C34
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x0600701D RID: 28701 RVA: 0x00255A52 File Offset: 0x00253C52
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x0600701E RID: 28702 RVA: 0x00255A5F File Offset: 0x00253C5F
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x0600701F RID: 28703 RVA: 0x00255A6C File Offset: 0x00253C6C
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06007020 RID: 28704 RVA: 0x00255A74 File Offset: 0x00253C74
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

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06007021 RID: 28705 RVA: 0x00255A94 File Offset: 0x00253C94
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

		// Token: 0x04003DE9 RID: 15849
		public bool active;

		// Token: 0x04003DEA RID: 15850
		public List<Thing> culpritsThings;

		// Token: 0x04003DEB RID: 15851
		public List<Pawn> culpritsPawns;

		// Token: 0x04003DEC RID: 15852
		public List<Caravan> culpritsCaravans;

		// Token: 0x04003DED RID: 15853
		public List<GlobalTargetInfo> culpritsTargets;

		// Token: 0x04003DEE RID: 15854
		public GlobalTargetInfo? culpritTarget;
	}
}
