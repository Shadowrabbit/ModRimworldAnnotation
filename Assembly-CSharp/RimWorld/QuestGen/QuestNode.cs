using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FDD RID: 8157
	public abstract class QuestNode
	{
		// Token: 0x0600AD09 RID: 44297 RVA: 0x003263B8 File Offset: 0x003245B8
		public QuestNode()
		{
			this.myTypeShort = base.GetType().Name;
			if (this.myTypeShort.StartsWith("QuestNode_"))
			{
				this.myTypeShort = this.myTypeShort.Substring("QuestNode_".Length);
			}
		}

		// Token: 0x0600AD0A RID: 44298 RVA: 0x0032640C File Offset: 0x0032460C
		public float SelectionWeight(Slate slate)
		{
			float? value = this.selectionWeight.GetValue(slate);
			if (value == null)
			{
				return 1f;
			}
			return value.GetValueOrDefault();
		}

		// Token: 0x0600AD0B RID: 44299 RVA: 0x0032643C File Offset: 0x0032463C
		public void Run()
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start(this.ToString());
			}
			try
			{
				this.RunInt();
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nSlate vars:\n",
					QuestGen.slate.ToString()
				}), false);
			}
			if (DeepProfiler.enabled)
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600AD0C RID: 44300 RVA: 0x003264D0 File Offset: 0x003246D0
		public bool TestRun(Slate slate)
		{
			bool result;
			try
			{
				Action<QuestNode, Slate> action;
				if (slate.TryGet<Action<QuestNode, Slate>>("testRunCallback", out action, false) && action != null)
				{
					action(this, slate);
				}
				result = this.TestRunInt(slate);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nSlate vars:\n",
					slate.ToString()
				}), false);
				result = false;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x0600AD0D RID: 44301
		protected abstract void RunInt();

		// Token: 0x0600AD0E RID: 44302
		protected abstract bool TestRunInt(Slate slate);

		// Token: 0x0600AD0F RID: 44303 RVA: 0x00070C94 File Offset: 0x0006EE94
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x04007692 RID: 30354
		[Unsaved(false)]
		[TranslationHandle]
		public string myTypeShort;

		// Token: 0x04007693 RID: 30355
		public SlateRef<float?> selectionWeight;
	}
}
