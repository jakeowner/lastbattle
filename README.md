# lastbattle
client:
unity: 5.6.3f1

配置连接服务器地址(此处需要改动为服务器所在IP/PORT)
JxBlGame.cs 
	|-->public string LoginServerAdress = "192.168.1.54";
	|-->public int LoginServerPort = 49996;
指定了服务器列表里选择的服务器(此处不需要改动)
LoginCtrl.cs
	|-->NetworkManager.Instance.Init(JxBlGame.Instance.LoginServerAdress, 49996, NetworkManager.ServerType.LoginServer);

server:
vs2010 
编译Release x64版本