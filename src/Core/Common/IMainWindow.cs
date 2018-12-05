/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 11/17/2016
 * 时间: 13:11
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.ComponentModel;

namespace Core
{
	/// <summary>
	/// Description of IMainWindow.
	/// </summary>
	public interface IMainWindow 
	{
		event EventHandler<CancelEventArgs> OnDocumentClosing;		
        
        void LoadAvalonConfig(string filename);
		
		void SaveAvalonConfig(string filename);
			
		void DocumentDock(string documentTitle, object documentContent, 
		                  bool wpfOrWinform = true, bool sharelayoutDocumentPane = false);
		
		void BottomToolsWindowDock(string toolsTitle, object toolsWindowContent,
		                           bool wpfOrWinform = true, bool sharelayoutToolsPane = false);
		
		void LeftToolsWindowDock(string toolsTitle, object toolsWindowContent, 
		                         bool wpfOrWinform = true, bool sharelayoutToolsPane = false);

	    object GetActiveContent();

	}
}
