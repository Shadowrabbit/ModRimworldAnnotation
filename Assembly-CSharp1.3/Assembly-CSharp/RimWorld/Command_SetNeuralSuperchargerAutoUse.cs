using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F1 RID: 2545
	[StaticConstructorOnStartup]
	public class Command_SetNeuralSuperchargerAutoUse : Command
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003EBE RID: 16062 RVA: 0x00157172 File Offset: 0x00155372
		private static Texture2D AutoUseForEveryone
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone == null)
				{
					Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_EveryoneAutoUse", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x00157196 File Offset: 0x00155396
		private static Texture2D AutoUseWithDesire
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire == null)
				{
					Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AutoUseWithDesire", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06003EC0 RID: 16064 RVA: 0x001571BA File Offset: 0x001553BA
		private static Texture2D NoAutoUseTex
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.noAutoUseTex == null)
				{
					Command_SetNeuralSuperchargerAutoUse.noAutoUseTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_NoAutoUse", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.noAutoUseTex;
			}
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x001571E0 File Offset: 0x001553E0
		public Command_SetNeuralSuperchargerAutoUse(CompNeuralSupercharger comp)
		{
			this.comp = comp;
			switch (comp.autoUseMode)
			{
			case CompNeuralSupercharger.AutoUseMode.NoAutoUse:
				this.defaultLabel = "CommandNeuralSuperchargerNoAutoUse".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerNoAutoUseDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.NoAutoUseTex;
				return;
			case CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire:
				this.defaultLabel = "CommandNeuralSuperchargerAutoUseWithDesire".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerAutoUseWithDesireDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.AutoUseWithDesire;
				return;
			case CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone:
				this.defaultLabel = "CommandNeuralSuperchargerAutoForEveryone".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerAutoForEveryoneDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.AutoUseForEveryone;
				return;
			default:
				Log.Error(string.Format("Unknown auto use mode: {0}", comp.autoUseMode));
				return;
			}
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x001572D4 File Offset: 0x001554D4
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerNoAutoUse".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.NoAutoUse;
			}, Command_SetNeuralSuperchargerAutoUse.NoAutoUseTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerAutoUseWithDesire".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;
			}, Command_SetNeuralSuperchargerAutoUse.AutoUseWithDesire, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerAutoForEveryone".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone;
			}, Command_SetNeuralSuperchargerAutoUse.AutoUseForEveryone, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0400218C RID: 8588
		private static Texture2D autoUseForEveryone;

		// Token: 0x0400218D RID: 8589
		private static Texture2D autoUseWithDesire;

		// Token: 0x0400218E RID: 8590
		private static Texture2D noAutoUseTex;

		// Token: 0x0400218F RID: 8591
		private readonly CompNeuralSupercharger comp;
	}
}
