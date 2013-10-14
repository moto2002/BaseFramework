using UnityEngine;
using System.Collections.Generic;

namespace BaseFramework.AI
{
	public class DebugLogTask : Task
	{
		public int m_iFramesToTick;
		public string m_pxMessage;
		
		public DebugLogTask( Node pxNode ) : base( pxNode ) {  }
		
		protected override void InitialiseTask()
		{
			m_iFramesToTick = Random.Range( 0, 60 );
			m_iTickCount = 0;
			m_pxMessage = "Task with tick count: " + m_iFramesToTick;
		}
		
		protected override void DestroyTask()
		{
		}
		
		protected override void UpdateTask( Dictionary<string, object> pxActorView )
		{
			m_iTickCount++;
			
			if ( m_iTickCount > m_iFramesToTick - 1 )
			{
				m_eStatus = TaskState.eTaskSuccess;
				Debug.Log( m_pxMessage + " completed!" );
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