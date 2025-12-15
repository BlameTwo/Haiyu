# Haiyu Rpc

此协议为Haiyu客户端定义的远程调用协议，当前版本为实验性功能，不保证后续追加功能。

通信协议：WebSocket

通信格式：Json

远程调用：不允许，只允许本地回环地址

#### 免责声明

此协议不得使用在**爬虫**、**逆向分析**、**损坏玩家账号**、**本地客户端篡改**等行为，如若出现**账号封禁**、**计算机软硬件损坏**与Haiyu无关。



## 协议定义

地址定义：ws://localhost:9084/rpc

版本定义：App 版本/Rpc 版本

请求定义

| 字段        | 类型     | 备注         |
| --------- | ------ | ---------- |
| method    | string | 调用方法名称     |
| requestId | long   | 请求ID（随机生成） |
| params    | Array  | 请求参数       |

示例

```json
{
    "method": "methodName",
    "requestId": 2
    "params": [
     {
          "key":"paramKey",
          "value":"paramValue"
     }
    ]
}
```

返回定义

| 字段        | 类型     | 备注         |
| --------- |:------:| ---------- |
| requestId | long   | 对应请求ID     |
| message   | string | 数据本体/或报错信息 |
| success   | bool   | 请求是否接受     |

```json
{
    "requestId": 4313221365652805,
    "message": "返回信息或报错信息",
    "success": true, //是否错误
}
```

## 客户端信息

说明：返回当前Haiyu的客户端版本以及Rpc版本和其他信息

鉴权：无鉴权信息

版本支持：1.2.16/1.0

| 字段     | 值           |
| ------ | ----------- |
| method | app_version |

调用示例

```json
{
    "method": "app_version",
    "requestId": 4991070645471795,
    "params": []
}
```

返回值示例

```json
{
    "requestId": 4991070645471795,
    "message": "{\"appVersion\":\"1.2.16\",\"rpcVersion\":\"1.0.0\",\"webVersion\":\"142.0.3595.94\",\"frameworkVersion\":\".NET 10.0.1\",\"sdkVersion\":\"1.8.1106-stable\"}",
    "success": true
}
```

## 客户端状态

说明：返回当前Haiyu是否支持请求

鉴权：无鉴权信息

版本支持：1.2.16/1.0

| 字段     | 值        |
| ------ | -------- |
| method | app_ping |

调用示例

```json
{
    "method": "app_ping",
    "requestId": 5727519153795929,
    "params": []
}
```

返回示例

```json
{
    "requestId": 5727519153795929,
    "message": "0",
    "success": true
}
```

## 接口定义

说明：返回当前Haiyu的所有支持接口方法名称

鉴权：无鉴权信息

版本支持：1.2.16/1.0

| 字段     | 值范围         |
| ------ | ----------- |
| method | app_methods |

请求示例

```json
{
    "method": "app_methods",
    "requestId": 8418971221358410, //生成ID
    "params": []
}
```

返回示例

```json
{

    "requestId": 8418971221358410,
    "message": "[\"app_ping\",\"app_version\",\"app_methods\",\"cloud_getCloudUsers\",\"cloud_getCloudRecordKey\",\"cloud_saveAsCloudRecordResource\"]",

    "success": true

}
```

## 云工具

### 用户列表

说明：返回当前Haiyu的云工具本地存储账号列表

鉴权：需要密钥

版本支持：1.2.16/1.0

| 字段     | 值范围           |
| ------ | ------------- |
| method | getCloudUsers |

| 参数字段  | 值   |
| ----- | --- |
| token | 密钥  |

```json
{
    "method": "getCloudUsers",
    "requestId": 14215,
    "params": [
    {
          "key":"token",
          "value":"Haiyu客户端定义token"
    }
} 
```

```json
{
    "requestId": 14215,
    "message": "[\"U518734907A\"]",
    "success": true
}
```



### 抽卡记录ID

说明：返回一个抽卡记录的recordID，可使用此ID做抽卡拉取或其他自动化处理

鉴权：密钥

版本支持：1.2.16/1.0

| 字段     | 值范围               |
| ------ | ----------------- |
| method | getCloudRecordKey |

| 参数字段     | 值    |
| -------- |:---- |
| token    | 密钥   |
| userName | 用户名称 |

```json
{
    "method": "getCloudRecordKey",
    "requestId": 2056068777093481,
    "params": [
        {
            "key": "token",
            "value": "Haiyu客户端定义的Token"
        },
        {
            "key":"userName",
            "value":"U518734907A"
        }
    ]
}
```

```json
{
    "requestId": 2056068777093481,
    "message": "{\"playerId\":104370585,\"recordId\":\"3bb6ef782e21bf012a10d94881be7ce6\"}",
    "success": true
}
```

### 抽卡另存为

说明：调用Haiyu执行合并抽卡操作并将结果另存到文件中

鉴权：密钥

版本支持：1.2.16/1.0

| 字段     | 值                               |
| ------ | ------------------------------- |
| method | cloud_saveAsCloudRecordResource |

| 参数                                   | 值       |
| ------------------------------------ | ------- |
| token                                | 密钥      |
| userName                             | 指定账号名称  |
| savePath | 另存为磁盘地址 |

```json
{
    "method": "cloud_saveAsCloudRecordResource",
    "requestId": 8882367624257688, //随机
    "params": [
        {
            "key": "token",
            "value": "密钥"
        },
        {
            "key":"userName",
            "value":"userName"
        },
        {
            "key":"savePath",
            "value":"磁盘路径（记得转义字符）"
        }

    ]
}
```

```json
{
    "requestId": 8882367624257688,
    "message": "{\"path\":\"D:\\\\TestSave.json\",\"fileSize\":106095,\"dataCount\":823,\"margeTime\":\"2025-12-12T14:28:37.8070208+08:00\"}",
    "success": true
}
```
