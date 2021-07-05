using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200157F RID: 5503
	public abstract class SketchResolver
	{
		// Token: 0x0600821C RID: 33308 RVA: 0x002DFF90 File Offset: 0x002DE190
		public void Resolve(ResolveParams parms)
		{
			try
			{
				this.ResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception resolving ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nParms:\n",
					parms.ToString()
				}));
			}
		}

		// Token: 0x0600821D RID: 33309 RVA: 0x002E0004 File Offset: 0x002DE204
		public bool CanResolve(ResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception test running ",
					base.GetType().Name,
					": ",
					ex,
					"\n\nParms:\n",
					parms.ToString()
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x0600821E RID: 33310
		protected abstract void ResolveInt(ResolveParams parms);

		// Token: 0x0600821F RID: 33311
		protected abstract bool CanResolveInt(ResolveParams parms);
	}
}
