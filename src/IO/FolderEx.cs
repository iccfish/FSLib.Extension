using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSLib.Extension.IO
{
    using System.IO;

    /// <summary>
    /// 针对文件夹处理的公共类
    /// </summary>
    public static class FolderEx
    {
        /// <summary>
        /// 将指定路径文件夹重命名为指定的名字
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void RenameTo(string path, string name) => Directory.Move(path, Path.Combine(Path.GetDirectoryName(path), name));
    }
}
