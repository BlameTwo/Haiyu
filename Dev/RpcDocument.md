# Haiyu Rpc

此协议为Haiyu客户端定义的远程调用协议，当前版本为实验性功能，不保证后续追加功能。

通信协议：WebSocket

通信格式：Json

#### 免责声明

此协议不得使用在爬虫、逆向分析、损坏玩家账号等行为，如若出现账号封禁、计算机软硬件损坏与Haiyu无关。

## 协议定义

地址：ws://localhost:9084/rpc

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

示例

```json
{
    "requestId": 4313221365652805,
    "message": "{\"playerId\":104370585,\"recordId\":\"3bb6ef782e21bf012a10d94881be7ce6\"}",
    "success": true
}
```

## 云工具

### 用户列表

| 字段     | 值范围           |
| ------ | ------------- |
| method | getCloudUsers |

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
    "requestId": 5613182138882222,
    "message": "[\"U518734907A\"]",
    "success": true
}
```



### 抽卡记录ID

请求体定义

| 字段     | 值范围               |
| ------ | ----------------- |
| method | getCloudRecordKey |

| 参数字段     | 值            |
| -------- |:------------ |
| token    | Haiyu客户端设置密钥 |
| userName | 用户名称         |

```json
{
    "method": "getCloudRecordKey",
    "requestId": 14215,
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


