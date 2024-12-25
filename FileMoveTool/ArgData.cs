namespace FileMoveTool {
	internal class ArgData {
		internal string? fileFilter = null;
		internal string? sourceDirectory= null;
		internal string? destDirectory= null;

		/// <summary>
		/// 参数: 
		/// 无需用户确认，直接执行操作
		/// </summary>
		internal bool directRun = false;
		internal List<string> ignoreDirs = [];
		internal void AddIgnoreDir(string path) {
			ignoreDirs.Add(Path.GetFullPath(path));
		}
		/// <summary>
		/// 检查参数是否存在null
		/// </summary>
		/// <returns></returns>
		internal bool NotNullCheck() {
			if(fileFilter==null)return false;
			else if(sourceDirectory==null) return false;
			else if(destDirectory==null) return false;
			else
				return true;
		}
	}
}
