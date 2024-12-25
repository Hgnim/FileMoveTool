namespace FileMoveTool {
	internal class Program {
		public const string version = "1.0.0.20241202";

		static void Main(string[] args) {
			/*
start:;
			Console.WriteLine("Hello, World!");
			string str= Console.ReadLine();
			string[] output = Directory.GetFiles("D:/Test", str, SearchOption.AllDirectories);
			foreach (string file in output) {
				Console.WriteLine(file);
			}
			goto start;
			*/
			try {
				ArgData ad = new();
				if (!(args.Length > 0)) {
					HelpOutput();
					return;
				}
				for (int i=0;i<args.Length;i++) {
					switch (args[i]) {
						case "-y":
							ad.directRun = true;
							break;
						case "-i":
						case "--ignore":
							ad.AddIgnoreDir(args[++i]);
							break;
						case "-h":
						case "--help":
							HelpOutput();
							return;
						case "-v":
						case "--version":
							VersionOutput();
							return;
						default:
							if (ad.fileFilter == null)
								ad.fileFilter = args[i];
							else if (ad.sourceDirectory == null)
								ad.sourceDirectory = args[i];
#pragma warning disable IDE0074
							else if (ad.destDirectory == null)
								ad.destDirectory = args[i];
#pragma warning restore IDE0074
							break;
					}
				}
				if (!ad.NotNullCheck()) {
					Console.WriteLine("存在空参数！");
					HelpOutput();
					return;
				}
				string[] allSourceFiles ;
				string[] allRemoveFiles ;
				{
					///执行忽略文件夹过滤
					string[] RunIgnoreDir(string[] dirs) {
						List<string> output = [];
						foreach (string dir in dirs) {
							bool isIgnore = false;
							foreach(string iDir in ad.ignoreDirs) {
								if (dir.Contains(iDir, StringComparison.CurrentCulture)) {//判断忽略的目录字符是否存在于指定目录
									isIgnore = true;
									break;
								}
							}
							if(!isIgnore) 
								output.Add(dir);
						}
						return [.. output];
					}
					allSourceFiles = RunIgnoreDir(Directory.GetFiles(ad.sourceDirectory!, ad.fileFilter!, SearchOption.AllDirectories));
					allRemoveFiles = RunIgnoreDir(Directory.GetFiles(ad.destDirectory!, ad.fileFilter!, SearchOption.AllDirectories));
				}
				Console.WriteLine("已找到的源文件:");
				foreach (string file in allSourceFiles) {
					Console.WriteLine(file);
				}
				Console.WriteLine($"一共{allSourceFiles.Length}个。");

				Console.WriteLine("将被移除或替换的文件:");
				foreach (string file in allRemoveFiles) {
					Console.WriteLine(file);
				}
				Console.WriteLine($"一共{allRemoveFiles.Length}个。");
				Console.WriteLine($"准备将\"{ad.sourceDirectory}\"内的\"{ad.fileFilter}\"文件同步至\"{ad.destDirectory}\"。");
				if (!ad.directRun) {
					Console.Write("确定要执行此操作吗('y' or 'n'): ");
					string input = Console.ReadLine()!;
					if (input != "y") {
						Console.WriteLine("操作已被取消");
						return;
					}
				}

				{
					bool haveError = false;
					foreach (string file in allRemoveFiles) {
						try {
							File.Delete(file);
							Console.WriteLine($"\"{file}\" 删除成功");
						} catch { 
							Console.WriteLine($"\"{file}\" 删除失败");
							haveError = true; 
						}
					}
					string GetRelativePath(string sPath) {//获取源文件路径的相对路径
						string output = sPath.Replace(Path.GetFullPath(ad.sourceDirectory!), "");//替换掉源文件路径的源文件夹字符串
						{
reCheck:;
							string getFirst = output[..1];
							if (getFirst == "\\" || getFirst == "/") {
								output = output[1..];//如果发现路径前有斜杠，则去除路径前的斜杠
								goto reCheck;
							}
						}
						return output;
					}
					foreach (string file in allSourceFiles) {
						try {
							string DestPath = Path.Combine(ad.destDirectory!, GetRelativePath(file));
							Directory.CreateDirectory(Path.GetDirectoryName(DestPath)!);
							File.Copy(file, DestPath);
							Console.WriteLine($"\"{file}\" >> \"{DestPath}\" 同步成功");
						} catch { 
							Console.WriteLine($"\"{file}\" 同步失败");
							haveError = true;
						}
					}
					if (haveError) {
						Console.WriteLine("含有未执行成功的操作，请检查执行日志。");
						Console.Write("按任意键退出...");
						Console.ReadKey();
					} else {
						bool manualExit = false ;
						Console.WriteLine("所有操作均已成功执行");
						Task.Run(() => {
							Console.Write("程序将在5秒后自动退出，按任意键直接退出...");
							Console.ReadKey();
							manualExit = true;
						});
						for(int time=0;time<100 && !manualExit;time++) {
							Thread.Sleep(50);
						}
					}
				}
			}
#if DEBUG
			catch (Exception e) {
				Console.WriteLine("错误:");
				Console.WriteLine(e.ToString());
			}
#else
			catch { 
				Console.WriteLine("发生错误！");
				HelpOutput();
			}
#endif
		}


		static void HelpOutput() {
			Console.WriteLine(
@"命令帮助:
使用:
fmvt <options> [file filter] [source directory] [dest directory]
将源目录下(包括其子目录)的所有符合条件的文件同步至目标目录，将删除或覆盖目标目录内所有符合条件的文件
options:
'-i [directory]' or '--ignore [directory]': 忽略指定目录，该参数可以通过重复使用来实现忽略多个目录。目前只支持绝对路径
'-y': 在执行操作前无需用户确认

示例:
# 将'~/Test-s'目录下的所有.txt文件同步至'~/Text-t'文件夹，'~/Text-t'文件夹内的所有.txt文件在同步之前将被删除。
fmvt *.txt ~/Test-s ~/Test-t
"
);
		}
		static void VersionOutput() {
			Console.WriteLine(
@$"文件移动工具(FileMoveTool)
版本: V{version}
Github: https://github.com/Hgnim/FileMoveTool
Copyright (C) 2024 Hgnim, All rights reserved."               
				);
		}
	}
}
