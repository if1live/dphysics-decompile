# DPhysics-decompile
Decompile [DPhysics](https://www.assetstore.unity3d.com/en/#!/content/36206)

* [DPhysics Evaluation](https://www.assetstore.unity3d.com/en/#!/content/36206)
* Version: 1.0 (May 28, 2015)
* Where is decompiled source : Assets/DPhysics/Core/Scripts/

## Why I decompile it?

![visual studio compile error](https://raw.githubusercontent.com/if1live/dphysics-decompile/master/screenshots/vs-error.png)

```
1>------ Build started: Project: dphysics-decompile.CSharp, Configuration: Debug Any CPU ------
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "3.5.0.0" in the current target framework.
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3268: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the framework assembly "System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which could not be resolved in the currently targeted framework. ".NETFramework,Version=v3.5,Profile=Unity Subset v3.5". To resolve this problem, either remove the reference "DPhysics_Core" or retarget your application to a framework version which contains "System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089".
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Security, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
1>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Data.SqlXml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
1>D:\devel\dphysics-decompile\Assets\DPhysics\Examples\Newton's Balls\NewtonsBalls.cs(3,7,3,15): error CS0246: The type or namespace name 'DPhysics' could not be found (are you missing a using directive or an assembly reference?)
1>D:\devel\dphysics-decompile\Assets\DPhysics\Examples\Newton's Box\NewtonsBox.cs(3,7,3,15): error CS0246: The type or namespace name 'DPhysics' could not be found (are you missing a using directive or an assembly reference?)
1>D:\devel\dphysics-decompile\Assets\DPhysics\Multiplayer\Scripts\Client.cs(5,7,5,15): error CS0246: The type or namespace name 'DPhysics' could not be found (are you missing a using directive or an assembly reference?)
1>D:\devel\dphysics-decompile\Assets\DPhysics\Multiplayer\Scripts\Miscellaneous\TestCommands.cs(3,7,3,15): error CS0246: The type or namespace name 'DPhysics' could not be found (are you missing a using directive or an assembly reference?)
1>D:\devel\dphysics-decompile\Assets\DPhysics\Multiplayer\Scripts\Command.cs(23,9,23,17): error CS0246: The type or namespace name 'Vector2d' could not be found (are you missing a using directive or an assembly reference?)
1>D:\devel\dphysics-decompile\Assets\DPhysics\Multiplayer\Scripts\Command.cs(32,10,32,18): error CS0246: The type or namespace name 'Vector2d' could not be found (are you missing a using directive or an assembly reference?)
2>------ Build started: Project: dphysics-decompile.CSharp.Editor, Configuration: Debug Any CPU ------
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "3.5.0.0" in the current target framework.
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3268: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the framework assembly "System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which could not be resolved in the currently targeted framework. ".NETFramework,Version=v3.5,Profile=Unity Full v3.5". To resolve this problem, either remove the reference "DPhysics_Core" or retarget your application to a framework version which contains "System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089".
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Security, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
2>C:\Program Files (x86)\MSBuild\14.0\bin\Microsoft.Common.CurrentVersion.targets(1819,5): warning MSB3258: The primary reference "DPhysics_Core" could not be resolved because it has an indirect dependency on the .NET Framework assembly "System.Data.SqlXml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" which has a higher version "4.0.0.0" than the version "2.0.0.0" in the current target framework.
2>CSC : error CS0006: Metadata file 'D:\devel\dphysics-decompile\Temp\UnityVS_bin\Debug\Assembly-CSharp.dll' could not be found
========== Build: 0 succeeded, 2 failed, 0 up-to-date, 0 skipped ==========
```

When I use DPhysics, I see above compile error message.
This problem is caused by CLR DLL. DLL references C# 4.0 libraray.

![decompile DLL](https://raw.githubusercontent.com/if1live/dphysics-decompile/master/screenshots/decompile-dll.png)

## Decompiel progress
Decompile `Assets/DPhysics/Core/Plugins/DPhysics_Core.dll`, http://www.telerik.com/products/decompiler.aspx

Find `ret = new FInt()`, `ret = new Vector2d()` and change them to comment, if ret is function argument and out value.

Before 
```
public void AbsoluteValue(out FInt ret)
{
	ret = new FInt();
	....
```

After 
```
public void AbsoluteValue(out FInt ret)
{
	//ret = new FInt();
	....
```

Why? If ret and this is same, something is wrong.

```
public void Subtract(int OtherValue, out FInt ret)
{
	ret = new FInt();
	ret.RawValue = this.RawValue - ((long)OtherValue << 20);
}
...
// something wrong!
FInt v = new FInt(10);
v.Substract(v);
```

## Reference
* Unity3d forum
  * http://forum.unity3d.com/threads/dphysics-deterministic-2d-physics-engine.315553/

