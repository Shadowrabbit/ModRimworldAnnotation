using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D54 RID: 7508
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x0600A30E RID: 41742 RVA: 0x0006C46B File Offset: 0x0006A66B
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x0600A30F RID: 41743 RVA: 0x0006C473 File Offset: 0x0006A673
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x0600A310 RID: 41744 RVA: 0x0006C47B File Offset: 0x0006A67B
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x0600A311 RID: 41745 RVA: 0x0006C488 File Offset: 0x0006A688
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		// Token: 0x17001917 RID: 6423
		// (get) Token: 0x0600A312 RID: 41746 RVA: 0x0006C495 File Offset: 0x0006A695
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x0600A313 RID: 41747 RVA: 0x0006C49D File Offset: 0x0006A69D
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x0600A314 RID: 41748 RVA: 0x0006C4A5 File Offset: 0x0006A6A5
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		// Token: 0x1700191A RID: 6426
		// (get) Token: 0x0600A315 RID: 41749 RVA: 0x0006C4B5 File Offset: 0x0006A6B5
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

		// Token: 0x1700191B RID: 6427
		// (get) Token: 0x0600A316 RID: 41750 RVA: 0x0006C4DB File Offset: 0x0006A6DB
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x0600A317 RID: 41751 RVA: 0x0006C4E3 File Offset: 0x0006A6E3
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x0600A318 RID: 41752 RVA: 0x0006C4EB File Offset: 0x0006A6EB
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x0600A319 RID: 41753 RVA: 0x0006C4F6 File Offset: 0x0006A6F6
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x0600A31A RID: 41754 RVA: 0x002F6BB0 File Offset: 0x002F4DB0
		public static StatRequest For(Thing thing)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.", false);
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x0600A31B RID: 41755 RVA: 0x002F6C0C File Offset: 0x002F4E0C
		public static StatRequest For(Thing thing, Pawn pawn)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.", false);
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

		// Token: 0x0600A31C RID: 41756 RVA: 0x002F6C70 File Offset: 0x002F4E70
		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
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

		// Token: 0x0600A31D RID: 41757 RVA: 0x002F6CBC File Offset: 0x002F4EBC
		public static StatRequest For(AbilityDef def)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = def,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x0600A31E RID: 41758 RVA: 0x002F6D08 File Offset: 0x002F4F08
		public static StatRequest For(RoyalTitleDef def, Faction faction)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = null,
				faction = faction,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x0600A31F RID: 41759 RVA: 0x002F6D5C File Offset: 0x002F4F5C
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

		// Token: 0x0600A320 RID: 41760 RVA: 0x002F6D94 File Offset: 0x002F4F94
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

		// Token: 0x0600A321 RID: 41761 RVA: 0x002F6E08 File Offset: 0x002F5008
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
			return num;
		}

		// Token: 0x0600A322 RID: 41762 RVA: 0x002F6E60 File Offset: 0x002F5060
		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x0600A323 RID: 41763 RVA: 0x0006C501 File Offset: 0x0006A701
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x0600A324 RID: 41764 RVA: 0x0006C52F File Offset: 0x0006A72F
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600A325 RID: 41765 RVA: 0x0006C539 File Offset: 0x0006A739
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04006EAC RID: 28332
		private Thing thingInt;

		// Token: 0x04006EAD RID: 28333
		private Def defInt;

		// Token: 0x04006EAE RID: 28334
		private ThingDef stuffDefInt;

		// Token: 0x04006EAF RID: 28335
		private QualityCategory qualityCategoryInt;

		// Token: 0x04006EB0 RID: 28336
		private Faction faction;

		// Token: 0x04006EB1 RID: 28337
		private Pawn pawn;
	}
}
