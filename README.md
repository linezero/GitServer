
# GitServer

ASP.NET Core 2.0开发Git HTTP Server

设置

```
  "GitSettings": {
    "BasePath": "D:\\Git",
    "GitPath": "git"
  }
```

需要先安装Git,并确保git 命令可以执行

GitPath 可以是 git 的绝对路径


## 原理

git client => git server => git server

![](git-server-rpc-model.png)