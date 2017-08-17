
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

## 功能

- 创建仓库
- 浏览仓库
- git客户端push pull

## 开发

`git clone https://github.com/linezero/GitServer.git`

使用VS 2017 15.3+ 或者VS Code。


## 原理

git client => GitServer => git server

![](git-server-rpc-model.png)

## License

This project is under the [MIT License](LICENSE).
