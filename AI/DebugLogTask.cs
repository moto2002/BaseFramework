using UnityEngine;
using System.Collections;

namespace BaseFramework.AI
{
	public class DebugLogTask : Task
	{
		public int m_iFramesToTick;
		public string m_pxMessage;
		
		public DebugLogTask( Node pxNode ) : base( pxNode ) {  }
		
		protected override void InitialiseTask ()
		{
			m_iFramesToTick = Random.Range( 0, 60 );
			m_iTickCount = 0;
			m_pxMessage = "Task with tick count: " + m_iFramesToTick;
		}
		
		protected override void DestroyTask ()
		{
			Debug.Log( m_pxMessage + " completed!" );
		}
		
		protected override void UpdateTask (System.Collections.Generic.Dictionary<string, object> pxActorView)
		{
			m_iTickCount++;
			
			if ( m_iTickCount > m_iFramesToTick - 1 )
			{
				m_eStatus = TaskState.eTaskSuccess;
			}
			else
			{
				m_eStatus = TaskState.eTaskRunning;
				
				string pxMessage = m_pxMessage + "["+m_iTickCount+"]";
				Debug.Log( pxMessage );
			}
		}
		
		private int m_iTickCount;
	}
}