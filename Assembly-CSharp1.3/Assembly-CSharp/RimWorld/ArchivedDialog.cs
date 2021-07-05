using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF4 RID: 2804
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060041FB RID: 16891 RVA: 0x00002688 File Offset: 0x00000888
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060041FC RID: 16892 RVA: 0x0001A4C7 File Offset: 0x000186C7
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060041FD RID: 16893 RVA: 0x0016195D File Offset: 0x0015FB5D
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x0016196A File Offset: 0x0015FB6A
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x00161972 File Offset: 0x0015FB72
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x000126F5 File Offset: 0x000108F5
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06004201 RID: 16897 RVA: 0x00002688 File Offset: 0x00000888
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x000033AC File Offset: 0x000015AC
		public ArchivedDialog()
		{
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0016197C File Offset: 0x0015FB7C
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextArchivedDialogID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x001619D4 File Offset: 0x0015FBD4
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, false, this.title));
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00161A38 File Offset: 0x0015FC38
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x00161A9E File Offset: 0x0015FC9E
		public string GetUniqueLoadID()
		{
			return "ArchivedDialog_" + this.ID;
		}

		// Token: 0x04002834 RID: 10292
		public int ID;

		// Token: 0x04002835 RID: 10293
		public string text;

		// Token: 0x04002836 RID: 10294
		public string title;

		// Token: 0x04002837 RID: 10295
		public Faction relatedFaction;

		// Token: 0x04002838 RID: 10296
		public int createdTick;
	}
}
