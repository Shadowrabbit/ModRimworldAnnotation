using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D4 RID: 5076
	public abstract class Lesson : IExposable
	{
		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x06007B63 RID: 31587 RVA: 0x002B85C8 File Offset: 0x002B67C8
		protected float AgeSeconds
		{
			get
			{
				if (this.startRealTime < 0f)
				{
					this.startRealTime = Time.realtimeSinceStartup;
				}
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x06007B64 RID: 31588 RVA: 0x00002688 File Offset: 0x00000888
		public virtual ConceptDef Concept
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x06007B65 RID: 31589 RVA: 0x00002688 File Offset: 0x00000888
		public virtual InstructionDef Instruction
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06007B66 RID: 31590 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float MessagesYOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06007B67 RID: 31591 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExposeData()
		{
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x002B85EE File Offset: 0x002B67EE
		public virtual void OnActivated()
		{
			this.startRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDeactivated()
		{
		}

		// Token: 0x06007B6A RID: 31594
		public abstract void LessonOnGUI();

		// Token: 0x06007B6B RID: 31595 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void LessonUpdate()
		{
		}

		// Token: 0x06007B6C RID: 31596 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
		}

		// Token: 0x06007B6D RID: 31597 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Event(EventPack ep)
		{
		}

		// Token: 0x06007B6E RID: 31598 RVA: 0x0015E338 File Offset: 0x0015C538
		public virtual AcceptanceReport AllowAction(EventPack ep)
		{
			return true;
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x06007B6F RID: 31599 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string DefaultRejectInputMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04004446 RID: 17478
		public float startRealTime = -999f;

		// Token: 0x04004447 RID: 17479
		public const float KnowledgeForAutoVanish = 0.2f;
	}
}
