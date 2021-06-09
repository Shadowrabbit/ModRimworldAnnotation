using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B17 RID: 2839
	public struct TriggerSignal
	{
		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06004247 RID: 16967 RVA: 0x0003162B File Offset: 0x0002F82B
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x00189698 File Offset: 0x00187898
		public TriggerSignal(TriggerSignalType type)
		{
			this.type = type;
			this.memo = null;
			this.signal = default(Signal);
			this.thing = null;
			this.dinfo = default(DamageInfo);
			this.condition = PawnLostCondition.Undefined;
			this.faction = null;
			this.clamorType = null;
			this.previousRelationKind = null;
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06004249 RID: 16969 RVA: 0x00031638 File Offset: 0x0002F838
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x001896F4 File Offset: 0x001878F4
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x00189714 File Offset: 0x00187914
		public static TriggerSignal ForSignal(Signal signal)
		{
			return new TriggerSignal(TriggerSignalType.Signal)
			{
				signal = signal
			};
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x00189734 File Offset: 0x00187934
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			stringBuilder.Append(this.type.ToString());
			if (this.memo != null)
			{
				stringBuilder.Append(", memo=" + this.memo);
			}
			if (this.Pawn != null)
			{
				stringBuilder.Append(", pawn=" + this.Pawn);
			}
			if (this.dinfo.Def != null)
			{
				stringBuilder.Append(", dinfo=" + this.dinfo);
			}
			if (this.condition != PawnLostCondition.Undefined)
			{
				stringBuilder.Append(", condition=" + this.condition);
			}
			if (this.signal.tag != null)
			{
				stringBuilder.Append(", signal=" + this.signal);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04002D7F RID: 11647
		public TriggerSignalType type;

		// Token: 0x04002D80 RID: 11648
		public string memo;

		// Token: 0x04002D81 RID: 11649
		public Signal signal;

		// Token: 0x04002D82 RID: 11650
		public Thing thing;

		// Token: 0x04002D83 RID: 11651
		public DamageInfo dinfo;

		// Token: 0x04002D84 RID: 11652
		public PawnLostCondition condition;

		// Token: 0x04002D85 RID: 11653
		public Faction faction;

		// Token: 0x04002D86 RID: 11654
		public FactionRelationKind? previousRelationKind;

		// Token: 0x04002D87 RID: 11655
		public ClamorDef clamorType;
	}
}
