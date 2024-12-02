## 简介
- 该软件可以将一个目录下所有指定筛选的文件同步至另一个文件夹。
- 该软件必需要[安装.net 8.0 runtime框架](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-8.0.10-windows-x64-installer)才能运行。
- 支持windows和linux操作系统。

## 帮助文档
``` bash
命令帮助:
使用:
fmvt <options> [file filter] [source directory] [dest directory]
将源目录下(包括其子目录)的所有符合条件的文件同步至目标目录，将删除或覆盖目标目录内所有符合条件的文件
options:
'-i [directory]' or '--ignore [directory]': 忽略指定目录，该参数可以通过重复使用来实现忽略多个目录。目前只支持绝对路径
'-y': 在执行操作前无需用户确认

示例:
# 将'~/Test-s'目录下的所有.txt文件同步至'~/Text-t'文件夹，'~/Text-t'文件夹内的所有.txt文件在同步之前将被删除。
fmvt *.txt ~/Test-s ~/Test-t
```

## 注意事项
- 如果不清楚软件各个功能执行的操作，请先自行测试需要使用的功能，请勿盲目使用。
- 本软件的开发者对使用者因任何原因在使用本软件时对自己或他人造成的任何形式的损失和伤害不承担责任。
