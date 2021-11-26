## 简易使用说明

- 所有Manager管理器都可以通过XXXManager.GetInstance()获取并调用

### PoolManager

缓存池，可以缓解太频繁地物体生成删除导致游戏卡顿

```c#
PoolManager.GetInstance().PushObj(string,GameObject)
```

string是要放入缓存池的这个物体的名字，例如"子弹"

GameObject是要放入缓存池的这个物体

```c#
PoolManager.GetInstance().GetObj(string,UnityAction<GameObject>)
```

string是要从缓存池中取出的名字，要和上面放入时候的名字相同

第二个参数类似一段代码块，例如：

```c#
PoolMgr.GetInstance().GetObj("prefab/bullet", (o) =>
            {
                o.transform.position = transform.position;
                o.GetComponent<Bullet>().Init(parameter.attack, new Vector2(transform.localScale.x, -1));
                o.transform.SetParent(this.transform.parent);
            });
```

这段代码大体意思就是从缓存池获取一个叫“子弹”的物体o（GameObject类型），获取的时候：

①修改他的坐标

②让子弹初始化(比如有可能子弹伤害会随飞行距离逐渐增加，这时候从缓存池提出来原本累计的伤害还在，需要初始化一下)

③设置父物体

要在Assets下创建一个Resources的文件夹，里面放对应的物体，物体名字是Resources文件夹下的路径，才能正常读取，比如上面的代码就表示获取Assets/Resources/prefab/bullet这个预制体

##### 注意：

游戏里不要出现名字相同的物体，不然放入缓存池会有bug



### ResManager

用于加载资源，缓存池会调用这里面的函数，应该用不到

### MonoManager/MonoController

可以执行帧事件，可以增加或减少帧事件，一些通用的帧事件可以在添加到这里面

也可以添加协程

应该也用不到