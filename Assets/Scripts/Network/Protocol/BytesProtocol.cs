using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeaninglessNetwork
{
    public class BytesProtocol : BaseProtocol
    {
        public byte[] bytes;

        public override BaseProtocol Decode(byte[] buff, int startIndex, int length)
        {
            BytesProtocol bytesProtocol = new BytesProtocol();
            bytesProtocol.bytes = new byte[length];
            Array.Copy(buff, startIndex, bytesProtocol.bytes, 0, length);
            return bytesProtocol;
        }

        public override byte[] Encode()
        {
            return bytes;
        }

        public override string GetProtocolName()
        {
            int end=0;
            return GetString(0,ref end);
        }

        public override string GetDescription()
        {
            string str = "";
            if (bytes == null)
            {
                return str;
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                //将每一个字节的ASCII码转为字符串
                int temp = (int)bytes[i];
                str += temp.ToString() + " ";
            }
            return str;
        }

        /// <summary>
        /// 从消息数组中读取一条消息并转换为字符串
        /// </summary>
        public string GetString(int startIndex, ref int end)
        {
            //消息为空 返回空
            if (bytes == null)
            {
                return "";
            }
            //消息长度小于消息头的长度(4个字节)，消息出错，返回空
            if (bytes.Length < startIndex + sizeof(Int32))
            {
                return "";
            }
            //从StartIndex开始的4个字节转为Int32，即为消息体长度
            Int32 strlen = BitConverter.ToInt32(bytes, startIndex);
            //消息长度小于消息头长度+消息体长度，消息出错
            if (bytes.Length < startIndex + sizeof(Int32) + strlen)
            {
                return "";
            }
            string str = Encoding.UTF8.GetString(bytes, startIndex + sizeof(Int32), strlen);
            //结束位置重定
            end = startIndex + sizeof(Int32) + strlen;
            return str;
        }

        /// <summary>
        /// 将字符串转换为消息字节
        /// </summary>
        public void SpliceString(string str)
        {
            Int32 length = str.Length;
            //将4字节的字符串长度转为字节数组
            byte[] lengthBytes = BitConverter.GetBytes(length);
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            if(bytes==null)
            {
                //消息为空时：消息=消息头+消息体
                bytes = lengthBytes.Concat(strBytes).ToArray();
            }
            else
            {
                //消息非空时：消息=其他消息+消息头+消息体
                bytes = bytes.Concat(lengthBytes).Concat(strBytes).ToArray();
            }
        }

        /// <summary>
        /// 从消息数组中读取一条消息并转换为整数型，默认值为0
        /// </summary>
        /// <returns></returns>
        public int GetInt(int startIndex,ref int end)
        {
            if(bytes==null)
            {
                return 0;
            }
            if(bytes.Length<startIndex+sizeof(Int32))
            {
                return 0;
            }
            end = startIndex + sizeof(Int32);
            return BitConverter.ToInt32(bytes, startIndex);
        }
        public int GetInt(int startIndex)
        {
            int i = 0;
            return GetInt(startIndex,ref i);
        }
        public void SpliceInt(int num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if(bytes==null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }

        }

        public float GetFloat(int startIndex, ref int end)
        {
            if (bytes == null)
            {
                return 0;
            }
            if (bytes.Length < startIndex + sizeof(float))
            {
                return 0;
            }
            end = startIndex + sizeof(float);
            return BitConverter.ToSingle(bytes, startIndex);
        }
        public float GetFloat(int startIndex)
        {
            int i = 0;
            return GetFloat(startIndex, ref i);
        }
        public void SpliceFloat(float num)
        {
            byte[] numBytes = BitConverter.GetBytes(num);
            if (bytes == null)
            {
                bytes = numBytes;
            }
            else
            {
                bytes = bytes.Concat(numBytes).ToArray();
            }

        }
    }

}
