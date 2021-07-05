using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200068B RID: 1675
	public struct TriggerSignal
	{
		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002F15 RID: 12053 RVA: 0x0011810D File Offset: 0x0011630D
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x0011811C File Offset: 0x0011631C
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

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002F17 RID: 12055 RVA: 0x00118177 File Offset: 0x00116377
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x00118180 File Offset: 0x00116380
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x001181A0 File Offset: 0x001163A0
		public static TriggerSignal ForSignal(Signal signal)
		{
			return new TriggerSignal(TriggerSignalType.Signal)
			{
				signal = signal
			};
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x001181C0 File Offset: 0x001163C0
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

		// Token: 0x04001CCB RID: 7371
		public TriggerSignalType type;

		// Token: 0x04001CCC RID: 7372
		public string memo;

		// Token: 0x04001CCD RID: 7373
		public Signal signal;

		// Token: 0x04001CCE RID: 7374
		public Thing thing;

		// Token: 0x04001CCF RID: 7375
		public DamageInfo dinfo;

		// Token: 0x04001CD0 RID: 7376
		public PawnLostCondition condition;

		// Token: 0x04001CD1 RID: 7377
		public Faction faction;

		// Token: 0x04001CD2 RID: 7378
		public FactionRelationKind? previousRelationKind;

		// Token: 0x04001CD3 RID: 7379
		public ClamorDef clamorType;
	}
}
