using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200052C RID: 1324
	public abstract class ThingComp
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x0001D5E0 File Offset: 0x0001B7E0
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0001D5ED File Offset: 0x0001B7ED
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostExposeData()
		{
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostPostMake()
		{
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompTick()
		{
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompTickRare()
		{
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompTickLong()
		{
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0001C850 File Offset: 0x0001AA50
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDraw()
		{
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0001037D File Offset: 0x0000E57D
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x0001D5F6 File Offset: 0x0001B7F6
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x0001D5FF File Offset: 0x0001B7FF
		public virtual IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0001D608 File Offset: 0x0001B808
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0001D611 File Offset: 0x0001B811
		public virtual IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			yield break;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawGUIOverlay()
		{
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x00107D38 File Offset: 0x00105F38
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.Position : IntVec3.Invalid,
				")"
			});
		}

		// Token: 0x04001701 RID: 5889
		public ThingWithComps parent;

		// Token: 0x04001702 RID: 5890
		public CompProperties props;
	}
}
