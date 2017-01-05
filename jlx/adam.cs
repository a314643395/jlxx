using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jlx
{
    /*
     Copyright © 2015，Adam，All Rights Reserved.
     * Copyright ownership belongs to JLX,shall not be reproduced ,
     * copied,or used in other ways without permission.Otherwise Jlx will have the right to pursue legal responsibilities.
     */
    public class gin
    {
      static  byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

      /// <summary>
      /// 加密
      /// </summary>
      /// <param name="text">加密的字符串</param>
      /// <param name="encryptKey">秘钥</param>
      /// <returns>返回值</returns>
      public static string OnKey(string text,string encryptKey)//加密
      {
          try
          {
              byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
              byte[] rgbIV = Keys;
              byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
              DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
              MemoryStream mStream = new MemoryStream();
              CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
              cStream.Write(inputByteArray, 0, inputByteArray.Length);
              cStream.FlushFinalBlock();
              return Convert.ToBase64String(mStream.ToArray());
          }
          catch
          {
              return text;
          }
        }
      /// <summary>
      /// 解密
      /// </summary>
      /// <param name="text">加密后的字符串</param>
      /// <param name="decryptKey">秘钥</param>
      /// <returns>返回值</returns>
      public static string OffKey(string text, string decryptKey) //解密
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(text);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return text;
            }
        }

 
    }
    /// <summary>
    /// 获取IP和地址类
    /// </summary>
    public class GetIpAddress
    {
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://www.ip138.com/ips138.asp");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据
                int start = all.IndexOf("您的IP地址是：[") + 9;
                int end = all.IndexOf("]", start);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }
        /// <summary>
        /// 获取对应物理地址
        /// </summary>
        /// <returns></returns>
        public string GetAddress(string add)
        {
            string address = "";
            try
            {
                string s1 = "http://www.ip138.com/ips1388.asp?ip=";
                string s2 = "&action=2";
                WebRequest wr = WebRequest.Create(s1 + add + s2);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据
                int start = all.IndexOf("本站主数据：");
                int end = all.IndexOf("</li><li>参考数据一：", start);
                address = all.Substring(start, end - start);
                address = address.Replace("本站主数据：", "IP信息：");
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return address;
        }
    }
    /// <summary>
    /// 获取时间
    /// </summary>
    public class timer
    {
        /// <summary>
        /// 获取北京时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetBeiJingTime()
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://open.baidu.com/special/time/");//百度北京时间地址

            req.Headers.Add("content", "text/html; charset=utf-8");

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            Stream stream = res.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));

            string html = sr.ReadToEnd();

            string time = GetRegexStr(html, "(?<=baidu_time\\().*?(?=\\))").Substring(0, 10);//这里是时间戳 不是时间 要转换 

            stream.Dispose();

            sr.Dispose();

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            long lTime = long.Parse(time + "0000000");

            TimeSpan toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);

        }



        private static string GetRegexStr(string html, string regex)
        {

            Regex reg = new Regex(regex);

            string result = reg.Matches(html)[0].Value;

            return result;

        }
    }

/// <summary>
/// INI文件操作函数
/// </summary>
    public class OpenIni
    { 
    
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 检查文件是否存在，不存在就创建该配置文件
        /// </summary>
        /// <param name="FileName">文件名</param>
        public void inspect(string FileName)
        {
            if (File.Exists(@".\" + FileName))
            {
                return;
            }
            else
            {
                FileInfo fi = new FileInfo(FileName);
                fi.Create().Close();
            }
        }

       #region 读Ini文件
        /// <summary>
        /// 读取INI文件内容
        /// </summary>
        /// <param name="Section">节点</param>
        /// <param name="Key">字段</param>
        /// <param name="NoText">字段值</param>
        /// <param name="iniFilePath">文件地址</param>
        /// <returns></returns>
        public static string ReadIniData(string Section,string Key,string NoText,string iniFilePath)
        {
            if(File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section,Key,NoText,temp,1024,iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

       #region 写Ini文件
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section">节</param>
        /// <param name="Key">字段</param>
        /// <param name="Value">字段值</param>
        /// <param name="iniFilePath">地址</param>
        /// <returns></returns>
        public static bool WriteIniData(string Section,string Key,string Value,string iniFilePath)
        {
            if(File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section,Key,Value,iniFilePath);    
                if(OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

   
    }

}
