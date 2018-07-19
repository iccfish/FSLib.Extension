 #!/bin/sh
 
 version=`grep -Po '(?<=<Version>)([\.\d]+)(?=</Version>)' src/FSLib.Extension.csproj`
 
 echo "新版本号 $version"

echo 正在更新AssemblyInfo.cs ……
sed -i -r "s/\[assembly: Assembly(File)?Version\(\"[0-9\.]+\"\)\]/[assembly: Assembly\1Version(\"${version}\")]/" src/Properties/AssemblyInfo.cs

echo 正在更新Config文件……
sed -i -r "s/newVersion=\"[0-9\.]+\"/newVersion=\"$version\"/" src/content/App.config.install.xdt
sed -i -r "s/newVersion=\"[0-9\.]+\"/newVersion=\"$version\"/" src/content/Web.config.install.xdt

echo 更新完成！
read