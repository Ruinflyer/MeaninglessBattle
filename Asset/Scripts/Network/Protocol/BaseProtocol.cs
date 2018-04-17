using System;
using System.Collections.Generic;
using System.Text;

namespace MeaninglessNetwork
{
    public class BaseProtocol
    {
        /// <summary>
        /// 解码-从buff缓冲区中的第startIndex个下标开始读取length长度的字节
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual BaseProtocol Decode(byte[] buff,int startIndex,int length)
        {
            return new BaseProtocol();
        }

        public virtual byte[] Encode()
        {
            return new byte[] { };
        }

        /// <summary>
        /// 获取协议名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetProtocolName()
        {
            return "";
        }

        /// <summary>
        /// 获取协议描述
        /// </summary>
        /// <returns></returns>
        public virtual string GetDescription()
        {
            return "";
        }
    }
}
