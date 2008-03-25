using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace ServerLight
{
    public static class PathHelper
    {
        /// <summary>
        /// Get a relative path from a reference path and a full path. 
        /// <example>reference path is c:\test\proj.wbproj
        /// full path is c:\example\coucou.txt
        /// result is ..\coucou.txt</example>
        /// Note : A directory path string must end by a '\' ex: use 'c:\test\' and not 'c:\test' witch is interpreted like a 'test' file in 'c:\'
        /// </summary>
        /// <param name="referencePath"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string ToRelativePath(DirectoryInfo referencePath, FileInfo fullPath)
        {
            string relative = ToRelativePath(referencePath, fullPath.Directory);
            return relative + fullPath.Name;
        }

        /// <summary>
        /// Get a relative path from a reference path and a full path. 
        /// <example>reference path is c:\test\proj.wbproj
        /// full path is c:\example\coucou.txt
        /// result is ..\coucou.txt</example>
        /// Remember that a directory path string must end by a '\' ex: use 'c:\test\' and not 'c:\test' which is the 'test' file in 'c:\'
        /// </summary>
        /// <param name="referencePathDirectoryInfo"></param>
        /// <param name="fullPathDirectoryInfo"></param>
        /// <returns></returns>
        public static string ToRelativePath(DirectoryInfo referencePathDirectoryInfo, DirectoryInfo fullPathDirectoryInfo)
        {
            Debug.Assert(referencePathDirectoryInfo != null);
            Debug.Assert(fullPathDirectoryInfo != null);

            if (!referencePathDirectoryInfo.Root.FullName.Equals(fullPathDirectoryInfo.Root.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return fullPathDirectoryInfo.FullName;
            }

            string[] referencePathSplit = SplitPath(referencePathDirectoryInfo.FullName);
            string[] fullPathSplit = SplitPath(fullPathDirectoryInfo.FullName);
            int length = Math.Min(referencePathSplit.Length, fullPathSplit.Length);
            int commonPartIndex = 0;
            for (int i = 0; i < length; i++)
            {
                if (referencePathSplit[i].Equals(fullPathSplit[i]))
                {
                    commonPartIndex = i;
                }
                else
                {
                    break;
                }
            }
            StringBuilder relativePath = new StringBuilder();
            if (commonPartIndex == referencePathSplit.Length - 1)
            {
                relativePath.Append(".\\");
            }
            else
            {
                for (int i = commonPartIndex + 1; i < referencePathSplit.Length; i++)
                {
                    relativePath.Append("..\\");
                }
            }
            for (int i = commonPartIndex + 1; i < fullPathSplit.Length; i++)
            {
                relativePath.Append(fullPathSplit[i]);
                relativePath.Append("\\");
            }
            return relativePath.ToString();
        }

        /// <summary>
        /// Get a full path from a reference path and a relative path. 
        /// </summary>
        /// <param name="referencePath"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string FromRelativePath(DirectoryInfo referencePath, string relativePath)
        {
            Debug.Assert(referencePath != null);
            Debug.Assert(relativePath != null);

            //Is this a relative path
            if (!relativePath.StartsWith("."))
            {
                return relativePath;
            }

            //1 find common part
            string[] referenceSplit = SplitPath(referencePath.FullName);
            string[] relativePathSplit = new string[0];
            StringBuilder fullPath = new StringBuilder();
            string fileName = string.Empty;
            if (relativePath.EndsWith("\\"))
            {
                relativePathSplit = SplitPath(relativePath);
            }
            else if (!string.IsNullOrEmpty(relativePath))
            {
                relativePathSplit = SplitPath(relativePath.Substring(0, relativePath.LastIndexOf('\\')));
                fileName = relativePath.Substring(relativePath.LastIndexOf('\\') + 1);
            }

            int numDirectoryUp = Regex.Matches(relativePath, @"\.\.").Count;


            for (int i = 0; i < referenceSplit.Length - numDirectoryUp; i++)
            {
                fullPath.Append(referenceSplit[i]);
                fullPath.Append("\\");
            }
            if (numDirectoryUp == 0)
            {
                numDirectoryUp = 1;
            }
            for (int i = numDirectoryUp; i < relativePathSplit.Length; i++)
            {
                fullPath.Append(relativePathSplit[i]);
                fullPath.Append("\\");
            }
            fullPath.Append(fileName);
            return fullPath.ToString();
        }

        private static string[] SplitPath(string path)
        {
            string referencePathString = string.Empty;
            if (!path.EndsWith("\\"))
            {
                referencePathString = path + "\\";
            }
            else
            {
                referencePathString = path;
            }
            string[] fromSplit = referencePathString.Split('\\');
            if (referencePathString[referencePathString.Length - 1] != '\\')
            {
                string[] fromSplit2 = new string[fromSplit.Length - 1];
                Array.Copy(fromSplit, fromSplit2, fromSplit.Length - 1);
                fromSplit = fromSplit2;
            }
            if (string.IsNullOrEmpty(fromSplit[fromSplit.Length - 1]))
            {
                string[] fromSplit2 = new string[fromSplit.Length - 1];
                Array.Copy(fromSplit, fromSplit2, fromSplit.Length - 1);
                return fromSplit2;
            }
            return fromSplit;
        }


    }

#if DEBUG
    [TestFixture]
    public class PathHelperTestFixture
    {
        [Test]
        public void ToRelativePath()
        {
            //Project
            string result = PathHelper.ToRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), new FileInfo(@"C:\root\coucou.txt"));
            Assert.AreEqual(result, @"..\..\coucou.txt");

            result = PathHelper.ToRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), new DirectoryInfo(@"C:\root\sub1\"));
            Assert.AreEqual(result, @"..\");

            result = PathHelper.ToRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), new DirectoryInfo(@"C:\root\sub1\sub2\"));
            Assert.AreEqual(result, @".\");

            result = PathHelper.ToRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), new DirectoryInfo(@"C:\root\sub1\sub2\sub3\"));
            Assert.AreEqual(result, @".\sub3\");

            result = PathHelper.ToRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), new DirectoryInfo(@"C:\root\sub2"));
            Assert.AreEqual(result, @"..\..\sub2\");
        }

        [Test]
        public void FromRelativePath()
        {
            string result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), @"C:\root\sub1\sub2\");
            Assert.AreEqual(result, @"C:\root\sub1\sub2\");

            result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\"), @"D:\root\");
            Assert.AreEqual(result, @"D:\root\");

            result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), @".\");
            Assert.AreEqual(result, @"C:\root\sub1\sub2\");

            result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), @".\sub3\");
            Assert.AreEqual(result, @"C:\root\sub1\sub2\sub3\");

            result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), @"..\..\sub2\");
            Assert.AreEqual(result, @"C:\root\sub2\");
            
            result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), @"..\");
            Assert.AreEqual(result, @"C:\root\sub1\");

            result = PathHelper.FromRelativePath(new DirectoryInfo(@"C:\root\sub1\sub2\"), @"..\..\coucou.txt");
            Assert.AreEqual(result, @"C:\root\coucou.txt");

        }
    }
#endif    
}