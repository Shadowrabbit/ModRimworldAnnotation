using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014EF RID: 5359
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06007F99 RID: 32665 RVA: 0x002D1277 File Offset: 0x002CF477
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x06007F9A RID: 32666 RVA: 0x002D127F File Offset: 0x002CF47F
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x06007F9B RID: 32667 RVA: 0x002D1287 File Offset: 0x002CF487
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06007F9C RID: 32668 RVA: 0x002D1294 File Offset: 0x002CF494
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06007F9D RID: 32669 RVA: 0x002D12A1 File Offset: 0x002CF4A1
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06007F9E RID: 32670 RVA: 0x002D12A9 File Offset: 0x002CF4A9
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x06007F9F RID: 32671 RVA: 0x002D12B1 File Offset: 0x002CF4B1
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x06007FA0 RID: 32672 RVA: 0x002D12C1 File Offset: 0x002CF4C1
		public List<StatModifier> StatBases
		{
			get
			{
				if (!(this.defInt is BuildableDef))
				{
					return this.AbilityDef.statBases;
				}
				return this.BuildableDef.statBases;
			}
		}

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06007FA1 RID: 32673 RVA: 0x002D12E7 File Offset: 0x002CF4E7
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x06007FA2 RID: 32674 RVA: 0x002D12EF File Offset: 0x002CF4EF
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06007FA3 RID: 32675 RVA: 0x002D12F7 File Offset: 0x002CF4F7
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06007FA4 RID: 32676 RVA: 0x002D1302 File Offset: 0x002CF502
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x06007FA5 RID: 32677 RVA: 0x002D1310 File Offset: 0x002CF510
		public static StatRequest For(Thing thing)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x06007FA6 RID: 32678 RVA: 0x002D136C File Offset: 0x002CF56C
		public static StatRequest For(Thing thing, Pawn pawn)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			result.pawn = pawn;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x06007FA7 RID: 32679 RVA: 0x002D13D0 File Offset: 0x002CF5D0
		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				defInt = def,
				stuffDefInt = stuffDef,
				qualityCategoryInt = quality
			};
		}

		// Token: 0x06007FA8 RID: 32680 RVA: 0x002D141C File Offset: 0x002CF61C
		public static StatRequest For(AbilityDef def, Pawn forPawn = null)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = def,
				qualityCategoryInt = QualityCategory.Normal,
				pawn = forPawn
			};
		}

		// Token: 0x06007FA9 RID: 32681 RVA: 0x002D1470 File Offset: 0x002CF670
		public static StatRequest For(RoyalTitleDef def, Faction faction, Pawn pawn = null)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = null,
				faction = faction,
				qualityCategoryInt = QualityCategory.Normal,
				pawn = pawn
			};
		}

		// Token: 0x06007FAA RID: 32682 RVA: 0x002D14CC File Offset: 0x002CF6CC
		public static StatRequest ForEmpty()
		{
			return new StatRequest
			{
				thingInt = null,
				defInt = null,
				stuffDefInt = null,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x06007FAB RID: 32683 RVA: 0x002D1504 File Offset: 0x002CF704
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return "(" + this.Thing + ")";
			}
			return string.Concat(new object[]
			{
				"(",
				this.Thing,
				", ",
				(this.StuffDef != null) ? this.StuffDef.defName : "null",
				")"
			});
		}

		// Token: 0x06007FAC RID: 32684 RVA: 0x002D1578 File Offset: 0x002CF778
		public override int GetHashCode()
		{
			int num = 0;
			num = Gen.HashCombineInt(num, (int)this.defInt.shortHash);
			if (this.thingInt != null)
			{
				num = Gen.HashCombineInt(num, this.thingInt.thingIDNumber);
			}
			if (this.stuffDefInt != null)
			{
				num = Gen.HashCombineInt(num, (int)this.stuffDefInt.shortHash);
			}
			num = Gen.HashCombineInt(num, this.qualityCategoryInt.GetHashCode());
			if (this.faction != null)
			{
				num = Gen.HashCombineInt(num, this.faction.GetHashCode());
			}
			if (this.pawn != null)
			{
				num = Gen.HashCombineInt(num, this.pawn.GetHashCode());
			}
			return num;
		}

		// Token: 0x06007FAD RID: 32685 RVA: 0x002D161C File Offset: 0x002CF81C
		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x06007FAE RID: 32686 RVA: 0x002D1668 File Offset: 0x002CF868
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt && other.qualityCategoryInt == this.qualityCategoryInt && other.faction == this.faction && other.pawn == this.pawn;
		}

		// Token: 0x06007FAF RID: 32687 RVA: 0x002D16CB File Offset: 0x002CF8CB
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x002D16D5 File Offset: 0x002CF8D5
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04004FA0 RID: 20384
		private Thing thingInt;

		// Token: 0x04004FA1 RID: 20385
		private Def defInt;

		// Token: 0x04004FA2 RID: 20386
		private ThingDef stuffDefInt;

		// Token: 0x04004FA3 RID: 20387
		private QualityCategory qualityCategoryInt;

		// Token: 0x04004FA4 RID: 20388
		private Faction faction;

		// Token: 0x04004FA5 RID: 20389
		private Pawn pawn;
	}
}
