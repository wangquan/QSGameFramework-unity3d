package 
{
	import flash.display.StageDisplayState;
	import flash.net.SharedObject;
	import com.unity.*;
	
	/**
	 * Flash对接接口
	 * @author wq
	 */
	public class CallFlash
	{
		private static var _LocalData:SharedObject;//本地保存数据
		
		//4399
		public static var serviceHold:* = null;
		public static function SetHold(hold:*):void
		{
			serviceHold = hold;
		}
		
		//3366
		public static var open3366Service:* =null;   
		public static function SetService(service:*):void 
		{  
			open3366Service = service;   
		}
		
		//提交积分
		public static function SubmitScore(score:int):void
		{
			if (serviceHold != null) serviceHold.showRefer(score);
			if (serviceHold != null) serviceHold.submitScore(score);
			if (open3366Service != null) open3366Service.submitScore(score);
		}
		
		//更新分数
		public static function UpdateScore(score:int):void
		{
			if (serviceHold != null) serviceHold.changeScore(score);
		}
		
		//推送游戏
		public static function PushGame():void
		{
			if (serviceHold != null) serviceHold.showGameList();
			if (open3366Service != null) open3366Service.recomGame();
		}
		
		//获取本地保存数据
		public static function GetData():String
		{
			if (_LocalData == null) _LocalData = SharedObject.getLocal("LaJiaoRenGame");
			return _LocalData.data.data == null ? "" : _LocalData.data.data;
		}
		
		//保存到本地数据
		public static function SaveData(data:String):void
		{
			if (_LocalData == null) _LocalData = SharedObject.getLocal("LaJiaoRenGame");
			_LocalData.data.data = data;
			_LocalData.flush();
		}
		
		//全屏
		public static function FullScreen():void
		{
			if (UnityNative.stage.displayState == StageDisplayState.NORMAL)
			{
				UnityNative.stage.displayState = StageDisplayState.FULL_SCREEN_INTERACTIVE;
			}else
			{
				UnityNative.stage.displayState = StageDisplayState.NORMAL;
			}
		}
	}
	
}